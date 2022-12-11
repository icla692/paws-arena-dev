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
    [SerializeField]
    private List<Transform> possibleLocations;

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

    public void Relocate()
    {
        LeanTween.scale(dummyWrapper, Vector3.zero, 0.5f).setDelay(1f).setOnComplete(() =>
        {
            dummyWrapper.transform.parent = possibleLocations[UnityEngine.Random.Range(0, possibleLocations.Count)];
            dummyWrapper.transform.localPosition = Vector3.zero;

            LeanTween.scale(dummyWrapper, Vector3.one, 0.5f).setDelay(0.5f);
        });
    }
}
