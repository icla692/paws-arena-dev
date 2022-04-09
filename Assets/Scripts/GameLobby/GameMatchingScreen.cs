using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMatchingScreen : MonoBehaviour
{
    [Header("Managers")]
    public PhotonManager photonManager;
    public LobbyUIManager lobbyUIManager;

    [Header("Internals")]
    public TextMeshProUGUI opponentName;
    public GameObject startGameButton;

    private void OnEnable()
    {
        Init();
        PhotonManager.onPlayerJoined += OnPlayerJoined;
        PhotonManager.onPlayerLeft += OnPlayerLeft;
        PhotonManager.OnRoomLeft += OnRoomLeft;
    }

    private void OnDisable()
    {
        PhotonManager.onPlayerJoined -= OnPlayerJoined;
        PhotonManager.onPlayerLeft -= OnPlayerLeft;
        PhotonManager.OnRoomLeft -= OnRoomLeft;
    }

    private void Init()
    {
        opponentName.text = "000000";
        startGameButton.SetActive(false);

        List<string> otherNicknames = photonManager.GetPlayers();
        if(otherNicknames.Count >= 1)
        {
            OnPlayerJoined(otherNicknames[0]);
        }
    }

    private void OnPlayerJoined(string opponentNickname)
    {
        opponentName.text = opponentNickname;
        startGameButton.SetActive(true);
    }

    private void OnPlayerLeft()
    {
        opponentName.text = "000000";
        startGameButton.SetActive(false);
    }

    public void TryExitRoom()
    {
        photonManager.TryLeaveRoom();
    }

    private void OnRoomLeft()
    {
        lobbyUIManager.OpenCharacterSelectionScreen();
    }
}
