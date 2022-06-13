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
    private PhotonView photonView;


    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
    }
    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView != null && !photonView.IsMine) {
            SetupOtherPlayer();
            return;
        }

        SetupMyPlayer();
    }

    private void OnEnable()
    {
        if (photonView != null && photonView.IsMine)
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
            Debug.LogWarning("Check if the chat pannel is open");
            if (!ChatManager.Instance.GetChatPanelStatus())
            {
                playerActions.Enable(); 
            }
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
        photonView.RPC("NetworkedChangeWeaponState", RpcTarget.All, val);
    }

    public bool IsMine()
    {
        return photonView.IsMine;
    }

    //Actual logic
    [PunRPC]
    private void NetworkedChangeWeaponState(int val)
    {
        weaponWrapper.SetActive(val >= 0);
        weaponWrapper.GetComponent<WeaponBehaviour>().Init();
    }
}
