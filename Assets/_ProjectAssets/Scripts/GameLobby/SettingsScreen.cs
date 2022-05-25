using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScreen : MonoBehaviour
{
    public LobbyUIManager uiManager;
    public CustomToggle hasMusicToggle;
    public CustomToggle hasSoundFxToggle;
    public NextPrevStringList settingsList;

    private void OnEnable()
    {
        ShowSettings(GameState.gameSettings);
    }

    public void Apply()
    {
        GameState.gameSettings.hasMusic = hasMusicToggle.isOn;
        GameState.gameSettings.hasSoundFX = hasSoundFxToggle.isOn;
        var newRes = settingsList.GetValue();
        var values = newRes.Split('x');
        GameState.gameSettings.currentResolution = new int[] { int.Parse(values[0]), int.Parse(values[1]) };
        GameState.gameSettings.Apply();

        uiManager.CloseSettings();
    }
    private void ShowSettings(GameSettings gameSettings)
    {
        hasMusicToggle.SetValue(gameSettings.hasMusic);
        hasSoundFxToggle.SetValue(gameSettings.hasSoundFX);

        settingsList.SetValue($"{gameSettings.currentResolution[0]}x{gameSettings.currentResolution[1]}");
    }
}
