using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformBehaviour : MonoBehaviour
{
    public PlayerCustomization playerCustomization;
    public bool isMyCat = true;

    void OnEnable()
    {
        if (isMyCat)
        {
            playerCustomization.SetCat(GameState.selectedNFT.imageUrl, GameState.selectedNFT.ids);
        }
    }


    public async void SetCat(string imageUrl)
    {
        NFT nft = new NFT()
        {
            imageUrl = imageUrl
        };

        await nft.GrabImage();
        playerCustomization.SetCat(nft.imageUrl, nft.ids);
    }

}
