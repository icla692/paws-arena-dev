using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_MockNFT : MonoBehaviour
{
    public TMPro.TMP_InputField urlInputField;
    public Button submitButton;
    public NFTSelection nftSelectionPage;

    private void Awake()
    {
        submitButton.onClick.AddListener(async () =>
        {
            GameState.nfts.Add(new NFT() { imageUrl = urlInputField.text });
            await nftSelectionPage.InitPage();
        });
    }
}
