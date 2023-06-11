using UnityEngine;

[CreateAssetMenu(fileName = "LevelSnackRewardNew", menuName = "ScriptableObjects/LevelRewards/Snack")]
public class LevelSnackReward : LevelRewardBase
{
    [SerializeField] int amount;

    public int Amount => amount;

    public override void Claim()
    {
        ValuablesManager.Instance.Snacks += Amount;
    }
}
