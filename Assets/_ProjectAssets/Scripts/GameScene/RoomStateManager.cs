using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateManager : MonoBehaviour
{
    public static event Action<GameSceneStates> OnStateUpdated;

    public PUNGameRoomManager photonManager;
    public GameObject playerPrefab;

    [HideInInspector]
    public GameSceneStates state;

    void Start()
    {
        photonManager.OnPlayerConnectedToScene += OnPlayerConnectedToScene;
        Init();
    }

    private void Init()
    {
        SetState(GameSceneStates.WAITING_FOR_ALL_PLAYERS_TO_JOIN);

        int seat = photonManager.GetMySeat();
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(seat == 0 ? 14 : 40, 20), Quaternion.identity);

        int usersInScene = photonManager.GetUsersInScene();
        usersInScene++;
        photonManager.SetUsersInScene(usersInScene);
    }

    public void SetState(GameSceneStates state)
    {
        this.state = state;
        OnStateUpdated?.Invoke(state);
    }

    private void OnPlayerConnectedToScene(int obj)
    {
        Debug.Log($"Players in scene: {obj} / {PhotonNetwork.CurrentRoom.PlayerCount}");
        if(obj == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            SetState(GameSceneStates.STARTING_GAME);
        }
    }
}
