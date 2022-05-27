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
        };
    }
    public void Apply()
    {
        PlayerPrefs.SetString(key, JsonUtility.ToJson(this));
    }
}
