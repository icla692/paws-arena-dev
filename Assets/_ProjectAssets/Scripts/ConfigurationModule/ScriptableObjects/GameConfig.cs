using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEnvironment
{
    DEV,
    STAGING,
    PROD
}

[CreateAssetMenu(fileName = "Config", menuName = "Configurations/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public bool enableDevLogs = false;

    public GameEnvironment env;
    public string devUrl = "https://localhost:7226";
    public string stagingUrl = "https://localhost:7226";
    public string prodUrl = "https://localhost:7226";
}
