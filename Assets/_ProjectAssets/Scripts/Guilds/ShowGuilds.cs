using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGuilds : MonoBehaviour
{
    [SerializeField] private GuildPanel guildPanel;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ShowGuild);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ShowGuild);
    }

    private void ShowGuild()
    {
        guildPanel.Setup();
    }
}
