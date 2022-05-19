using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public static event Action<bool, Vector2> onBulletMoved;
    public AudioClip shotSfx;

    private bool isTouched = false;
    private Rigidbody2D rb;
    private PhotonView photonView;
    private Transform thisT;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        thisT = transform;

        SFXManager.Instance.PlayOneShot(shotSfx, 0.5f);
    }

    private void Update()
    {
        thisT.right = Vector2.Lerp(thisT.right,
                                       rb.velocity.normalized * ConfigurationManager.Instance.Config.GetFactorRotationIndicator(),
                                       Time.deltaTime);
        onBulletMoved?.Invoke(photonView.IsMine, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTouched)
            return;
        
        isTouched = true;

        var hitPose = collision.contacts[0].point;

        //If it blows very fast, on other player Start doesn't even has time to play
        if (photonView!=null && photonView.IsMine)
        {
            VFXManager.Instance.PUN_InstantiateExplosion(hitPose);
        }

        Destroy(gameObject);
    }
}
