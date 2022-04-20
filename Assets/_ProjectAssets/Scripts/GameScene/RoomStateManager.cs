using Anura.Templates.MonoSingleton;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateManager : MonoSingleton<RoomStateManager>
{
    public static event Action<GameSceneStates> OnStateUpdated;

    public PUNGameRoomManager photonManager;
    public GameObject playerPrefab;

    [HideInInspector]
    public GameSceneMasterInfo sceneInfo = new GameSceneMasterInfo();

    private int lastPlayerRound = -1;
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Init();
    }

    private void Init()
    {
        SetState(GameSceneStates.WAITING_FOR_ALL_PLAYERS_TO_JOIN);
        photonView.RPC("OnPlayerSceneLoaded", RpcTarget.All);
    }

    public void SetState(GameSceneStates state)
    {
        //Exit old states
        int wasMyRound = IsMyRound(this.sceneInfo.state);
        if (wasMyRound > 0)
        {
            HandleMyRoundOver();
        }

        // Handle States
        int isMyRound = IsMyRound(state);
        if (state == GameSceneStates.STARTING_GAME)
        {
            StartCoroutine(HandleStartingSceneCoroutine());
        }else if(state == GameSceneStates.PLAYER_1)
        {
            lastPlayerRound = 0;
        }else if(state == GameSceneStates.PLAYER_2)
        {
            lastPlayerRound = 1;
        }
        else if (state == GameSceneStates.PROJECTILE_LAUNCHED)
        {
            StartCoroutine(HandleProjectileLaunched());
        }

        if (isMyRound > 0)
        {
            HandleMyRound();
        }

        this.sceneInfo.state = state;

        OnStateUpdated?.Invoke(state);
    }

    /// <summary>
    /// <para>1 = My Round</para>
    /// <para>-1 = Opponent Round</para>
    /// <para>0 = No player's round</para>
    /// </summary>
    /// <param name="state">State to check</param>
    /// <returns></returns>
    private int IsMyRound(GameSceneStates state)
    {
        int mySeat = photonManager.GetMySeat();
        if ((state == GameSceneStates.PLAYER_1 && mySeat == 0) || (state == GameSceneStates.PLAYER_2 && mySeat == 1))
        {
            return 1;
        }else if((state == GameSceneStates.PLAYER_2 && mySeat == 0) || (state == GameSceneStates.PLAYER_1 && mySeat == 1))
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    private IEnumerator HandleStartingSceneCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        int seat = photonManager.GetMySeat();
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(seat == 0 ? 14 : 40, 20), Quaternion.identity);

        yield return new WaitForSeconds(3f);
        SetState(GameSceneStates.PLAYER_1);
    }

    private void HandleMyRound()
    {
        var playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        playerActions.Enable();
    }

    private void HandleMyRoundOver()
    {
        var playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        playerActions.Disable();
    }

    private IEnumerator HandleProjectileLaunched()
    {
        yield return new WaitForSeconds(3f);

        photonView.RPC("StartNextRound", RpcTarget.All);
    }

    [PunRPC]
    private void StartNextRound()
    {
        if (lastPlayerRound == 0)
        {
            SetState(GameSceneStates.PLAYER_2);
        }
        else
        {
            SetState(GameSceneStates.PLAYER_1);
        }
    }

    [PunRPC]
    private void OnPlayerSceneLoaded()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            sceneInfo.usersInScene++;

            Debug.Log($"Players in scene: {sceneInfo.usersInScene} / {PhotonNetwork.CurrentRoom.PlayerCount}");
            if (sceneInfo.usersInScene == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC("OnAllPlayersJoinedScene", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OnAllPlayersJoinedScene()
    {
        SetState(GameSceneStates.STARTING_GAME);
    }
}
