using Photon.Pun;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField]
    private PlayerGraphicsBehaviour playerGraphicsBehaviour;

    [HideInInspector]
    public PlayerState state;
    private void Start()
    {
        var photonView = GetComponent<PhotonView>();
        if (photonView != null && !photonView.IsMine) return;

        state = new PlayerState();

        var playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        
        var playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
        playerMotionBehaviour.RegisterMovementCallbacks(playerActions);
        playerMotionBehaviour.RegisterJumpCallbacks(playerActions);
        playerMotionBehaviour.RegisterPlayerState(state);

        playerGraphicsBehaviour.RegisterPlayerState(state);

        var playerIndicatorBehaviour = GetComponent<PlayerIndicatorBehaviour>();
        playerIndicatorBehaviour.RegisterDirectionCallbacks(playerActions);
        
        var playerThrowBehaviour = GetComponent<PlayerThrowBehaviour>();
        playerThrowBehaviour.RegisterThrowCallbacks(playerActions);
    }

}
