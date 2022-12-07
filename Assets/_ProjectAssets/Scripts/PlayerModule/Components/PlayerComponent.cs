using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField]
    private PlayerGraphicsBehaviour playerGraphicsBehaviour;
    [SerializeField]
    private GameObject weaponWrapper;

    [HideInInspector]
    public PlayerState state;

    private GameInputActions.PlayerActions playerActions;
    private PlayerMotionBehaviour playerMotionBehaviour;
    private bool isMultiplayer;
    private PhotonView photonView;


    private void Awake()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();

        photonView = GetComponent<PhotonView>();
        playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
    }
    private void Start()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();
        photonView = GetComponent<PhotonView>();

        if (!isMultiplayer)
        {
            photonView.enabled = false;
            GetComponent<PhotonTransformView>().enabled = false;
            photonView = null;
        }

        if (photonView != null && !photonView.IsMine) {
            SetupOtherPlayer();
            return;
        }

        SetupMyPlayer();
    }

    private void OnEnable()
    {
        if (!isMultiplayer || (photonView != null && photonView.IsMine))
        {
            RoomStateManager.OnStateUpdated += OnStateUpdatedForMyPlayer;
        }
        else
        {
            RoomStateManager.OnStateUpdated += OnStateUpdatedForOtherPlayer;
        }

    }

    private void OnDisable()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView != null && photonView.IsMine)
        {
            RoomStateManager.OnStateUpdated -= OnStateUpdatedForMyPlayer;
        }
        else
        {
            RoomStateManager.OnStateUpdated -= OnStateUpdatedForOtherPlayer;
        }

    }

    private void OnDestroy()
    {
        if (state != null)
        {
            state.onWeaponIdxChanged -= OnWeaponOutChanged;
            PlayerActionsBar.WeaponIndexUpdated -= ChangeWeaponState;
        }
    }

    private void SetupMyPlayer()
    {
        PlayerManager.Instance.RegisterMyPlayer(this);
        state = new PlayerState();
        state.onWeaponIdxChanged += OnWeaponOutChanged;

        playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();

        playerMotionBehaviour.RegisterMovementCallbacks(playerActions);
        playerMotionBehaviour.RegisterJumpCallbacks(playerActions);
        playerMotionBehaviour.RegisterPlayerState(state);

        playerGraphicsBehaviour.RegisterPlayerState(state);

        var playerIndicatorBehaviour = GetComponentInChildren<PlayerIndicatorBehaviour>();
        playerIndicatorBehaviour.RegisterDirectionCallbacks(playerActions);

        var playerThrowBehaviour = GetComponentInChildren<PlayerThrowBehaviour>();
        playerThrowBehaviour.RegisterThrowCallbacks(playerActions);

        PlayerActionsBar.WeaponIndexUpdated += ChangeWeaponState;

        ChangeWeaponState(-1);
        playerActions.Disable();
    }

    private void SetupOtherPlayer()
    {
        Destroy(GetComponent<Rigidbody2D>());
    }

    private void OnStateUpdatedForMyPlayer(IRoomState roomState)
    {
        state.SetHasWeaponOut(-1);
        if (roomState is MyTurnState)
        {
           playerActions.Enable(); 
        }
        else
        {
            playerActions.Disable();
        }
    }

    private void OnStateUpdatedForOtherPlayer(IRoomState roomState)
    {
        //weaponWrapper.SetActive(roomState is OtherPlayersShootingState);
    }

    //Set State
    private void ChangeWeaponState(int idx)
    {
        if(state.weaponIdx == idx)
        {
            idx = -1;
        }

        state.SetHasWeaponOut(idx);
    }


    //Listen to state and propagate to all clients
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

    //Actual logic
    [PunRPC]
    public void NetworkedChangeWeaponState(int val)
    {
        weaponWrapper.SetActive(val >= 0);
        weaponWrapper.GetComponent<WeaponBehaviour>().Init(val);
    }
}
