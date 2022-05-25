using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    private const string key = "gamesettings";

    [SerializeField]
    public bool hasMusic;
    [SerializeField]
    public bool hasSoundFX;
    [SerializeField]
    public int[] currentResolution;

    public static GameSettings Default()
    {
        if (PlayerPrefs.HasKey(key))
        {
            return JsonUtility.FromJson<GameSettings>(PlayerPrefs.GetString(key));
        }
        return new GameSettings()
        {
            hasMusic = true,
            hasSoundFX = true,
            currentResolution = new int[] { 1920, 1080 }
        };
    }
    public void Apply()
    {
        PlayerPrefs.SetString(key, JsonUtility.ToJson(this));
        Screen.SetResolution((int)currentResolution[0], (int)currentResolution[1], false);
    }
}
