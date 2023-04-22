using System;
using UnityEngine;

public class ValuablesManager : MonoBehaviour
{
    public static ValuablesManager Instance;

    float snacks;
    float jugOfMilk;
    float glassOfMilk;
    float limeCrystal;
    float greenCrystal;
    float blueCrystal;
    float purpleCrystal;
    float orangeCrystal;
    float giftItem;

    public Action UpdatedSnacks;
    public Action UpdatedJugOfMilk;
    public Action UpdatedGlassOfMilk;
    public Action UpdatedLimeCrystal;
    public Action UpdatedGreenCrystal;
    public Action UpdatedBlueCrystal;
    public Action UpdatedPurpleCrystal;
    public Action UpdatedOrangeCrystal;
    public Action UpdatedGiftItem;

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

    public float LimeCrystal
    {
        get
        {
            return limeCrystal;
        }
        set
        {
            limeCrystal = value;
            UpdatedLimeCrystal?.Invoke();
        }
    }

    public float GreenCrystal
    {
        get
        {
            return greenCrystal;
        }
        set
        {
            greenCrystal = value;
            UpdatedGreenCrystal?.Invoke();
        }
    }

    public float BlueCrystal
    {
        get
        {
            return blueCrystal;
        }
        set
        {
            blueCrystal = value;
            UpdatedBlueCrystal?.Invoke();
        }
    }

    public float PurpleCrystal
    {
        get
        {
            return purpleCrystal;
        }

        set
        {
            purpleCrystal = value;
            UpdatedPurpleCrystal?.Invoke();
        }
    }

    public float OrangeCrystal
    {
        get
        {
            return orangeCrystal;
        }
        set
        {
            orangeCrystal = value;
            UpdatedOrangeCrystal?.Invoke();
        }
    }

    public float GiftItem
    {
        get
        {
            return giftItem;
        }
        set
        {
            giftItem = value;
            UpdatedGiftItem?.Invoke();
        }
    }
}
