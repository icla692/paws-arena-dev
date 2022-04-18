using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PUNGameRoomManager : MonoBehaviourPunCallbacks
{
    public event Action<int> OnPlayerConnectedToScene;
    private const string roomKey_usersInScene = "usersInScene";
    private const string playerKey_seat= "seat";

    public int GetMySeat()
    {
        return Int32.Parse(PhotonNetwork.LocalPlayer.CustomProperties[playerKey_seat].ToString());
    }

    public int GetUsersInScene()
    {
        var hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        if (hashtable.TryGetValue(roomKey_usersInScene, out object userCountObject))
        {
            return (int)userCountObject;
        }

        return 0;
    }

    public void SetUsersInScene(int count)
    {
        var hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        hashtable[roomKey_usersInScene] = count;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.TryGetValue(roomKey_usersInScene, out object userCountObject))
        {
            OnPlayerConnectedToScene?.Invoke((int)userCountObject);
        }
    }
}
