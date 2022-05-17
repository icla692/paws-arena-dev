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
        StartCoroutine(ConnectionCoroutine());
    }

    private void OnConnectedToServer()
    {
        lobbyUIManager.OpenNFTSelectionScreen();
    }

    private IEnumerator ConnectionCoroutine()
    {
        connectButton.SetActive(false);
        logText.SetActive(true);
        var text = logText.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Pretending we connect to ICP Wallet...";

        //Mock data. To be removed
        GameState.nfts.Add(new NFT() { imageUrl = "https://v3zkd-syaaa-aaaah-qcm5a-cai.raw.ic0.app/?&tokenid=j4efc-wikor-uwiaa-aaaaa-b4ath-iaqca-aaabd-a" });
        GameState.walletId = "asd";

        yield return new WaitForSeconds(1f);
        text.text = "Pretending we get info from server...";

        yield return new WaitForSeconds(1f);
        text.text = "Connecting to game master server...";

        photonManager.Connect();
    }
}
