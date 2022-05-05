using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageBehaviour : MonoBehaviour
{
    public float area = 5.0f;
    public int maxDamage = 20;
    public bool doesDamageByDistance = false;
    public bool hasPushForce = false;
    public float pushForce = 10f;

    private void OnEnable()
    {
        PaintingManager.Instance.RandomShape();
        PaintingManager.Instance.Destroy(transform.position);

        PlayerManager.Instance.AreaDamage(transform.position, area, maxDamage, doesDamageByDistance, hasPushForce, pushForce);
    }
}
