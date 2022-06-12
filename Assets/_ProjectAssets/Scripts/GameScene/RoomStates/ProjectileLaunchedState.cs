
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ProjectileLaunchedState : IRoomState
{
    public void Init(RoomStateManager context)
    {
        context.StartCoroutine(HandleProjectileLaunched(context));
    }

    public void OnExit()
    {
    }
    private IEnumerator HandleProjectileLaunched(RoomStateManager context)
    {
        if (context.WasMyRound())
        {
            context.trajectory.StartRecording();
        }
        yield return new WaitForSeconds(4f);


        if (context.WasMyRound())
        {
            context.trajectory.StopRecording();
        }
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            context.TryStartNextRound();
        }
    }
}