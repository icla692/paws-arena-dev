using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedNFTImage : MonoBehaviour
{
    public RawImage image;
    public GameObject enterArenaBut;

    private void OnEnable()
    {
        enterArenaBut.SetActive(false);
        GameState.onSelectedNFT += UpdateSelectedNFT;
    }

    private void OnDisable()
    {
        GameState.onSelectedNFT -= UpdateSelectedNFT;
    }

    private void UpdateSelectedNFT(NFT nft)
    {
        if (image != null)
        {
            image.texture = nft.imageTex;
        }
        enterArenaBut.SetActive(true);
    }
}
