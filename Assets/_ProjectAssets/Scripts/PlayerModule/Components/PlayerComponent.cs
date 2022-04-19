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
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView != null && !photonView.IsMine) return;

        state = new PlayerState();

        playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        
        var playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
        playerMotionBehaviour.RegisterMovementCallbacks(playerActions);
        playerMotionBehaviour.RegisterJumpCallbacks(playerActions);
        playerMotionBehaviour.RegisterPlayerState(state);

        playerGraphicsBehaviour.RegisterPlayerState(state);

        var playerIndicatorBehaviour = GetComponent<PlayerIndicatorBehaviour>();
        playerIndicatorBehaviour.RegisterDirectionCallbacks(playerActions);
        
        var playerThrowBehaviour = GetComponent<PlayerThrowBehaviour>();
        playerThrowBehaviour.RegisterThrowCallbacks(playerActions);

        playerActions.Disable();
    }
}
