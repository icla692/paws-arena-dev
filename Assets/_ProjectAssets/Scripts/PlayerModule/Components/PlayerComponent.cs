using Photon.Pun;
using System;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField]
    private PlayerGraphicsBehaviour playerGraphicsBehaviour;

    [HideInInspector]
    public PlayerState state;

    private GameInputActions.PlayerActions playerActions;

    private void Start()
    {
        var photonView = GetComponent<PhotonView>();
        if (photonView != null && !photonView.IsMine) return;

        state = new PlayerState();

        playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        
        var playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
        playerMotionBehaviour.RegisterMovementCallbacks(playerActions);
        playerMotionBehaviour.RegisterJumpCallbacks(playerActions);
        playerMotionBehaviour.RegisterPlayerState(state);

        playerGraphicsBehaviour.RegisterPlayerState(state);

        var playerIndicatorBehaviour = GetComponentInChildren<PlayerIndicatorBehaviour>();
        playerIndicatorBehaviour.RegisterDirectionCallbacks(playerActions);
        
        var playerThrowBehaviour = GetComponentInChildren<PlayerThrowBehaviour>();
        playerThrowBehaviour.RegisterThrowCallbacks(playerActions);
    }

    private void OnEnable()
    {
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(GameSceneStates state)
    {
        //if(state != GameSceneStates.PLAYER_1 && state != GameSceneStates.PLAYER_2)
        //{
        //    playerActions.Disable();
        //}
        //else
        //{
        //    playerActions.Enable();
        //}
    }
}
