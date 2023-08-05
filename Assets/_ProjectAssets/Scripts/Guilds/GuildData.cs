using System;
using System.Collections.Generic;

[Serializable]
public class GuildData
{
    public string Id;
    public string Name;
    public int FlagId;
    public List<GuildPlayerData> Players;
    public int MinimumPoints;
    public int MatchesWon;
}
