using Photon.Pun;
using Photon.Realtime;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static event Action OnJoinedRoomEvent;
    public static event Action<string> onPlayerJoined;
    public static event Action onPlayerLeft;

    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    private string gameVersion = "1";

    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"PUN: Connected to master server on region {PhotonNetwork.CloudRegion}!");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"PUN: Disconnected from server with cause: {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN: No Random Room to join. Creating room...");
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room succesfully!");
        OnJoinedRoomEvent?.Invoke();
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player Joined room {newPlayer.NickName}");
        onPlayerJoined?.Invoke(newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"Player Left Room {otherPlayer.NickName}");
        onPlayerLeft?.Invoke();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameRoom");
    }
}
