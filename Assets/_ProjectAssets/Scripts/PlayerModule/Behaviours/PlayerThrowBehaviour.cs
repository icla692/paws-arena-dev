using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Extensions;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowBehaviour : MonoBehaviour
{
    public static event Action<WeaponEntity> onLaunchPreparing;
    [SerializeField] private BasePlayerComponent playerComponent;

    [SerializeField] private PlayerIndicatorBehaviour indicator;
    [SerializeField] private Transform launchPoint;

    private Config config => ConfigurationManager.Instance.Config;

    private PhotonView photonView;
    private bool isMultiplayer;
    private bool isEnabled = false;

    private WeaponEntity currentWeapon;
    private List<GameObject> projectiles;

    private void OnEnable()
    {
        photonView = GetComponent<PhotonView>();
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();

        if (!isMultiplayer && photonView != null)
        {
            photonView.enabled = false;
            photonView = null;
        }

        isEnabled = true;
        if (!isMultiplayer || photonView.IsMine)
        {
            PlayerActionsBar.OnShoot += PrepareLaunch;
        }
    }

    private void OnDisable()
    {
        isEnabled = false;
        if (!isMultiplayer || photonView.IsMine)
        {
            PlayerActionsBar.OnShoot -= PrepareLaunch;
        }
    }

    public void RegisterThrowCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Approve.performed += _ => PrepareLaunch();
    }
    public void RegisterThrowCallbacks(BotInputActions.PlayerActions playerActions)
    {
        playerActions.Approve.performed += _ => PrepareLaunch();
    }

    private void PrepareLaunch()
    {
        if (!isEnabled) return;
        isEnabled = false;

        int weaponIdx = playerComponent.state.weaponIdx;
        currentWeapon = ConfigurationManager.Instance.Weapons.GetWeapon(weaponIdx);

        projectiles = new List<GameObject>();

        for (int i = 0; i < currentWeapon.numberOfProjectiles; i++) {
            GameObject obj = SingleAndMultiplayerUtils.Instantiate("Bullets/" + currentWeapon.bulletPrefab.name, launchPoint.position, Quaternion.Euler(transform.rotation.eulerAngles));
            
            projectiles.Add(obj);
            if(i != currentWeapon.numberOfProjectiles / 2)
            {
                obj.GetComponent<BulletComponent>().hasEnabledPositionTracking = false;
            }
        }

        //So that animation etc plays on all clients
        SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "OnLaunchPreparing", RpcTarget.All);
    }

    [PunRPC]
    public void OnLaunchPreparing()
    {
        onLaunchPreparing?.Invoke(currentWeapon);
    }

    public void Launch()
    {
        if (photonView != null && !photonView.IsMine) return;

        int weaponIdx = playerComponent.state.weaponIdx;
        var weapon = ConfigurationManager.Instance.Weapons.GetWeapon(weaponIdx);

        float deviation = 10;
        for (int i = 0; i < projectiles.Count; i++)
        {
            float angle = deviation * (i - projectiles.Count / 2);
            Vector3 direction = Quaternion.Euler(0, 0, angle) * launchPoint.up;

            projectiles[i].GetComponent<BulletComponent>().Launch(direction, GetBulletSpeed());
        }
        RoomStateManager.Instance.SetProjectileLaunchedState(weapon.waitBeforeTurnEnd);
    }



    private float GetBulletSpeed()
    {
        return config.GetBulletSpeed(indicator.currentPower);
    }
}