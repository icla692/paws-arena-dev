using UnityEngine;
using System;

[Serializable]
public class LevelReward
{
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public LevelRewardBase Reward { get; private set; }
}
