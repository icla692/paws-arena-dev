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
        var go = GameObject.Instantiate(damageDealPrefab, Vector3.zero, Quaternion.identity, transform);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(UnityEngine.Random.Range(-2.0f, 2.0f), 0);
        go.GetComponent<DamageDealingText>().Init(damage);
    }
}
