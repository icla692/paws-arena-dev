using UnityEngine;

[CreateAssetMenu(fileName = "LevelShardRewardNew", menuName = "ScriptableObjects/LevelRewards/Shard")]

public class LevelShardReward : LevelRewardBase
{
    [SerializeField] int amount;

    public int Amount => amount;

    public override void Claim()
    {
        switch (Type)
        {
            case LevelRewardType.CommonShard:
                DataManager.Instance.PlayerData.CommonCrystal += Amount;
                break;
            case LevelRewardType.UncommonShard:
                DataManager.Instance.PlayerData.UncommonCrystal += Amount;
                break;
            case LevelRewardType.RareShard:
                DataManager.Instance.PlayerData.RareCrystal += Amount;
                break;
            case LevelRewardType.EpicShard:
                DataManager.Instance.PlayerData.EpicCrystal += Amount;
                break;
            case LevelRewardType.LengedaryShard:
                DataManager.Instance.PlayerData.LegendaryCrystal += Amount;
                break;
            default:
                break;
        }
    }
}