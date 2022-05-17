using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LobbyUIManager : MonoBehaviour
{
    [Header("Connecting")]
    public GameObject connectingToServerScreen;

    [Header("NFT Selection")]
    public GameObject nftSelectionScreen;

    [Header("Game Menu")]
    public GameObject gameMenuPanel;
    public GameObject gameMenuSprites;

    [Header("Connecting")]
    public GameObject connectingToRoom;
    public LobbyPhotonConnection lobbyPhotonConnection;

    public void OpenNFTSelectionScreen()
    {
        connectingToServerScreen.SetActive(false);
        nftSelectionScreen.SetActive(true);

        gameMenuPanel.SetActive(false);
        gameMenuSprites.SetActive(false);

        connectingToRoom.SetActive(false);
    }

    public void OpenGameMenu()
    {
        nftSelectionScreen.SetActive(false);

        gameMenuPanel.SetActive(true);
        gameMenuSprites.SetActive(true);
    }

    public void TryConnectToRoom()
    {
        gameMenuPanel.SetActive(false);
        gameMenuSprites.SetActive(false);

        connectingToRoom.SetActive(true);
        lobbyPhotonConnection.TryJoinRoom();
    }
}
