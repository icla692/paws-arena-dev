using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformBehaviour : MonoBehaviour
{
    public PlayerCustomization playerCustomization;

    void OnEnable()
    {
        playerCustomization.SetCat(GameState.selectedNFT.imageUrl, GameState.selectedNFT.ids);
    }

}
