using Anura.ConfigurationModule.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BotAI : MonoBehaviour
{
    public struct Location
    {
        public Vector3 position;
        public Vector3 launchPosition;
        public int direction;  // leftOfOrigin=-1, origin=0, rightOfOrigin=1
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

    private Bounds Bounds => Collider.bounds;
    private Vector3 Center => Bounds.center;

    private Rigidbody2D Rigidbody { get; set; }
    private Collider2D Collider { get; set; }
    public Transform LaunchPoint { get; set; }

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

    private IEnumerator PlayTurn()
    {
        int moveDir = 0;
        Location choice;

        do
        {
            List<Location> potentialLocations = GetPotentialLocations(moveDir != -1, moveDir != 1);
            choice = ChooseLocation(potentialLocations);
            moveDir = choice.direction;

            DebugLocation(choice);

            yield return StartCoroutine(MoveTo(choice));
            yield return new WaitForSeconds(2);
        }
        while (repeatMoveTo && TimeLeft > 2);        

        yield return StartCoroutine(Shoot(choice));
    }

    private List<Location> GetPotentialLocations(bool left, bool right)
    {
        var result = new List<Location> { GetLocation() };

        float movementStep = Bounds.size.x * 0.9f;

        for (int i = 1; i <= 1; i++)
        {
            if (left) AddLocationIfNew(ref result, GetLocation(-1, i * movementStep));
            if (right) AddLocationIfNew(ref result, GetLocation(1, i * movementStep));
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

    private Location ChooseLocation(List<Location> locations)
    {
        CalculateLocationScores(ref locations);
        locations = locations.OrderByDescending(x => x.score).ToList();
        return locations[0];
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

    private IEnumerator Shoot(Location location)
    {
        yield return null;
        yield return null;

        api.weaponIdx = Random.Range(0, 3);
        api.SelectWeapon();

        yield return null;
        api.shootAngle = location.bestAngle; //Random.Range(125, 145);
        api.shootPower = location.bestPower; //Random.Range(0.75f, 1);
        api.SelectAnglePower();

        Debug.Log("Best distance: " + location.bestDistanceToEnemy);
        Debug.Log("location.bestAngle: " + location.bestAngle);
        Debug.Log("location.bestPower: " + location.bestPower);
        Debug.Log("Best score: " + location.score);

        yield return null;        
        api.Shoot();
    }

    private Location GetLocation()
    {
        return new Location
        {
            direction = 0,
            position = Center,
            launchPosition = LaunchPoint.position,
            directHit = false
        };
    }

    private Location GetLocation(int direction, float xOffset)
    {
        Vector3 newPosition = TerrainNavigationLibrary.GetPositionAtXDisplacement(
                Bounds,
                GetTerrainDirection(direction),
                xOffset);

        return new Location
        {
            direction = direction,
            position = newPosition,
            launchPosition = newPosition + (LaunchPoint.position - Center),
            directHit = false
        };
    }

    private TerrainNavigationLibrary.Direction GetTerrainDirection(int d)
    {
        return d == -1 ? TerrainNavigationLibrary.Direction.Left : TerrainNavigationLibrary.Direction.Right;
    }

    private void CalculateLocationScores(ref List<Location> locations)
    {
        for (int i=0; i<locations.Count; i++)
        {
            Location l = locations[i];
            BotAIAim.SimulateLocationAim(ref l);

            if (l.directHit) l.score = 0.5f;
            l.score += Mathf.Lerp(0.5f, 0, Mathf.Clamp(l.bestDistanceToEnemy, 0, 5) / 5);

            locations[i] = l;
        }
    }
}