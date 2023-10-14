using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponSkin", menuName = "ScriptableObjects/WeaponSkin")]

public class WeaponSkinSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public WeaponSkinType Type { get; private set; }
    [field: SerializeField] public Sprite[] Sprites { get; private set; }
    [field: SerializeField] public Sprite[] ProjectileSprite { get; private set; }
    [field: SerializeField] public Sprite Preview { get; private set; }
    private static List<WeaponSkinSO> allRewards;
    
    public static WeaponSkinSO Get(int _id)
    {
        LoadAllRewards();
        return allRewards.First(element => element.Id == _id);
    }
    
    private static void LoadAllRewards()
    {
        if (allRewards != null)
        {
            return;
        }

        allRewards = Resources.LoadAll<WeaponSkinSO>("WeaponSkin/").ToList();
    }
}