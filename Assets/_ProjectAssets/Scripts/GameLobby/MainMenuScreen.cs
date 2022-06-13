using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anura.ConfigurationModule.Managers;

public class MainMenuScreen : MonoBehaviour
{
    public TMPro.TextMeshProUGUI betCoins;

    void OnEnable()
    {
        betCoins.text = "" + ConfigurationManager.Instance.Config.GetBetValue();
    }
}
