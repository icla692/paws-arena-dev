using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public PlayerThrowBehaviour playerThrowBehaviour;

    public GameObject weaponWrapper;
    public SpriteRenderer weaponBody;

    public void Init(int weaponIdx)
    {
        if (weaponIdx < 0)
        {
            return;
        }


        weaponWrapper.SetActive(true);

        var weapon = ConfigurationManager.Instance.Weapons.GetWeapon(weaponIdx);
        weaponBody.sprite = weapon.launcher;
    }

    private void OnDisable()
    {
        weaponWrapper.SetActive(false);
    }

}
