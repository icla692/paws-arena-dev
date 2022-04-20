using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LobbyUIManager : MonoBehaviour
{
    public GameObject connectingToServerScreen;
    public GameObject topBar;
    public GameObject characterSelectionScreen;
    public GameObject gameMatchingScreen;

    public void OpenCharacterSelectionScreen()
    {
        connectingToServerScreen.SetActive(false);
        gameMatchingScreen.SetActive(false);

        topBar.SetActive(true);
        characterSelectionScreen.SetActive(true);
    }
}
