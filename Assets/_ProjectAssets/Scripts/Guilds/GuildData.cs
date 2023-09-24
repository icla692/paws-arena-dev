using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class GuildData
{
    private List<GuildPlayerData> players;
    public string Id;
    public string Name;
    public int KittyId;
    public int FlagIndex;
    public int MinimumPoints;
    public List<int> BattlesHistory = new();
    [JsonIgnore]public int NextIndex;
    private List<GuildPlayerData> originalPlayers = new () ;
    public string BattleId;
    public int MatchesWon;
    
    [JsonIgnore]public GuildBattle Battle;

    public List<GuildPlayerData> Players
    {
        get => players;
        set
        {
            originalPlayers = value.ToList();
            players = value;
            NextIndex = players.Count;

            foreach (var _player in players.ToList())
            {
                if (_player == null)
                {
                    players.Remove(_player);
                }
            }
        }
    }

    public GuildPlayerData GetPlayer(string _playerId)
    {
        foreach (var _player in players.ToList())
        {
            if (_player.Id == _playerId)
            {
                return _player;
            }
        }

        return null;
    }

    public void ReorderPlayersByPoints()
    {
        players = players.OrderByDescending(_player => _player.Points).ToList();
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Place = i + 1;
        }
    }

    public void KickPlayer(string _playerId)
    {
        foreach (var _player in players.ToList())
        {
            if (_player.Id == _playerId)
            {
                players.Remove(_player);
                return;
            }
        }
    }

    public GuildPlayerData GetLeader()
    {
        foreach (var _player in players.ToList())
        {
            if (_player.IsLeader)
            {
                return _player;
            }
        }

        return null;
    }

    public int GetPlayerIndex(string _playerId)
    {
        for (int i = 0; i < originalPlayers.Count; i++)
        {
            if (originalPlayers[i]==null)
            {
                continue;
            }

            if (originalPlayers[i].Id==_playerId)
            {
                return i;
            }
        }

        throw new Exception("Cant find player");
    }

    [JsonIgnore] public int SumOfPoints
    {
        get
        {
            int _sumOfPoints = 0;
            foreach (var _player in Players)
            {
                _sumOfPoints += _player.Points;
            }

            return _sumOfPoints;
        }
    }

}
