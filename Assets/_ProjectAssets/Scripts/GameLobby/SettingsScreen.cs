using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScreen : MonoBehaviour
{
    public LobbyUIManager uiManager;
    public CustomToggle hasMusicToggle;
    public CustomToggle hasSoundFxToggle;

    private void OnEnable()
    {
        ShowSettings(GameState.gameSettings);
    }

    public void Apply()
    {
        GameState.gameSettings.hasMusic = hasMusicToggle.isOn;
        GameState.gameSettings.hasSoundFX = hasSoundFxToggle.isOn;
        GameState.gameSettings.Apply();

        uiManager.CloseSettings();
    }
    private void ShowSettings(GameSettings gameSettings)
    {
        hasMusicToggle.SetValue(gameSettings.hasMusic);
        hasSoundFxToggle.SetValue(gameSettings.hasSoundFX);
    }
}
