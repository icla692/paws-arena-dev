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

    private void SetupMyPlayer()
    {
        PlayerManager.Instance.RegisterMyPlayer(this);
        state = new PlayerState();

        playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();

        playerMotionBehaviour.RegisterMovementCallbacks(playerActions);
        playerMotionBehaviour.RegisterJumpCallbacks(playerActions);
        playerMotionBehaviour.RegisterPlayerState(state);

        playerGraphicsBehaviour.RegisterPlayerState(state);

        var playerIndicatorBehaviour = GetComponentInChildren<PlayerIndicatorBehaviour>();
        playerIndicatorBehaviour.RegisterDirectionCallbacks(playerActions);

        var playerThrowBehaviour = GetComponentInChildren<PlayerThrowBehaviour>();
        playerThrowBehaviour.RegisterThrowCallbacks(playerActions);
    }

    private void SetupOtherPlayer()
    {
        Destroy(GetComponent<Rigidbody2D>());
    }

    private void OnStateUpdatedForMyPlayer(IRoomState roomState)
    {
        if(roomState is MyTurnState)
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
}
