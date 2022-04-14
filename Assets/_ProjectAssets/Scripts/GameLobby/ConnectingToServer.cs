using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingToServer : MonoBehaviour
{
    [Header("Managers")]
    public PhotonManager photonManager;
    public LobbyUIManager lobbyUIManager;

    [Header("Internals")]
    public GameObject connectButton;
    public GameObject logText;
    private void OnEnable()
    {
        PhotonManager.OnConnectedServer += OnConnectedToServer;
    }
    private void OnDisable()
    {

        PhotonManager.OnConnectedServer -= OnConnectedToServer;
    }

    public void TryEnterGame()
    {
        connectButton.SetActive(false);
        logText.SetActive(true);

        photonManager.Connect();
    }

    private void OnConnectedToServer()
    {
        lobbyUIManager.OpenCharacterSelectionScreen();
    }
}
