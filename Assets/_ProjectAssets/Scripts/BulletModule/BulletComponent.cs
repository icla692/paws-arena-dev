using Anura.ConfigurationModule.Managers;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    private bool isTouched = false;
    private Rigidbody2D rb;
    private Transform thisT;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        thisT = transform;
    }

    private void Update()
    {
        thisT.right = Vector2.Lerp(thisT.right,
                                       rb.velocity.normalized * ConfigurationManager.Instance.Config.GetFactorRotationIndicator(),
                                       Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTouched)
            return;
        
        isTouched = true;
        var hitPose = collision.contacts[0].point;

        PaintingManager.Instance.RandomShape();
        PaintingManager.Instance.Destroy(hitPose);
        VFXManager.Instance.InstantiateExplosion(hitPose);

        Destroy(gameObject);
    }
}
