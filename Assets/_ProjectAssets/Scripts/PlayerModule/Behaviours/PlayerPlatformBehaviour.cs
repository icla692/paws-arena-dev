using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformBehaviour : MonoBehaviour
{
    public PlayerCustomization playerCustomization;

    void Start()
    {
        playerCustomization.SetCat(GameState.selectedNFT.ids);
    }

}
