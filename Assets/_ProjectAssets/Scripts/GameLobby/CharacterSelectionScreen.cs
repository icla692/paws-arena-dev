using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectionScreen : MonoBehaviour
{
    [Header("Managers")]
    public PhotonManager photonManager;
    public LobbyUIManager lobbyUIManager;

    [Header("Internals")]
    public PlayerNameInputField nicknameInput;
    public GameObject startButton;
    public GameObject joinRoomLog;


    private void OnEnable()
    {
        Init();

        nicknameInput.OnPlayerNameUpdated += OnNicknameUpdated;
        PhotonManager.OnCreatingRoom += OnCreatingRoom;
        PhotonManager.OnJoinedRoomEvent += OnRoomJoined;
    }

    private void OnDisable()
    {
        nicknameInput.OnPlayerNameUpdated -= OnNicknameUpdated;
        PhotonManager.OnCreatingRoom -= OnCreatingRoom;
        PhotonManager.OnJoinedRoomEvent -= OnRoomJoined;
    }

    private void Init()
    {
        startButton.SetActive(true);
        joinRoomLog.GetComponent<TextMeshProUGUI>().text = "Finding your opponent...";
        joinRoomLog.SetActive(false);
    }

    private void OnNicknameUpdated(string val)
    {
        startButton.SetActive(!string.IsNullOrEmpty(val));
    }

    public void TryJoinRoom()
    {
        startButton.SetActive(false);
        joinRoomLog.SetActive(true);

        photonManager.ConnectToRandomRoom();
    }

    private void OnCreatingRoom()
    {
        joinRoomLog.GetComponent<TextMeshProUGUI>().text = "No open match. Making a new one...";
    }

    private void OnRoomJoined()
    {
        lobbyUIManager.OpenGameMatchingScreen();
    }
}
