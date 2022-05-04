using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    private bool isTouched = false;
    private Rigidbody2D rb;
    private PhotonView photonView;
    private Transform thisT;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
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

        if (photonView.IsMine)
        {
            VFXManager.Instance.PUN_InstantiateExplosion(hitPose);
        }

        Destroy(gameObject);
    }
}
