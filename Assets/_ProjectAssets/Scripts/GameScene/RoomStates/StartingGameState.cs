
using Anura.ConfigurationModule.Managers;
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
        if (ConfigurationManager.Instance.Config.GetIsMultiplayer())
        {
            InstantiatePlayer_MP(context);
        }
        else
        {
            InstantiatePlayer_SP(context);
        }

        yield return new WaitForSeconds(3f);
        context.SetFirstPlayerTurn();
    }

    private void InstantiatePlayer_MP(RoomStateManager context)
    {
        int seat = context.photonManager.GetMySeat();
        Vector2 spawnPos = seat == 0 ? PlayerManager.Instance.GetPlayer1SpawnPos() : PlayerManager.Instance.GetPlayer2SpawnPos();
        PhotonNetwork.Instantiate(context.playerPrefab.name, spawnPos, Quaternion.identity);
        PhotonNetwork.Instantiate(context.playerUIPrefab.name, Vector3.zero, Quaternion.identity);
    }

    private void InstantiatePlayer_SP(RoomStateManager context)
    {
        int seat = 0;
        Vector2 spawnPos = seat == 0 ? PlayerManager.Instance.GetPlayer1SpawnPos() : PlayerManager.Instance.GetPlayer2SpawnPos();
        GameObject playerPrefab = Resources.Load(context.playerPrefab.name) as GameObject;
        GameObject.Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        
        GameObject playerUIPrefab = Resources.Load(context.playerUIPrefab.name) as GameObject;
        GameObject.Instantiate(playerUIPrefab, Vector3.zero, Quaternion.identity);
    }
}