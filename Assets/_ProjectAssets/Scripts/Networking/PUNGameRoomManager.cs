using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PUNGameRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    private void Start()
    {
        int seat = Int32.Parse(PhotonNetwork.LocalPlayer.CustomProperties["seat"].ToString());
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(seat == 0 ? 14 : 40, 20), Quaternion.identity);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
