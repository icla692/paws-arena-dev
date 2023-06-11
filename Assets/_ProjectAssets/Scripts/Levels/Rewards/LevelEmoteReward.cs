using UnityEngine;

[CreateAssetMenu(fileName = "LevelEmoteRewardNew", menuName = "ScriptableObjects/LevelRewards/Emote")]
public class LevelEmoteReward : LevelRewardBase
{
    [SerializeField] int id;

    public int Id => id;

    public override void Claim()
    {
        Debug.Log("Implement claim functionality for emote rewards");
    }
}
