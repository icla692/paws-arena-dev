
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
            GameResolveState resolveState = PlayerManager.Instance.GetWinnerByHealth();
            if (resolveState != GameResolveState.NO_WIN)
            {
                context.photonView.RPC("StartResolveGame", RpcTarget.All, resolveState);
            }
            else
            {
                context.photonView.RPC("StartNextRound", RpcTarget.All);
            }
        }
    }
}