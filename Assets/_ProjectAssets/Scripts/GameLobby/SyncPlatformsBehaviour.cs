using Anura.Templates.MonoSingleton;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformPose
{
    [SerializeField]
    public Vector3 pos;
    [SerializeField]
    public Vector3 scale;
}

public class SyncPlatformsBehaviour : MonoSingleton<SyncPlatformsBehaviour>
{
    public PUNRoomUtils punRoomUtils;
    [Space]
    public GameObject syncPlayerPlatformPrefab;
    public PlatformPose player1Pose;
    public PlatformPose player2Pose;
    // Start is called before the first frame update
    void Start()
    {
        var go = PhotonNetwork.Instantiate(syncPlayerPlatformPrefab.name, player1Pose.pos, Quaternion.identity);
    }

    public void InstantiateBot()
    {
        var go = GameObject.Instantiate(syncPlayerPlatformPrefab, player2Pose.pos, Quaternion.identity);
        go.GetComponent<SyncPlayerPlatformBehaviour>().isBot = true;
    }
    public PlatformPose GetMySeatPosition(PhotonView photonView, bool isBot)
    {
        if (isBot)
        {
            return player2Pose;
        }

        if(photonView == null)
        {
            return player1Pose;
        }

        List<Player> players = punRoomUtils.GetOtherPlayers();
        if (players.Count == 1)
        {
            int otherPlayerSeat = Int32.Parse(players[0].CustomProperties["seat"].ToString());
            if ((photonView.IsMine && otherPlayerSeat == 1) || (!photonView.IsMine && otherPlayerSeat == 0))
                return player1Pose;
            if ((photonView.IsMine && otherPlayerSeat == 0) || (!photonView.IsMine && otherPlayerSeat == 1))
                return player2Pose;
        }

        return player1Pose;
    }
}
