using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerComponent : MonoBehaviour
{
    [HideInInspector]
    public PlayerState state;
    [HideInInspector]
    public int playerSeat = 0;
    [SerializeField]
    private GameObject weaponWrapper;

    private PlayerMotionBehaviour playerMotionBehaviour;
    private bool isMultiplayer;
    private PhotonView photonView;

    private void Awake()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();
        photonView = GetComponent<PhotonView>();
        playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
    }

    private void OnDestroy()
    {
        if (state != null)
        {
            state.onWeaponIdxChanged -= OnWeaponOutChanged;
            PlayerActionsBar.WeaponIndexUpdated -= ChangeWeaponState;
        }
    }

    public void PreSetup()
    {
        state = new PlayerState();
        state.onWeaponIdxChanged += OnWeaponOutChanged;
    }
    public void PostSetup()
    {
        PlayerActionsBar.WeaponIndexUpdated += ChangeWeaponState;
        ChangeWeaponState(-1);
    }

    private void ChangeWeaponState(int idx)
    {
        if (state.weaponIdx == idx)
        {
            idx = -1;
        }
        if (playerSeat == RoomStateManager.Instance.lastPlayerRound)
        {
            state.SetHasWeaponOut(idx);
        }
    }

    private void OnWeaponOutChanged(int val)
    {
        playerMotionBehaviour.SetIsPaused(val >= 0);

        SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "NetworkedChangeWeaponState", RpcTarget.All, val);
    }

    public bool IsMine()
    {
        if (isMultiplayer)
        {
            return photonView.IsMine;
        }
        else
        {
            return true;
        }
    }

    [PunRPC]
    public void NetworkedChangeWeaponState(int val)
    {
        weaponWrapper.SetActive(val >= 0);
        weaponWrapper.GetComponent<WeaponBehaviour>().Init(val);
    }
}
