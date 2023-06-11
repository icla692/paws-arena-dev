using UnityEngine;

[CreateAssetMenu(fileName = "LevelItemRewardNew", menuName = "ScriptableObjects/LevelRewards/Item")]
public class LevelItemReward : LevelRewardBase
{
    public override void Claim()
    {
        Debug.Log("Implement item reward for levels");
    }
}