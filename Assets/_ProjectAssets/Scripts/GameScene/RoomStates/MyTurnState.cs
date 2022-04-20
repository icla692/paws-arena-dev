using Photon.Pun;

public class MyTurnState : IRoomState
{
    public void Init(RoomStateManager context)
    {
        var playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        playerActions.Enable();
        context.lastPlayerRound = PhotonNetwork.LocalPlayer.IsMasterClient ? 0 : 1;
    }

    public void OnExit()
    {
        var playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        playerActions.Disable();
    }
}
