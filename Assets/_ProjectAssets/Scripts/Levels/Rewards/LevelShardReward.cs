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
                DataManager.Instance.PlayerData.Crystals.CommonCrystal += Amount;
                break;
            case LevelRewardType.UncommonShard:
                DataManager.Instance.PlayerData.Crystals.UncommonCrystal += Amount;
                break;
            case LevelRewardType.RareShard:
                DataManager.Instance.PlayerData.Crystals.RareCrystal += Amount;
                break;
            case LevelRewardType.EpicShard:
                DataManager.Instance.PlayerData.Crystals.EpicCrystal += Amount;
                break;
            case LevelRewardType.LengedaryShard:
                DataManager.Instance.PlayerData.Crystals.LegendaryCrystal += Amount;
                break;
            default:
                break;
        }
    }
}