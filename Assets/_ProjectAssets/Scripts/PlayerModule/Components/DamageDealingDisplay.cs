using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealingDisplay : MonoBehaviour
{
    public GameObject damageDealPrefab;
    public BasePlayerComponent basePlayerComponent;

    Vector3 damageOffset = new Vector3(0, 1, 0);
    int amountOfShowingDamageTexts = 0;

    private void OnEnable()
    {
        basePlayerComponent.onDamageTaken += OnDamageTaken;
        DamageDealingText.Finished += DeduceAmountOfTexts;
    }

    private void OnDisable()
    {
        basePlayerComponent.onDamageTaken -= OnDamageTaken;
        DamageDealingText.Finished -= DeduceAmountOfTexts;
    }

    void DeduceAmountOfTexts()
    {
        amountOfShowingDamageTexts--;
    }

    private void OnDamageTaken(int damage)
    {
        amountOfShowingDamageTexts++;
        var go = GameObject.Instantiate(damageDealPrefab, transform.position, Quaternion.identity, null);
        go.transform.localPosition += (amountOfShowingDamageTexts * damageOffset);
        go.transform.GetChild(0).position = new Vector2(UnityEngine.Random.Range(-2.0f, 2.0f), 0);
        go.transform.GetChild(0).GetComponent<DamageDealingText>().Init(damage);
    }
}
