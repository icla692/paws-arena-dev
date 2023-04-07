using System;
using UnityEngine;

public class ValuablesManager : MonoBehaviour
{
    public static ValuablesManager Instance;

    float snacks;
    float jugOfMilk;
    float glassOfMilk;

    public Action UpdatedSnacks;
    public Action UpdatedJugOfMilk;
    public Action UpdatedGlassOfMilk;

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

    public float JugOfMilk
    {
        get
        {
            return jugOfMilk;
        }
        set
        {
            jugOfMilk = value;
            UpdatedJugOfMilk?.Invoke();
        }
    }

    public float GlassOfMilk
    {
        get
        {
            return glassOfMilk;
        }
        set
        {
            glassOfMilk = value;
            UpdatedGlassOfMilk?.Invoke();
        }

    }
}
