using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public PlayerComponent playerComponent;
    public PlayerThrowBehaviour playerThrowBehaviour;

    public Transform mobileParent;

    private GameObject lastWeapon = null;

    public void Init()
    {
        int weaponIdx = playerComponent.state.weaponIdx;
        if (weaponIdx < 0)
        {
            Debug.LogWarning("No weapon selected but weapon enabled");
            return;
        }

        if(lastWeapon != null)
        {
            Destroy(lastWeapon);
        }

        var weapon = ConfigurationManager.Instance.Weapons.GetWeapon(weaponIdx);
        lastWeapon = PhotonNetwork.Instantiate("Weapons/" + weapon.launcher.name, Vector3.zero, Quaternion.identity);
        lastWeapon.transform.parent = mobileParent;
        lastWeapon.transform.localPosition = Vector3.zero;
        lastWeapon.transform.localRotation = Quaternion.identity;

        lastWeapon.GetComponent<WeaponMobileLauncherBehaviour>().throwBehaviour = playerThrowBehaviour;
    }
}
