using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    private bool isTouched = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTouched)
            return;
        
        isTouched = true;
        var hitPose = collision.contacts[0].point;

        PaintingManager.Instance.Destroy(hitPose);
        VFXManager.Instance.InstantiateExplosion(hitPose);

        Destroy(gameObject);
    }
}
