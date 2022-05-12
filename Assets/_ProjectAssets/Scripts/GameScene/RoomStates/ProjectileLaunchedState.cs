
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
        yield return new WaitForSeconds(3f);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            context.TryStartNextRound();
        }
    }
}