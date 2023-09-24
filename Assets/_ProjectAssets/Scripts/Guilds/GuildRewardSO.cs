using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGuildBattleReward", menuName = "ScriptableObjects/GuildBattleReward")]
public class GuildRewardSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Name { get; private set; }

    private static List<GuildRewardSO> allRewards;

    public static List<GuildRewardSO> GetAll()
    {
        LoadAllRewards();
        return allRewards.ToList();
    }

    public static GuildRewardSO Get(int _id)
    {
        LoadAllRewards();
        return allRewards.First(_element => _element.Id == _id);
    }

    private static void LoadAllRewards()
    {
        if (allRewards != null)
        {
            return;
        }

        allRewards = Resources.LoadAll<GuildRewardSO>("GuildBattleRewards/").ToList();
    }
}
