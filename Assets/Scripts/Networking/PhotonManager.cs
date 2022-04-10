using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static event Action OnConnectedServer;
    public static event Action OnJoinedRoomEvent;
    public static event Action OnCreatingRoom;
    public static event Action OnRoomLeft;
    public static event Action<string> onPlayerJoined;
    public static event Action onPlayerLeft;

    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    private string gameVersion = "1";


    #region ACTIONS
    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
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
    public List<Player> GetOtherPlayers()
    {
        return PhotonNetwork.PlayerList.Where(player => !player.IsLocal).ToList();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
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
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom });
        OnCreatingRoom?.Invoke();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room succesfully!");
        OnJoinedRoomEvent?.Invoke();
    }

    public override void OnLeftRoom()
    {
        OnRoomLeft?.Invoke();
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

    public void AddPlayerCustomProperty(string key, string value)
    {
        Hashtable hashtable = new Hashtable();
        hashtable[key] = value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
    #endregion
}
