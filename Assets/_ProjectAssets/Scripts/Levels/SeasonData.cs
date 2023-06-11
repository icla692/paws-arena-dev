using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class SeasonData
{
    // level wariables
    int seasonNumber;
    DateTime seasonEnds;
    bool hasPass;
    int experience;
    int level;
    int experienceOnCurrentLevel;
    int scaler=1000;
    List<ClaimedReward> claimedLevelRewards = new List<ClaimedReward>();

    public Action UpdatedClaimedLevels;
    public Action UpdatedHasPass;
    public Action UpdatedExp;

   [JsonIgnore] public int Scaler => scaler;
   [JsonIgnore] public int ExperienceOnCurrentLevel => experienceOnCurrentLevel;

    public int Experience
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
            UpdatedExp?.Invoke();
            CalculateLevel();
        }
    }

    void CalculateLevel()
    {
        int _experience = Experience;
        int _level = 0;

        while (_experience >= scaler)
        {
            _level++;
            _experience -= scaler;
        }

        experienceOnCurrentLevel = _experience;
        level = _level;

    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public void AddCollectedLevelReward(ClaimedReward _reward)
    {
        claimedLevelRewards.Add(_reward);
        UpdatedClaimedLevels?.Invoke();
    }

    public List<ClaimedReward> ClaimedLevelRewards
    {
        get
        {
            return claimedLevelRewards;
        }
        set
        {
            claimedLevelRewards = value;
        }
    }
    public int SeasonNumber
    {
        get
        {
            return seasonNumber;
        }
        set
        {
            seasonNumber = value;
        }
    }

    public DateTime SeasonEnds
    {
        get
        {
            return seasonEnds;
        }
        set
        {
            seasonEnds = value;
        }
    }

    public bool HasPass
    {
        get
        {
            return hasPass;
        }
        set
        {
            hasPass = value;
            UpdatedHasPass?.Invoke();
        }
    }

    public bool HasClaimed(LevelRewardBase _reward, int _level)
    {
        foreach (var _claimedReward in claimedLevelRewards)
        {
            if (_claimedReward.IsPremium == _reward.IsPremium && _claimedReward.Level == _level)
            {
                return true;
            }
        }

        return false;
    }
}
