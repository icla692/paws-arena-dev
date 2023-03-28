using System;
using UnityEngine;

public class ValuablesManager : MonoBehaviour
{
    public static ValuablesManager Instance;

    float snacks;
    float milk;

    public Action UpdatedSnacks;
    public Action UpdatedMilk;


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

    public float Snacks
    {
        get
        {
            return snacks;
        }
        set
        {
            snacks = value;
            UpdatedSnacks?.Invoke();
        }
    }

    public float Milk
    {
        get
        {
            return milk;
        }
        set
        {
            milk = value;
            UpdatedMilk?.Invoke();
        }
    }
}
