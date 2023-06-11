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
                ValuablesManager.Instance.CommonCrystal += Amount;
                break;
            case LevelRewardType.UncommonShard:
                ValuablesManager.Instance.UncommonCrystal += Amount;
                break;
            case LevelRewardType.RareShard:
                ValuablesManager.Instance.RareCrystal += Amount;
                break;
            case LevelRewardType.EpicShard:
                ValuablesManager.Instance.EpicCrystal += Amount;
                break;
            case LevelRewardType.LengedaryShard:
                ValuablesManager.Instance.LegendaryCrystal += Amount;
                break;
            default:
                break;
        }
    }
}