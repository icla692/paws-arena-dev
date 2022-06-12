using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeaponEntity
{
    [SerializeField]
    public GameObject launcher;
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    public int numberOfProjectiles = 1;
}

[CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Configurations/Weapons Config", order = 4)]
public class WeaponsConfig : ScriptableObject
{
    [SerializeField]
    private List<WeaponEntity> weapons;

    public WeaponEntity GetWeapon(int idx)
    {
        return weapons[idx];
    }
}
