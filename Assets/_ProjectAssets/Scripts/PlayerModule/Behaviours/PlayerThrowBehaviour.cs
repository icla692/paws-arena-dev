using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Extensions;
using Photon.Pun;
using System;
using UnityEngine;

public class PlayerThrowBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerIndicatorBehaviour indicator;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject bullet;

    private Config config => ConfigurationManager.Instance.Config;

    private PhotonView photonView;
    private bool isEnabled = false;

    private void OnEnable()
    {
        photonView = GetComponent<PhotonView>();
        isEnabled = true;
        if (photonView.IsMine)
        {
            PlayerActionsBar.OnShoot += Launch;
        }
    }

    private void OnDisable()
    {
        isEnabled = false;
        if (photonView.IsMine)
        {
            PlayerActionsBar.OnShoot -= Launch;
        }
    }

    public void RegisterThrowCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Approve.performed += _ => Launch();
    }

    private void Launch()
    {
        if (!isEnabled) return;
        var obj = PhotonNetwork.Instantiate(bullet.name, launchPoint.position, Quaternion.Euler(transform.rotation.eulerAngles));
        obj.GetComponent<Rigidbody2D>().AddForce(launchPoint.up* GetBulletSpeed(), ForceMode2D.Impulse);
        RoomStateManager.Instance.SetProjectileLaunchedState();
    }

    private float GetBulletSpeed()
    {
        return config.GetBulletSpeed(indicator.currentPower);
    }
}