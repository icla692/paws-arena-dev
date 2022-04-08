using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LobbyUIManager : MonoBehaviour
{
    public PlayerNameInputField nicknameInput;
    public GameObject connectButton;
    public GameObject startGameButton;
    public GameObject log;

    private void OnEnable()
    {
        connectButton.SetActive(false);
        log.SetActive(false);

        nicknameInput.OnPlayerNameUpdated += NicknameUpdated;
        PhotonManager.OnJoinedRoomEvent += OnJoinedRoom;
        PhotonManager.onPlayerJoined += PlayerJoined;
        PhotonManager.onPlayerLeft += PlayerLeft;
    }

    private void OnDisable()
    {
        nicknameInput.OnPlayerNameUpdated -= NicknameUpdated;
        PhotonManager.OnJoinedRoomEvent -= OnJoinedRoom;
        PhotonManager.onPlayerJoined -= PlayerJoined;
        PhotonManager.onPlayerLeft -= PlayerLeft;
    }

    private void NicknameUpdated(string nickname)
    {
        connectButton.SetActive(!string.IsNullOrEmpty(nickname));
    }

    private void OnJoinedRoom()
    {
        connectButton.SetActive(false);
        nicknameInput.gameObject.SetActive(false);
        log.SetActive(true);
    }

    private void PlayerJoined(string nickname)
    {
        log.GetComponent<TextMeshProUGUI>().text = $"Your opponent is here!\n {nickname}";
        startGameButton.SetActive(true);
    }

    private void PlayerLeft()
    {
        log.GetComponent<TextMeshProUGUI>().text = $"Player is out :( . Waiting for someone else...";
        startGameButton.SetActive(false);
    }
}
