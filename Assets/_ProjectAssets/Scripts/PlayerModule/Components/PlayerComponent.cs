using Photon.Pun;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    private void Start()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        var playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        
        var playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
        playerMotionBehaviour.RegisterMovementCallbacks(playerActions);
        playerMotionBehaviour.RegisterJumpCallbacks(playerActions);
        
        var playerIndicatorBehaviour = GetComponent<PlayerIndicatorBehaviour>();
        playerIndicatorBehaviour.RegisterDirectionCallbacks(playerActions);
        
        var playerThrowBehaviour = GetComponent<PlayerThrowBehaviour>();
        playerThrowBehaviour.RegisterThrowCallbacks(playerActions);
    }

}
