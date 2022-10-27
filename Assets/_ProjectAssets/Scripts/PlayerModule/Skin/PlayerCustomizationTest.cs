using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCustomization))]
public class PlayerCustomizationTest : MonoBehaviour
{

    private PlayerCustomization playerCustomization;

    private void Awake()
    {
        playerCustomization = GetComponent<PlayerCustomization>();
    }

    //********//

    public string colorId = "kittyColor1";

    [Button(text:"Set Color", enabledMode: EButtonEnableMode.Playmode)]
    private void SetColor()
    {
        playerCustomization.SetKittyColor(colorId);
    }

    public string eyesId = "eyes1";

    [Button(text: "Set Eyes", enabledMode: EButtonEnableMode.Playmode)]
    private void SetEyes()
    {
        playerCustomization.SetEyes(eyesId);
    }
}
