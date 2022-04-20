using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PUNGameRoomManager : MonoBehaviourPunCallbacks
{
    private const string roomKey_usersInScene = "usersInScene";
    private const string playerKey_seat= "seat";

    public int GetMySeat()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(playerKey_seat))
        {
            return 0;
        }

        bool tryParse = Int32.TryParse(PhotonNetwork.LocalPlayer.CustomProperties[playerKey_seat].ToString(), out int result);
        
        return result;
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        //if (propertiesThatChanged.TryGetValue(roomKey_usersInScene, out object userCountObject))
        //{
        //}
    }
}
