using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public event Action OnStartedConnection;
    public event Action OnConnectedServer;
    public event Action OnCreatingRoom;
    public event Action OnRoomLeft;

    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    private string gameVersion = "1";

    private bool isRoomCreated = false;
    private bool isSinglePlayer = false;


    #region ACTIONS
    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        OnStartedConnection?.Invoke();
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        else
        {
            OnConnectedServer?.Invoke();
        }
    }

    public void ConnectToRandomRoom()
    {
        isSinglePlayer = false;
        PhotonNetwork.JoinRandomRoom();
    }

    public void TryLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion
    #region CALLBACKS

    public override void OnConnectedToMaster()
    {
        Debug.Log($"PUN: Connected to master server on region {PhotonNetwork.CloudRegion}!");
        OnConnectedServer?.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"PUN: Disconnected from server with cause: {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN: No Random Room to join. Creating room...");
        isRoomCreated = true;

        string roomName = GameState.principalId + Guid.NewGuid();
        Debug.Log($"PUN: Creating room {roomName}");
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        OnCreatingRoom?.Invoke();
    }

    public void CreateSinglePlayerRoom()
    {
        isRoomCreated = true;
        isSinglePlayer = true;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 1 });
        OnCreatingRoom?.Invoke();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room succesfully!");
        if (isRoomCreated)
        {
            if (!isSinglePlayer)
            {
                PhotonNetwork.LoadLevel("GameRoom");
            }
            else
            {
                PhotonNetwork.LoadLevel("SinglePlayerGameRoom");
            }
        }
    }

    public override void OnLeftRoom()
    {
        OnRoomLeft?.Invoke();
    }
    #endregion
}
