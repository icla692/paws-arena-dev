using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
    public event Action onDummyHit;
    [SerializeField]
    private GameObject dummyWrapper;
    [SerializeField]
    private Animator animator;

    private void Start()
    {
        dummyWrapper.SetActive(false);
    }
    public void EnableDummy()
    {
        dummyWrapper.SetActive(true);
        AreaEffectsManager.Instance.OnAreaDamage += OnAreaDamage;
    }

    private void OnAreaDamage(Vector2 position, float area, int maxDamage, bool damageByDistance, bool hasPushForce, float pushForce)
    {
        Vector3 dummyPos = dummyWrapper.transform.position;
        float dmgDistance = Vector3.Distance(dummyPos, position);
        if (dmgDistance > area) return;

        animator.SetTrigger("IsHit");

        onDummyHit?.Invoke();
    }
}
