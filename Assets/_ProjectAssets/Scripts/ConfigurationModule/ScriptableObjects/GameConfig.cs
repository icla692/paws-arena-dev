using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Config", menuName = "Configurations/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public bool enableDevLogs = false;
}
