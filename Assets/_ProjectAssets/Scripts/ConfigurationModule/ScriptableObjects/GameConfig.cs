using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Config", menuName = "Configurations/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public bool enableDevLogs = false;

    public bool isDev = true;
    public string devUrl = "https://localhost:7226";
    public string prodUrl = "https://localhost:7226";
}
