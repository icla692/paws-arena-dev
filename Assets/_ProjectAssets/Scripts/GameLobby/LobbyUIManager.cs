using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyUIManager : MonoBehaviour
{
    [Header("Password")]
    public GameObject passwordScreen;

    [Header("Connecting")]
    public GameObject connectingToServerScreen;

    [Header("NFT Selection")]
    public List<GameObject> nftSelectionScreens;

    [Header("Equipment")]
    public List<GameObject> equipmentScreens;

    [Header("Game Menu")]
    public List<GameObject> gameMenuScreens;
    //public GameObject gameMenuSprites;

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

        if(GameState.selectedNFT != null)
        {
            OpenGameMenu();
        }
    }

    private void OnDisable()
    {
        PhotonManager.OnStartedConnection -= OpenLoadingScreen;
        PhotonManager.OnConnectedServer -= OpenGameMenu;
    }

    private void OnDestroy()
    {
        LeanTween.cancelAll();
    }

    public void OpenNFTSelectionScreen()
    {
        passwordScreen.SetActive(false);
        connectingToServerScreen.SetActive(false);
        connectingToRoom.SetActive(false);

        foreach (GameObject screen in gameMenuScreens)
        {
            screen.SetActive(false);
        }

        foreach (GameObject screen in nftSelectionScreens)
        {
            screen.SetActive(true);
        }
    }

    public void OpenEquipmentScreen()
    {
        foreach (GameObject screen in gameMenuScreens)
        {
            screen.SetActive(false);
        }

        foreach (GameObject screen in equipmentScreens)
        {
            screen.SetActive(true);
        }
    }

    private void OpenLoadingScreen()
    {
        foreach (GameObject screen in nftSelectionScreens)
        {
            screen.SetActive(false);
        }
        loadingScreen.SetActive(true);
    }

    public void OpenGameMenu()
    {
        loadingScreen.SetActive(false);
        connectingToServerScreen.SetActive(false);
        passwordScreen.SetActive(false);

        foreach(GameObject screen in equipmentScreens)
        {
            screen.SetActive(false);
        }

        foreach (GameObject screen in gameMenuScreens)
        {
            screen.SetActive(true);
        }
    }

    private void CloseGameMenu()
    {
        foreach (GameObject screen in gameMenuScreens)
        {
            screen.SetActive(false);
        }
    }

    public void TryConnectToRoom()
    {
        CloseGameMenu();
        connectingToRoom.SetActive(true);
        lobbyPhotonConnection.TryJoinRoom();
    }

    public void TryConnectToTrainingRoom()
    {
        //CloseGameMenu();
        //connectingToRoom.SetActive(true);
        //lobbyPhotonConnection.TryJoinSinglePlayerRoom();
        SceneManager.LoadScene("GameSceneTutorial", LoadSceneMode.Single);
    }
    public void GoToSinglePlayer()
    {
        //CloseGameMenu();
        //connectingToRoom.SetActive(true);
        //lobbyPhotonConnection.TryJoinSinglePlayerRoom();
        SceneManager.LoadScene("PlayerTest_new", LoadSceneMode.Single);
    }

    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard", LoadSceneMode.Single);
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
