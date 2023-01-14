using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittyCustomization
{
    public Dictionary<EquipmentType, Equipment> playerEquipmentConfig;
}

public class KittiesCustomizations
{
    public Dictionary<string, KittyCustomization> customizations;

    public KittiesCustomizations()
    {
        customizations = new Dictionary<string, KittyCustomization>();
    }
}

public class KittiesCustomizationService
{
    public static KittiesCustomizations config;

    static KittiesCustomizationService(){
        config = new KittiesCustomizations();
    }

    public static KittyCustomization GetCustomization(string url)
    {
        var customizations = config.customizations;
        if (customizations.ContainsKey(url))
        {
            return customizations[url];
        }

        return null;
    }

    public static void Save(string url, Dictionary<EquipmentType, Equipment> playerEquipmentConfig)
    {
        KittyCustomization customization = new KittyCustomization()
        {
            playerEquipmentConfig = playerEquipmentConfig
        };

        if (config.customizations.ContainsKey(url))
        {
            config.customizations[url] = customization;
        }
        else
        {
            config.customizations.Add(url, customization);
        }
    }
}