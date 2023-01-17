using Anura.ConfigurationModule.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class BotAI : MonoBehaviour
{
    public struct Location
    {
        public int stepsFromOrigin;
        public Vector3 position;
        public Vector3 launchPosition;
        public int direction;  // leftOfOrigin=-1, origin=0, rightOfOrigin=1
        public int enemyDirection;  // leftOfOrigin=-1, rightOfOrigin=1
        public float score;
        public float bestAngle;
        public float bestPower;
        public float bestDistanceToEnemy;
        public bool directHit;
    }

    private const float MOVEMENT_ARRIVAL = 0.2f;
    private const int UPDATE_FRAMES = 10;
    private const float UPDATE_TIME_STUCK = 3;

    private BotPlayerAPI api = BotPlayerAPI.Instance;
    private float TimeLeft => RoomStateManager.Instance.Timer.TimeLeft;
    private BotConfiguration Configuration => BotManager.Instance.configuration;

    private Bounds Bounds => Collider.bounds;
    private Vector3 Center => Bounds.center;

    private float MapMinY { get; set; }
    private float MapMaxY { get; set; }
    private List<int> WeaponsWeightedList { get; set; }

    private Rigidbody2D Rigidbody { get; set; }
    private Collider2D Collider { get; set; }
    public Transform LaunchPoint { get; set; }

    private Vector3 EnemyPosition => BotManager.Instance.Enemy[0].transform.position;
    private float MovementStep => Bounds.size.x * 0.9f;

    private BotAIAim BotAIAim { get; set; }

    // For debugging, to be deleted
    #region Debugging
    private bool debug = false;
    private List<GameObject> debugGOs = new List<GameObject>();
    private void DebugLocation(Location l, float scale = 1)
    {
        if (!debug) return;

        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        g.transform.position = l.position;
        g.transform.localScale = Vector3.one * scale;
        Destroy(g.GetComponent<Collider>());
        debugGOs.Add(g);
    }
    #endregion

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();

        MapMinY = BotManager.Instance.leftMapBound.bounds.min.y;
        MapMaxY = BotManager.Instance.leftMapBound.bounds.max.y;

        WeaponsWeightedList = new List<int>();
        for (int i = 0; i < Configuration.weaponWeights.Length; i++)
            for (int j = 0; j < Configuration.weaponWeights[i]; j++)
                WeaponsWeightedList.Add(i);

        BotAIAim = new BotAIAim(BotManager.Instance.Enemy.ToArray());
    }

    public void Play()
    {
        foreach (var g in debugGOs) Destroy(g);
        debugGOs.Clear();

        StartCoroutine(PlayTurn());
    }

    public void Wait()
    {
    }

    private Location? turnChoice = null;

    private IEnumerator PlayTurn()
    {
        int moveDir = 0;
        turnChoice = null;

        do
        {
            List<Location> potentialLocations = GetPotentialLocations(moveDir != -1, moveDir != 1);

            yield return StartCoroutine(ChooseLocation(potentialLocations));
            if (turnChoice == null) yield break;

            Location choice = turnChoice.Value;
            moveDir = choice.direction;
            DebugLocation(choice);

            yield return StartCoroutine(MoveTo(choice));
            yield return new WaitForSeconds(2);
        }
        while (repeatMoveTo && TimeLeft > 2);        

        yield return StartCoroutine(Shoot());
    }

    private List<Location> GetPotentialLocations(bool left, bool right)
    {
        var result = new List<Location> { GetLocation() };

        for (int i = 1; i <= Configuration.maxTravelSteps; i++)
        {
            if (left) AddLocationIfNew(ref result, GetLocation(-1, i));
            if (right) AddLocationIfNew(ref result, GetLocation(1, i));
        }

        foreach (var l in result) DebugLocation(l, 0.5f);

        return result;
    }

    private void AddLocationIfNew(ref List<Location> list, Location location)
    {
        foreach (var l in list)
        {
            if (Vector3.Distance(l.position, location.position) < MOVEMENT_ARRIVAL)
                return;
        }

        list.Add(location);
    }

    private IEnumerator ChooseLocation(List<Location> locations)
    {
        for (int i = 0; i < locations.Count; i++)
        {
            Location l = locations[i];

            BotAIAim.SimulateLocationAim(ref l);
            EvaluateLocationScore(ref l);

            locations[i] = l;

            yield return null;
            yield return null;
        }

        locations = locations.OrderByDescending(x => x.score).ToList();

        turnChoice = locations[0];
    }

    private void EvaluateLocationScore(ref Location location)
    {
        float score = 0;

        if (Configuration.weightDirectHit > 0)
        {
            if (location.directHit) score += Configuration.weightDirectHit;
        }

        if (Configuration.weightBestShotDistance > 0)
        {
            float maxDist = 5 * Bounds.size.x;
            score += Mathf.Lerp(Configuration.weightBestShotDistance, 0, Mathf.Clamp(location.bestDistanceToEnemy, 0, maxDist) / maxDist);
        }

        if (Configuration.weightDirectionImportance > 0)
        {
            float s = Configuration.maxTravelSteps * location.enemyDirection;
            score += Mathf.InverseLerp(s, -s, location.stepsFromOrigin) * Configuration.weightDirectionImportance;
        }

        if (Configuration.weightHeightImportance > 0)
        {
            float heightFraction = Mathf.InverseLerp(MapMinY, MapMaxY, location.position.y);
            score += Mathf.Lerp(0, Configuration.weightHeightImportance, heightFraction);
        }

        location.score = score / Configuration.WeightsTotal;
    }

    private bool repeatMoveTo = false;

    private IEnumerator MoveTo(Location location)
    {
        yield return null;
        yield return null;
        repeatMoveTo = false;

        api.Move(location.direction);

        int counter = 0;

        float stuckTime = Time.time;
        Vector3 stuckPos = Center;
        
        while (Mathf.Abs(Center.x - location.position.x) > MOVEMENT_ARRIVAL)
        {            
            yield return null;

            if (counter % UPDATE_FRAMES == 0)
            {
                // Change direction
                if ((location.position.x - Center.x) * location.direction < 0)
                {
                    location.direction *= -1;
                    api.CancelMove();
                    yield return null;
                    api.Move(location.direction);
                }
                else
                {
                    // Jump
                    if (Mathf.Abs(Rigidbody.velocity.x) < 0.01f)
                    {
                        api.Jump();
                    }
                }                
            }
            // Check if stuck
            else if (Time.time - stuckTime >= UPDATE_TIME_STUCK)
            {
                if (Vector3.Distance(Center, stuckPos) < Bounds.size.x)
                {
                    repeatMoveTo = true;
                    break;
                }
                stuckTime = Time.time;
                stuckPos = Center;
            }

            counter++;
        }

        api.CancelMove();
    }

    private IEnumerator Shoot()
    {
        if (turnChoice is not Location location) yield break;

        yield return null;
        yield return null;

        api.weaponIdx = GetWeightedRandomWeapon();
        api.SelectWeapon();

        yield return null;
        api.shootAngle = location.bestAngle;
        api.shootPower = location.bestPower;
        api.SelectAnglePower();

        Debug.Log("Shooting::distance: " + location.bestDistanceToEnemy);
        Debug.Log("Shooting::angle: " + location.bestAngle);
        Debug.Log("Shooting::power: " + location.bestPower);
        Debug.Log("Shooting::score: " + location.score);

        yield return null;        
        api.Shoot();
    }

    private Location GetLocation()
    {
        return new Location
        {
            direction = 0,
            stepsFromOrigin = 0,
            position = Center,
            launchPosition = LaunchPoint.position,
            directHit = false,
            enemyDirection = GetEnemyDirection(Center)
        };
    }

    private Location GetLocation(int direction, int steps)
    {
        float xOffset = steps * MovementStep;

        Vector3 newPosition = TerrainNavigationLibrary.GetPositionAtXDisplacement(
                Bounds,
                GetTerrainDirection(direction),
                xOffset);

        return new Location
        {
            direction = direction,
            enemyDirection = GetEnemyDirection(newPosition),
            stepsFromOrigin = direction * steps,
            position = newPosition,
            launchPosition = newPosition + (LaunchPoint.position - Center),
            directHit = false
        };
    }

    private int GetWeightedRandomWeapon()
    {
        return WeaponsWeightedList[Random.Range(0, WeaponsWeightedList.Count)];
    }

    private int GetEnemyDirection(Vector3 pos)
    {
        float xDif = pos.x - EnemyPosition.x;
        if (xDif > 0) return -1;
        return 1;
    }

    private TerrainNavigationLibrary.Direction GetTerrainDirection(int d)
    {
        return d == -1 ? TerrainNavigationLibrary.Direction.Left : TerrainNavigationLibrary.Direction.Right;
    }
}