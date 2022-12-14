using Anura.ConfigurationModule.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
    public event Action onDummyHit;
    public event Action onDummyMiss;
    public event Action onDummyDead;
    [SerializeField]
    private GameObject dummyWrapper;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private List<Transform> possibleLocations;

    private PlayerDataCustomView npcHPBar;
    private int myHP;

    private void Start()
    {
        npcHPBar = PlayerDataCustomView.npcBar;

        Init();
        dummyWrapper.SetActive(false);
    }

    private void Init()
    {
        myHP = ConfigurationManager.Instance.Dummy.dummyHP;
        npcHPBar.SetHealth(myHP);
    }
    public void EnableDummy()
    {
        dummyWrapper.SetActive(true);

        npcHPBar.SetNickname("Dummy");
        myHP = ConfigurationManager.Instance.Dummy.dummyHP;
        npcHPBar.SetHealth(myHP);
        AreaEffectsManager.Instance.OnAreaDamage += OnAreaDamage;
    }

    private void OnAreaDamage(Vector2 position, float area, int maxDamage, bool damageByDistance, bool hasPushForce, float pushForce)
    {
        Vector3 dummyPos = dummyWrapper.transform.position;
        float dmgDistance = Vector3.Distance(dummyPos, position);
        if (dmgDistance > area)
        {
            onDummyMiss?.Invoke();
            return;
        };

        float damagePercentage = (area - dmgDistance) / area;
        int dmgToBeDone = damageByDistance ? (int)Math.Floor(damagePercentage * maxDamage) : maxDamage;

        myHP -= dmgToBeDone;
        myHP = Math.Max(0, myHP);

        npcHPBar.SetHealth(myHP);

        animator.SetTrigger("IsHit");

        onDummyHit?.Invoke();
        if(myHP == 0)
        {
            onDummyDead?.Invoke();
        }
    }

    public void Relocate()
    {
        LeanTween.scale(dummyWrapper, Vector3.zero, 0.5f).setDelay(1f).setOnComplete(() =>
        {
            Init();
            dummyWrapper.transform.parent = possibleLocations[UnityEngine.Random.Range(0, possibleLocations.Count)];
            dummyWrapper.transform.localPosition = Vector3.zero;

            LeanTween.scale(dummyWrapper, Vector3.one, 0.5f).setDelay(0.5f);
        });
    }
}
