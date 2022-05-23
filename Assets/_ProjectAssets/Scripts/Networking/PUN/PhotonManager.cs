using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static event Action OnStartedConnection;
    public static event Action OnConnectedServer;
    public static event Action OnCreatingRoom;
    public static event Action OnRoomLeft;

    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    private string gameVersion = "1";

    private bool isRoomCreated = false;


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
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom });
        OnCreatingRoom?.Invoke();

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room succesfully!");
        if (isRoomCreated)
        {
            PhotonNetwork.LoadLevel("GameRoom");
        }
    }

    public override void OnLeftRoom()
    {
        OnRoomLeft?.Invoke();
    }
    #endregion
}
