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

    [Header("Settings")]
    public GameObject settings;

    [Header("Others")]
    public GameObject loadingScreen;


    private void OnEnable()
    {
        PhotonManager.OnStartedConnection += OpenLoadingScreen;
        PhotonManager.OnConnectedServer += OpenGameMenu;
    }

    private void OnDisable()
    {
        PhotonManager.OnStartedConnection -= OpenLoadingScreen;
        PhotonManager.OnConnectedServer -= OpenGameMenu;
    }
    public void OpenNFTSelectionScreen()
    {
        connectingToServerScreen.SetActive(false);
        gameMenuPanel.SetActive(false);
        gameMenuSprites.SetActive(false);
        connectingToRoom.SetActive(false);

        nftSelectionScreen.SetActive(true);
    }

    private void OpenLoadingScreen()
    {
        nftSelectionScreen.SetActive(false);
        loadingScreen.SetActive(true);
    }

    public void OpenGameMenu()
    {
        loadingScreen.SetActive(false);
        connectingToServerScreen.SetActive(false);

        gameMenuPanel.SetActive(true);
        gameMenuSprites.SetActive(true);
    }

    private void CloseGameMenu()
    {
        gameMenuPanel.SetActive(false);
        gameMenuSprites.SetActive(false);
    }

    public void TryConnectToRoom()
    {
        CloseGameMenu();
        connectingToRoom.SetActive(true);
        lobbyPhotonConnection.TryJoinRoom();
    }

    public void TryConnectToTrainingRoom()
    {
        CloseGameMenu();
        connectingToRoom.SetActive(true);
        lobbyPhotonConnection.TryJoinSinglePlayerRoom();
    }

    public void OpenSettings()
    {
        CloseGameMenu();
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        OpenGameMenu();
        settings.SetActive(false);
    }
}
