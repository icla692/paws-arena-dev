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

    [SerializeField] private float delayExplosion;

    [HideInInspector]
    public bool hasEnabledPositionTracking = true;

    private bool isTouched = false;
    protected Rigidbody2D rb;
    protected PhotonView photonView;
    private Transform thisT;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        thisT = transform;

        rb.isKinematic = true;
    }

    public void Launch(Vector3 direction, float speed)
    {
        SFXManager.Instance.PlayOneShot(shotSfx, 0.5f);
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
            onBulletMoved?.Invoke(photonView.IsMine, transform.position);
        }
    }

    private IEnumerator DelayExplosion(float seconds, Collision2D collision)
    {
        yield return new WaitForSeconds(seconds);
        Explosion(collision);
    }

    private void Explosion(Collision2D collision)
    {
        var hitPose = collision.contacts[0].point;

        HandleCollision(hitPose);
    }

    protected virtual void HandleCollision(Vector2 hitPose)
    {
        //If it blows very fast, on other player Start doesn't even has time to play
        if (photonView != null && photonView.IsMine)
        {
            VFXManager.Instance.PUN_InstantiateExplosion(hitPose, explosionPrefab);
        }

        PhotonNetwork.Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTouched)
            return;
        
        isTouched = true;

        if (delayExplosion != 0)
            StartCoroutine(DelayExplosion(delayExplosion, collision));
        else
            Explosion(collision);
    }
}
