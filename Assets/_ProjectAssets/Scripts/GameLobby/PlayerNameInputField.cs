using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInputField : MonoBehaviour
{
    public event Action<string> OnPlayerNameUpdated;
    const string playerNamePrefKey = "PlayerName";

    private void Start()
    {
        string defaultName = string.Empty;
        TMP_InputField inputField = GetComponent<TMP_InputField>();
        if(inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                inputField.text = defaultName = PlayerPrefs.GetString(playerNamePrefKey); ;
            }
        }

        OnPlayerNameUpdated?.Invoke(defaultName);
        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
        OnPlayerNameUpdated?.Invoke(value);
    }
}
