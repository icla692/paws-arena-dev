using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInputField : MonoBehaviour
{
    public event Action<string> OnPlayerNameUpdated;
    const string playerNamePrefKey = "PlayerName";

    private void Start()
    {
        string defaultName = string.Empty;
        InputField inputField = GetComponent<InputField>();
        if(inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
                OnPlayerNameUpdated?.Invoke(defaultName);
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        OnPlayerNameUpdated?.Invoke(value);

        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
