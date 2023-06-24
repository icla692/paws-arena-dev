using System;
using Newtonsoft.Json;
using UnityEngine;

[SerializeField]
public class CrystalsData
{
    float commonCrystal = 0;
    float uncommonCrystal = 0;
    float rareCrystal = 0;
    float epicCrystal = 0;
    float legendaryCristal = 0;
    
    [JsonIgnore] public Action UpdatedCommonCrystal;
    [JsonIgnore] public Action UpdatedUncommonCrystal;
    [JsonIgnore] public Action UpdatedRareCrystal;
    [JsonIgnore] public Action UpdatedEpicCrystal;
    [JsonIgnore] public Action UpdatedLegendaryCrystal;
    
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
    
    [JsonIgnore]
    public float TotalCrystalsAmount => commonCrystal + uncommonCrystal + rareCrystal + epicCrystal + legendaryCristal;

}
