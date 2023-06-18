using UnityEngine;

[CreateAssetMenu(fileName = "LevelMilkRewardNew", menuName = "ScriptableObjects/LevelRewards/Milk")]
public class LevelMilkReward : LevelRewardBase
{
    [SerializeField] int amount;

    public int Amount => amount;

    public override void Claim()
    {
        DataManager.Instance.PlayerData.JugOfMilk += amount;
    }
}