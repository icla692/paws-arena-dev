using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNicknameButton : MonoBehaviour
{
    public event Action<string> OnPlayerNameUpdated;
    const string playerNamePrefKey = "PlayerName";

    [SerializeField]
    private TMPro.TextMeshProUGUI nicknameText;
    [SerializeField]
    private InputModal inputModal;

    private void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            nicknameText.text = PlayerPrefs.GetString(playerNamePrefKey); ;
        }else
        {
            nicknameText.text = "ICKITTY";
            EnableEdit(false);
        }

        OnPlayerNameUpdated?.Invoke(nicknameText.text);
        GameState.nickname = GameState.nickname = PhotonNetwork.NickName = nicknameText.text;
    }

    public void EnableEdit(bool isCancelable)
    {
        inputModal.Show("Nickname", "Nickname", isCancelable, (nickname) =>
        {
            SetPlayerName(nickname);
        });
    }

    public void SetPlayerName(string value)
    {
        PhotonNetwork.NickName = nicknameText.text = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
        OnPlayerNameUpdated?.Invoke(value);
    }
}
