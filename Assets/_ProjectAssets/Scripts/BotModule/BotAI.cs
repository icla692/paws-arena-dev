using Anura.ConfigurationModule.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static BotAIAim;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BotAI : MonoBehaviour
{
    public class LocationSim
    {
        public Location location;
        public float score;
        public float angle;
        public float power;
        public float distanceToEnemy;
        public bool directHit;
        public bool simulated;
        public float eta;

        public LocationSim(Location location)
        {
            this.location = location;
            score = 0;
            angle = Random.Range(0, 360);
            power = Random.Range(0, 1f);
            distanceToEnemy = Mathf.Infinity;
            eta = Mathf.Infinity;
            directHit = false;
            simulated = false;
        }
    }

    public class Location
    {
        public int stepsFromOrigin;
        public Vector3 position;
        public Vector3 launchPosition;
        public int direction;  // leftOfOrigin=-1, origin=0, rightOfOrigin=1
        public int enemyDirection;  // leftOfOrigin=-1, rightOfOrigin=1
        public Dictionary<Weapon, LocationSim> locationSims = new Dictionary<Weapon, LocationSim>();

        public Location(BotAI ai, int stepsFromOrigin, int direction, Vector3 position, Vector3 launchPosition)
        {
            this.stepsFromOrigin = stepsFromOrigin;
            this.direction = direction;
            this.position = position;
            this.launchPosition = launchPosition;
            enemyDirection = ai.GetEnemyDirection(position);
        }
    }

    private const float MOVEMENT_ARRIVAL = 0.2f;
    private const int UPDATE_FRAMES = 10;
    private const float UPDATE_TIME_STUCK = 3;

    private const int DECISION_LOC_TOP_CHOICES = 10;
    private const float DECISION_LOC_TOP_MARGIN = 0.05f;

    private BotPlayerAPI api = BotPlayerAPI.Instance;
    private float TimeLeft => RoomStateManager.Instance.Timer.TimeLeft;
    private BotConfiguration Configuration => BotManager.Instance.configuration;

    private Bounds Bounds => Collider.bounds;
    private Vector3 Center => Bounds.center;

    public float MapMinY { get; private set; }
    public float MapMaxY { get; private set; }

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

    private void DebugList<K>(List<K> list)
    {
        string s = "";
        for (int i = 0; i < list.Count; i++)
            s += list[i].ToString() + " ";
        Debug.Log(s);
    }
    #endregion

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();

        MapMinY = BotManager.Instance.leftMapBound.bounds.min.y;
        MapMaxY = BotManager.Instance.leftMapBound.bounds.max.y;

        BotAIAim = new BotAIAim(this, BotManager.Instance.Enemy.ToArray());
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

    private Location chosenLocation = null;
    private Weapon chosenWeapon;

    private IEnumerator PlayTurn()
    {
        int moveDir = 0;
        chosenLocation = null;

        do
        {
            List<Location> potentialLocations = GetPotentialLocations(moveDir != -1, moveDir != 1);

            yield return StartCoroutine(ChooseLocation(potentialLocations));
            if (chosenLocation == null) yield break;

            Location choice = chosenLocation;
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
        BotAIAim.StartSimulation(locations);
        yield return new WaitWhile(() => BotAIAim.Simulating);
        locations = BotAIAim.GetSimulationResults();

        for (int i = 0; i < locations.Count; i++)
        {
            EvaluateLocationScores(locations[i]);
        }

        DecideLocation(locations);
    }

    private void DecideLocation(List<Location> locations)
    {
        List<KeyValuePair<Weapon, LocationSim>> sims = new List<KeyValuePair<Weapon, LocationSim>>();
        foreach (Location l in locations)
            sims.AddRange(l.locationSims.Where(x => x.Value.simulated));

        var ordered = sims.OrderByDescending(x => x.Value.score).Take(DECISION_LOC_TOP_CHOICES);
        float marginScore = ordered.First().Value.score * (1 - DECISION_LOC_TOP_MARGIN);

        List<Weapon> weightedList = new List<Weapon>();

        foreach (var entry in ordered.Where(x => x.Value.score >= marginScore))
        {
            for (int i = 0; i < BotAIAim.WeaponsData[entry.Key].weight; i++)
                weightedList.Add(entry.Key);
        }

        chosenWeapon = weightedList[Random.Range(0, weightedList.Count)];
        chosenLocation = ordered.First(x => x.Key == chosenWeapon).Value.location;
    }

    private void EvaluateLocationScores(Location location)
    {
        foreach (var sim in location.locationSims)
        {
            if (!sim.Value.simulated) continue;

            float score = 0;

            if (Configuration.weightDirectHit > 0)
            {
                if (sim.Value.directHit) score += Configuration.weightDirectHit;
            }

            if (Configuration.weightBestShotDistance > 0)
            {
                float maxDist = 5 * Bounds.size.x;
                score += Mathf.Lerp(Configuration.weightBestShotDistance, 0, Mathf.Clamp(sim.Value.distanceToEnemy, 0, maxDist) / maxDist);
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

            sim.Value.score = score / Configuration.WeightsTotal;
        }        
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
        if (chosenLocation == null) yield break;

        yield return null;
        yield return null;

        api.weaponIdx = BotAIAim.WeaponsData[chosenWeapon].internalIndex;
        api.SelectWeapon();

        yield return null;
        api.shootAngle = chosenLocation.locationSims[chosenWeapon].angle;
        api.shootPower = chosenLocation.locationSims[chosenWeapon].power;
        api.SelectAnglePower();

        if (Configuration.debugBotAI)
        {
            Debug.Log("BotAI: Shooting::weapon: " + chosenWeapon);
            Debug.Log("BotAI: Shooting::distance: " + chosenLocation.locationSims[chosenWeapon].distanceToEnemy);
            Debug.Log("BotAI: Shooting::angle: " + chosenLocation.locationSims[chosenWeapon].angle);
            Debug.Log("BotAI: Shooting::power: " + chosenLocation.locationSims[chosenWeapon].power);
            Debug.Log("BotAI: Shooting::score: " + chosenLocation.locationSims[chosenWeapon].score);
        }            

        yield return null;        
        api.Shoot();

        if (chosenWeapon == Weapon.Split)
        {
            yield return new WaitForSeconds(chosenLocation.locationSims[chosenWeapon].eta - 1);
            api.Shoot();
        }        
    }

    private Location GetLocation()
    {
        return new Location(this, 0, 0, Center, LaunchPoint.position);
    }

    private Location GetLocation(int direction, int steps)
    {
        float xOffset = steps * MovementStep;

        Vector3 newPosition = TerrainNavigationLibrary.GetPositionAtXDisplacement(
                Bounds,
                GetTerrainDirection(direction),
                xOffset);

        return new Location(
            this,
            direction * steps,
            direction,
            newPosition,
            newPosition + (LaunchPoint.position - Center));
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