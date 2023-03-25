using System;
using UnityEngine;

public class ValuablesManager : MonoBehaviour
{
    public static ValuablesManager Instance;

    float snacks;

    public Action UpdatedSnacks;

    public float Snacks => snacks;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSnacks(float _value)
    {
        snacks = _value;
        UpdatedSnacks?.Invoke();
    }
}
