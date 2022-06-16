using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordScreen : MonoBehaviour
{
    public LobbyUIManager uiManager;

    public TMPro.TMP_InputField passwordInput;
    public GameObject startButton;
    public GameObject logText;

    public void OnSubmit()
    {
        if(passwordInput.text.ToUpper() == "XYJHDD")
        {
            StartCoroutine(WalletConnectionCoroutine());
        }
    }

    private IEnumerator WalletConnectionCoroutine()
    {
        startButton.SetActive(false);
        passwordInput.gameObject.SetActive(false);
        logText.SetActive(true);

        var text = logText.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Pretending we connect to ICP Wallet...";

        //Mock data. To be removed
        GameState.nfts.Add(new NFT() { imageUrl = "https://v3zkd-syaaa-aaaah-qcm5a-cai.raw.ic0.app/?&tokenid=j4efc-wikor-uwiaa-aaaaa-b4ath-iaqca-aaabd-a" });
        GameState.walletId = "asd";

        yield return new WaitForSeconds(1f);
        text.text = "Pretending we get info from server...";

        uiManager.OpenNFTSelectionScreen();
    }
}
