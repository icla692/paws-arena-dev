using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealingDisplay : MonoBehaviour
{
    public GameObject damageDealPrefab;
    public BasePlayerComponent basePlayerComponent;

    private void OnEnable()
    {
        basePlayerComponent.onDamageTaken += OnDamageTaken;
    }

    private void OnDisable()
    {
        basePlayerComponent.onDamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(int damage)
    {
        var go = GameObject.Instantiate(damageDealPrefab, transform.position, Quaternion.identity, null);
        go.transform.GetChild(0).position = new Vector2(UnityEngine.Random.Range(-2.0f, 2.0f), 0);
        go.transform.GetChild(0).GetComponent<DamageDealingText>().Init(damage);
    }
}
