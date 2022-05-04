
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class StartingGameState : IRoomState
{
    public void Init(RoomStateManager context)
    {
        context.StartCoroutine(HandleStartingSceneCoroutine(context));
    }

    public void OnExit()
    {
    }

    private IEnumerator HandleStartingSceneCoroutine(RoomStateManager context)
    {
        yield return new WaitForSeconds(1.5f);
        int seat = context.photonManager.GetMySeat();
        PhotonNetwork.Instantiate(context.playerPrefab.name, new Vector3(seat == 0 ? 14 : 40, 20), Quaternion.identity);
        PhotonNetwork.Instantiate(context.playerUIPrefab.name, Vector3.zero, Quaternion.identity);

        yield return new WaitForSeconds(3f);
        context.SetFirstPlayerTurn();
    }
}