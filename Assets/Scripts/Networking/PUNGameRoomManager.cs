using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PUNGameRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0), Quaternion.identity);
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
