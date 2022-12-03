using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public static event Action<bool, Vector2> onBulletMoved;
    public AudioClip shotSfx;
    public GameObject explosionPrefab;

    [HideInInspector]
    public bool hasEnabledPositionTracking = true;

    private bool isTouched = false;
    protected Rigidbody2D rb;
    protected PhotonView photonView;
    protected bool isMultiplayer;

    private Transform thisT;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        thisT = transform;

        rb.isKinematic = true;

        HandleStart();

        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();
        if (!isMultiplayer)
        {
            photonView.enabled = false;
            GetComponent<PhotonRigidbody2DView>().enabled = false;
            photonView = null;
        }
    }

    protected virtual void HandleStart()
    {

    }
    public void Launch()
    {
        rb.isKinematic = false;
    }
    public void Launch(Vector3 direction, float speed)
    {
        if (shotSfx)
        {
            SFXManager.Instance.PlayOneShot(shotSfx, 0.5f);
        }
        rb.isKinematic = false;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        thisT.right = Vector2.Lerp(thisT.right,
                                       rb.velocity.normalized * ConfigurationManager.Instance.Config.GetFactorRotationIndicator(),
                                       Time.deltaTime);

        if (hasEnabledPositionTracking)
        {
            bool isMine = !isMultiplayer || photonView.IsMine;
            onBulletMoved?.Invoke(isMine, transform.position);
        }
    }

    protected virtual void HandleCollision(Vector2 hitPose)
    {
        //If it blows very fast, on other player Start doesn't even has time to play
        if (photonView == null || photonView.IsMine)
        {
            VFXManager.Instance.PUN_InstantiateExplosion(hitPose, explosionPrefab);
        }

        SingleAndMultiplayerUtils.Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTouched)
            return;
        
        isTouched = true;

        var hitPose = collision.contacts[0].point;

        HandleCollision(hitPose);
    }
}
