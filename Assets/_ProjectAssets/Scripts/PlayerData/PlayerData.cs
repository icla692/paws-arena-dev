using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData 
{
    float snacks=0;
    float jugOfMilk=0;
    float glassOfMilk=0;
    float commonCrystal = 0;
    float uncommonCrystal = 0;
    float rareCrystal = 0;
    float epicCrystal = 0;
    float legendaryCristal = 0;
    float giftItem = 0;
    CraftingProcess craftingProcess;
    bool hasPass;
    int experience;
    int level;
    int experienceOnCurrentLevel;
    int experienceForNextLevel;
    List<ClaimedReward> claimedLevelRewards = new List<ClaimedReward>();
    List<RecoveryEntrie> recoveringKitties = new List<RecoveryEntrie>();

    [JsonIgnore] public Action UpdatedSnacks;
    [JsonIgnore] public Action UpdatedJugOfMilk;
    [JsonIgnore] public Action UpdatedGlassOfMilk;
    [JsonIgnore] public Action UpdatedCommonCrystal;
    [JsonIgnore] public Action UpdatedUncommonCrystal;
    [JsonIgnore] public Action UpdatedRareCrystal;
    [JsonIgnore] public Action UpdatedEpicCrystal;
    [JsonIgnore] public Action UpdatedLegendaryCrystal;
    [JsonIgnore] public Action UpdatedGiftItem;
    [JsonIgnore] public Action UpdatedCraftingProcess;
    [JsonIgnore] public Action UpdatedClaimedLevels;
    [JsonIgnore] public Action UpdatedHasPass;
    [JsonIgnore] public Action UpdatedExp;
    [JsonIgnore] public Action UpdatedRecoveringKitties;

    public PlayerData()
    {
    }

    public float Snacks
    {
        get
        {
            return snacks;
        }
        set
        {
            snacks = value;
            UpdatedSnacks?.Invoke();
        }
    }

    public float JugOfMilk
    {
        get
        {
            return jugOfMilk;
        }
        set
        {
            jugOfMilk = value;
            UpdatedJugOfMilk?.Invoke();
        }
    }

    public float GlassOfMilk
    {
        get
        {
            return glassOfMilk;
        }
        set
        {
            glassOfMilk = value;
            UpdatedGlassOfMilk?.Invoke();
        }

    }

    public float CommonCrystal
    {
        get
        {
            return commonCrystal;
        }
        set
        {
            commonCrystal = value;
            UpdatedCommonCrystal?.Invoke();
        }
    }

    public float UncommonCrystal
    {
        get
        {
            return uncommonCrystal;
        }
        set
        {
            uncommonCrystal = value;
            UpdatedUncommonCrystal?.Invoke();
        }
    }

    public float RareCrystal
    {
        get
        {
            return rareCrystal;
        }
        set
        {
            rareCrystal = value;
            UpdatedRareCrystal?.Invoke();
        }
    }

    public float EpicCrystal
    {
        get
        {
            return epicCrystal;
        }

        set
        {
            epicCrystal = value;
            UpdatedEpicCrystal?.Invoke();
        }
    }

    public float LegendaryCrystal
    {
        get
        {
            return legendaryCristal;
        }
        set
        {
            legendaryCristal = value;
            UpdatedLegendaryCrystal?.Invoke();
        }
    }

    public float GiftItem
    {
        get
        {
            return giftItem;
        }
        set
        {
            giftItem = value;
            UpdatedGiftItem?.Invoke();
        }
    }

    public CraftingProcess CraftingProcess
    {
        get
        {
            return craftingProcess;
        }
        set
        {
            craftingProcess = value;
            UpdatedCraftingProcess?.Invoke();
        }
    }

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
        float _experience = Experience;
        int _level = 1;
        float _expForNextLevel = DataManager.Instance.GameData.LevelBaseExp;

        if (_experience<DataManager.Instance.GameData.LevelBaseExp)
        {
            experienceOnCurrentLevel = (int)_experience;
            _expForNextLevel = DataManager.Instance.GameData.LevelBaseExp;
        }
        else
        {
            while (_experience>=_expForNextLevel)
            {
                _level++;
                _experience -= _expForNextLevel;
                _expForNextLevel =_expForNextLevel+(_expForNextLevel * ((float)DataManager.Instance.GameData.LevelBaseScaler/100));
            }
        }

        experienceForNextLevel = (int)_expForNextLevel;
        experienceOnCurrentLevel = (int)_experience;
        level = _level;
    }

    [JsonIgnore] public int Level
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

    [JsonIgnore] public int ExperienceOnCurrentLevel => experienceOnCurrentLevel;
    [JsonIgnore] public int ExperienceForNextLevel => experienceForNextLevel;

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

    public List<RecoveryEntrie> RecoveringKitties
    {
        get
        {
            return recoveringKitties;
        }
    }

    public void AddRecoveringKittie(RecoveryEntrie _recoveryEntrie)
    {
        recoveringKitties.Add(_recoveryEntrie);
        UpdatedRecoveringKitties?.Invoke();
    }

    public void RemoveRecoveringKittie(string _imageUrl)
    {
        RecoveryEntrie _entry = null;

        foreach (var _recovery in recoveringKitties)
        {
            if (_recovery.KittyImageUrl==_imageUrl)
            {
                _entry = _recovery;
                break;
            }
        }

        if (_entry ==null)
        {
            return;
        }

        recoveringKitties.Remove(_entry);
        UpdatedRecoveringKitties?.Invoke();
    }

    [JsonIgnore]
    public float TotalCrystalsAmount => commonCrystal + uncommonCrystal + rareCrystal + epicCrystal + legendaryCristal;
}
