using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingToServer : MonoBehaviour
{
    [Header("Managers")]
    public LobbyUIManager lobbyUIManager;

    [Header("Internals")]
    public GameObject connectButton;
    public GameObject logText;

    public void ConnectToWallet()
    {
        StartCoroutine(WalletConnectionCoroutine());
    }

    private IEnumerator WalletConnectionCoroutine()
    {
        connectButton.SetActive(false);
        logText.SetActive(true);
        var text = logText.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Pretending we connect to ICP Wallet...";

        //Mock data. To be removed
        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?tokenid=xgzpf-bikor-uwiaa-aaaaa-cqace-eaqca-aacgt-q" });
        GameState.walletId = "asd";

        yield return new WaitForSeconds(1f);
        text.text = "Pretending we get info from server...";

        lobbyUIManager.OpenNFTSelectionScreen();
    }
}
