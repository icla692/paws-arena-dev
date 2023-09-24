using System;
using System.Collections.Generic;

[Serializable]
public class GuildBattle
{
    public string GuildOneId;
    public string GuildTwoId;
    public DateTime EndDate;
    public List<GuildBattleReward> Rewards;
}
