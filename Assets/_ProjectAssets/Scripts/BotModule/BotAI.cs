using Anura.ConfigurationModule.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BotAI : MonoBehaviour
{
    private struct Location
    {
        public Vector3 position;
        public int direction;  // leftOfOrigin=-1, origin=0, rightOfOrigin=1
        public float score;
    }

    private const float MOVEMENT_ARRIVAL = 0.2f;
    private const int UPDATE_FRAMES = 10;

    private BotPlayerAPI api = BotPlayerAPI.Instance;

    private Bounds Bounds => Collider.bounds;
    private Vector3 Center => Bounds.center;

    private Rigidbody2D Rigidbody { get; set; }
    private Collider2D Collider { get; set; }

    // For debugging, to be deleted
    private bool debug = false;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
    }

    public void Play()
    {
        StartCoroutine(PlayTurn());
    }

    public void Wait()
    {
    }

    private IEnumerator PlayTurn()
    {
        List<Location> potentialLocations = GetPotentialLocations();
        Location choice = ChooseLocation(potentialLocations);

        if (debug)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = choice.position;
            Destroy(g.GetComponent<Collider>());
        }        

        yield return StartCoroutine(MoveTo(choice));

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(Shoot(choice));
    }

    private List<Location> GetPotentialLocations()
    {
        var result = new List<Location>
        {
            GetLocation()
        };

        float movementStep = Bounds.size.x * 0.9f;

        for (int i = 1; i <= 10; i++)
        {
            result.Add(GetLocation(-1, i * movementStep));
            result.Add(GetLocation(1, i * movementStep));
        }

        return result;
    }

    private Location ChooseLocation(List<Location> locations)
    {
        CalculateLocationScores(ref locations);
        locations = locations.OrderByDescending(x => x.score).ToList();
        return locations[0];
    }

    private IEnumerator MoveTo(Location location)
    {
        yield return null;
        yield return null;

        api.Move(location.direction);

        int counter = 0;
        
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
        api.shootAngle = Random.Range(125, 145);
        api.shootPower = Random.Range(0.75f, 1);
        api.SelectAnglePower();

        yield return null;        
        api.Shoot();
    }

    private Location GetLocation()
    {
        return new Location
        {
            direction = 0,
            position = Center,
            score = 0
        };
    }

    private Location GetLocation(int direction, float xOffset)
    {   
        return new Location
        {
            direction = direction,
            position = TerrainNavigationLibrary.GetPositionAtXDisplacement(
                Bounds, 
                GetTerrainDirection(direction),
                xOffset),
            score = 0
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
            l.score = Random.Range(0, 1f);
            locations[i] = l;
        }
    }
}