using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static BotAI;

public class BotAIAim
{
    public struct CollisionInfo
    {
        public bool hit;
        public Vector3 position;
        public bool directHit;
        public float distanceFromEnemy;        
    }

    private Config Config => ConfigurationManager.Instance.Config;

    private const float SIM_DURATION = 5f;
    private const float SIM_INTERVAL = 0.05f;

    private CapsuleCollider2D bulletCapsule;
    private float bulletMass;

    private Collider2D[] enemy;

    public BotAIAim(Collider2D[] enemy)
    {
        this.enemy = enemy;

        bulletCapsule = BotManager.Instance.bulletPrefab.GetComponent<CapsuleCollider2D>();
        bulletMass = BotManager.Instance.bulletPrefab.GetComponent<Rigidbody2D>().mass;
    }

    public void SimulateLocationAim(ref Location location)
    {
        float bestDistance = Mathf.Infinity;

        for (int angle = 0; angle < 180; angle++)
        {
            for (float power = 0.1f; power <= 1; power += 0.05f)
            {
                Debug.Log("Angle " + angle);
                Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                Debug.Log("direction " + direction);

                List<Vector3> arc = new List<Vector3>();
                CollisionInfo collision = SimulateArc(direction, location.launchPosition, GetBulletForce(power), ref arc);
                if (collision.hit)
                {
                    if (collision.distanceFromEnemy < bestDistance)
                    {
                        bestDistance = collision.distanceFromEnemy;
                        location.bestAngle = angle;
                        location.bestPower = power;
                        location.bestDistanceToEnemy = bestDistance;
                    }

                    if (collision.directHit)
                    {
                        location.directHit = true;
                        return;
                    }
                }
            }
        }
    }

    private float GetBulletForce(float power)
    {
        return Config.GetBulletSpeed(power);
    }

    public CollisionInfo SimulateArc(Vector2 directionVector, Vector2 launchPosition, float force, ref List<Vector3> arc)
    {
        float velocity = force / bulletMass;

        int maxSteps = (int)(SIM_DURATION / SIM_INTERVAL);
        for (int i = 0; i < maxSteps; ++i)
        {
            Vector3 calculatedPosition = launchPosition + directionVector * velocity * i * SIM_INTERVAL;
            calculatedPosition.y += Physics2D.gravity.y / 2 * Mathf.Pow(i * SIM_INTERVAL, 2);

            arc.Add(calculatedPosition);

            Vector3 calculatedDirection = i == 0 ? directionVector : arc[i] - arc[i - 1];
            float angle = Vector3.Angle(Vector3.right, calculatedDirection);

            CollisionInfo collision = CheckCollision(calculatedPosition, angle);
            if (collision.hit) return collision;
        }

        return new CollisionInfo() { hit = false };
    }

    private CollisionInfo CheckCollision(Vector3 position, float angle)
    {
        CollisionInfo result = new CollisionInfo
        {
            position = position,
            hit = false,
            directHit = false,
            distanceFromEnemy = GetDistanceFromEnemy(position)
        };

        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
            position,
            bulletCapsule.size,
            bulletCapsule.direction, 
            angle, 
            TerrainNavigationLibrary.LAYERMASK_HITTABLES);
        if (hits == null || hits.Length == 0) return result;

        result.hit = true;

        foreach (Collider2D e in enemy)
        {
            if (hits.Contains(e))
            {
                result.directHit = true;
                return result;
            }
        }

        return result;
    }

    private float GetDistanceFromEnemy(Vector3 position)
    {
        return enemy.Select(e => Vector3.Distance(position, e.transform.position)).OrderBy(x => x).First();
    }

    /*
    public static Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float time)
    {
        float vx = (target.x - origin.x) / time;
        float vz = (target.z - origin.z) / time;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * time * time) / time;

        return new Vector3(vx, vy, vz);
    }
    */
}
