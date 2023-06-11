using System;
using System.Collections.Generic;
using UnityEngine;

public class ValuablesManager : MonoBehaviour
{
    public static ValuablesManager Instance;

    float snacks;
    float jugOfMilk;
    float glassOfMilk;
    float commonCrystal = 100;
    float uncommonCrystal = 3;
    float rareCrystal = 4;
    float epicCrystal = 4;
    float legendaryCristal = 4;
    float giftItem = 2;
    CraftingProcess craftingProcess;
    SeasonData seasonData;


    public Action UpdatedSnacks;
    public Action UpdatedJugOfMilk;
    public Action UpdatedGlassOfMilk;
    public Action UpdatedCommonCrystal;
    public Action UpdatedUncommonCrystal;
    public Action UpdatedRareCrystal;
    public Action UpdatedEpicCrystal;
    public Action UpdatedLegendaryCrystal;
    public Action UpdatedGiftItem;
    public Action UpdatedCraftingProcess;

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

    private void Start()
    {
        seasonData = new SeasonData();
        seasonData.Experience = 3900;
        seasonData.HasPass = true;
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

    public float CommonCrystal
    {
        get
        {
            return commonCrystal;
        }
        set
        {
            commonCrystal = value;
            UpdatedCommonCrystal?.Invoke();
        }
    }

    public float UncommonCrystal
    {
        get
        {
            return uncommonCrystal;
        }
        set
        {
            uncommonCrystal = value;
            UpdatedUncommonCrystal?.Invoke();
        }
    }

    public float RareCrystal
    {
        get
        {
            return rareCrystal;
        }
        set
        {
            rareCrystal = value;
            UpdatedRareCrystal?.Invoke();
        }
    }

    public float EpicCrystal
    {
        get
        {
            return epicCrystal;
        }

        set
        {
            epicCrystal = value;
            UpdatedEpicCrystal?.Invoke();
        }
    }

    public float LegendaryCrystal
    {
        get
        {
            return legendaryCristal;
        }
        set
        {
            legendaryCristal = value;
            UpdatedLegendaryCrystal?.Invoke();
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

    public CraftingProcess CraftingProcess
    {
        get
        {
            return craftingProcess;
        }
        set
        {
            craftingProcess = value;
            UpdatedCraftingProcess?.Invoke();
        }
    }

    public SeasonData SeasonData
    {
        get
        {
            return seasonData;
        }
        set
        {
            seasonData = value;
        }
    }
}
