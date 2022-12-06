
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

        InstantiatePlayer(context);

        yield return new WaitForSeconds(3f);

        //If tutorial don't auto-start game
        if (ConfigurationManager.Instance.Config.GetGameType() == Anura.ConfigurationModule.ScriptableObjects.GameType.TUTORIAL)
        {
            context.SetState(new GamePausedState());
        }
        else
        {
            context.SetFirstPlayerTurn();
        }
    }

    private void InstantiatePlayer(RoomStateManager context)
    {
        int seat = 0;
        if (ConfigurationManager.Instance.Config.GetIsMultiplayer())
        {
            seat = context.photonManager.GetMySeat();
        }
        Vector2 spawnPos = seat == 0 ? PlayerManager.Instance.GetPlayer1SpawnPos() : PlayerManager.Instance.GetPlayer2SpawnPos();

        SingleAndMultiplayerUtils.Instantiate(context.playerPrefab.name, spawnPos, Quaternion.identity);
        SingleAndMultiplayerUtils.Instantiate(context.playerUIPrefab.name, Vector3.zero, Quaternion.identity);
    }
}