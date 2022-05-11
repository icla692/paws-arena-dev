using Photon.Pun;

public class MyTurnState : IRoomState
{
    public void Init(RoomStateManager context)
    {
        context.lastPlayerRound = PhotonNetwork.LocalPlayer.IsMasterClient ? 0 : 1;
    }

    public void OnExit()
    {
    }
}
