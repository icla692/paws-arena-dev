using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGameCoins : MonoBehaviour
{
    public TMPro.TextMeshProUGUI totalCoinsValue;

    void Start()
    {
        if ((GameState.gameResolveState == GameResolveState.PLAYER_1_WIN && PhotonNetwork.LocalPlayer.IsMasterClient) ||
            (GameState.gameResolveState == GameResolveState.PLAYER_2_WIN && !PhotonNetwork.LocalPlayer.IsMasterClient))
        {
            totalCoinsValue.color = Color.green;
        }
        else if ((GameState.gameResolveState == GameResolveState.PLAYER_1_WIN && !PhotonNetwork.LocalPlayer.IsMasterClient) ||
            (GameState.gameResolveState == GameResolveState.PLAYER_2_WIN && PhotonNetwork.LocalPlayer.IsMasterClient))
        {
            totalCoinsValue.color = Color.red;
        }
        else if(GameState.gameResolveState == GameResolveState.DRAW)
        {
            totalCoinsValue.color = Color.yellow;
        }
    }
}
