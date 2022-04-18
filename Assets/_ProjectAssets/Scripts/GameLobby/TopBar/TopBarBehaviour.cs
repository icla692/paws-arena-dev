using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBarBehaviour : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nickname;
    public PlayerNameInputField nicknameInput;

    private void OnEnable()
    {
        nicknameInput.OnPlayerNameUpdated += OnNicknameUpdated;
    }

    private void OnDisable()
    {
        nicknameInput.OnPlayerNameUpdated -= OnNicknameUpdated;
    }

    private void OnNicknameUpdated(string obj)
    {
        nickname.text = obj;
    }
}
