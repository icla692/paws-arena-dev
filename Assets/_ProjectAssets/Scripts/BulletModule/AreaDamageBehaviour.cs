using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip explosionSfx;
    [SerializeField]
    private float area = 5.0f;
    [SerializeField]
    private int maxDamage = 20;
    [SerializeField]
    private bool doesDamageByDistance = false;
    [SerializeField]
    private bool hasPushForce = false;
    [SerializeField]
    private float pushForce = 10f;

    private void OnEnable()
    {
        PaintingManager.Instance.GetShape(0);
        PaintingManager.Instance.Destroy(transform.position);

        GameScenePostprocessingManager.Instance.EnableExplosionLayer(0.4f);
        PlayerManager.Instance.AreaDamage(transform.position, area, maxDamage, doesDamageByDistance, hasPushForce, pushForce);
        SFXManager.Instance.PlayOneShot(explosionSfx);
    }
}
