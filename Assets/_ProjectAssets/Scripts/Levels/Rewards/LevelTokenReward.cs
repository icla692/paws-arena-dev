using UnityEngine;

[CreateAssetMenu(fileName = "LevelTokenRewardNew", menuName = "ScriptableObjects/LevelRewards/Token")]
public class LevelTokenReward : LevelRewardBase
{
    [SerializeField] float amount;

    public float Amount => amount;

    public override void Claim()
    {
        Debug.Log("Implement token reward for levels");
    }
}
