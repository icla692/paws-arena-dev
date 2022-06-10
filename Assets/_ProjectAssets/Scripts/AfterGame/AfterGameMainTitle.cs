using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGameMainTitle : MonoBehaviour
{
    public GameObject winTitle;
    public GameObject loseTitle;
    public GameObject drawTitle;
    void Start()
    {
        if ((GameState.gameResolveState == GameResolveState.PLAYER_1_WIN && PhotonNetwork.LocalPlayer.IsMasterClient) ||
            (GameState.gameResolveState == GameResolveState.PLAYER_2_WIN && !PhotonNetwork.LocalPlayer.IsMasterClient))
        {
            winTitle.SetActive(true);
        }
        else if ((GameState.gameResolveState == GameResolveState.PLAYER_1_WIN && !PhotonNetwork.LocalPlayer.IsMasterClient) ||
            (GameState.gameResolveState == GameResolveState.PLAYER_2_WIN && PhotonNetwork.LocalPlayer.IsMasterClient))
        {
            loseTitle.SetActive(true);
        }
        else if (GameState.gameResolveState == GameResolveState.DRAW)
        {
            drawTitle.SetActive(true);
        }
    }
}
