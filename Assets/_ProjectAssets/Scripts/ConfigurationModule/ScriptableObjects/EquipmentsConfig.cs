using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EquipmentData
{
    public string Id;
    public Sprite Thumbnail;
    public EquipmentRarity Rarity;
}

public enum EquipmentRarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4
}

[CreateAssetMenu(fileName = "EquipmentsConfig", menuName = "Configurations/EquipmentsConfig", order = 4)]
public class EquipmentsConfig : ScriptableObject
{
    public List<EquipmentData> Eyes;
    public List<EquipmentData> Head;
    public List<EquipmentData> Mouth;
    public List<EquipmentData> Body;

    public List<EquipmentData> TailsOverlay;
    public List<EquipmentData> TailsFloating;
    public List<EquipmentData> TailsAnimated;
}
