using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittyCustomization
{
    public Dictionary<EquipmentType, Equipment> playerEquipmentConfig;
    public Dictionary<EquipmentType, Equipment> originalConfig;
}

public class KittiesCustomizations
{
    public Dictionary<string, KittyCustomization> customizationByCatUrl;

    public KittiesCustomizations()
    {
        customizationByCatUrl = new Dictionary<string, KittyCustomization>();
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
        var customizations = config.customizationByCatUrl;
        if (customizations.ContainsKey(url))
        {
            return customizations[url];
        }

        return null;
    }

    public static void SaveCustomConfig(string url, Dictionary<EquipmentType, Equipment> playerEquipmentConfig)
    {

        if (config.customizationByCatUrl.ContainsKey(url))
        {
            config.customizationByCatUrl[url].playerEquipmentConfig = playerEquipmentConfig;
        }
        else
        {
            KittyCustomization customization = new KittyCustomization()
            {
                playerEquipmentConfig = playerEquipmentConfig
            };

            config.customizationByCatUrl.Add(url, customization);
        }
    }

    public static KittyCustomization SaveOriginalConfig(string url, Dictionary<EquipmentType, Equipment> playerEquipmentConfig)
    {
        if (config.customizationByCatUrl.ContainsKey(url))
        {
            config.customizationByCatUrl[url].originalConfig = playerEquipmentConfig;
        }
        else
        {
            KittyCustomization customization = new KittyCustomization()
            {
                originalConfig = playerEquipmentConfig
            };

            config.customizationByCatUrl.Add(url, customization);
        }
        return config.customizationByCatUrl[url];
    }

    public static void RemoveCustomizations(string url)
    {
        if (config.customizationByCatUrl.ContainsKey(url))
        {
            config.customizationByCatUrl[url].playerEquipmentConfig = null;
        }
    }
}