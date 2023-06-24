using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerData PlayerData { get; private set; }
    public GameData GameData { get; private set; }

    const string SNACKS = "Snacks";
    const string JUG_OF_MILK = "JugOfMilk";
    const string GLASS_OF_MILK = "GlassOfMilk";
    const string COMMON_CRISTAL = "Crystals/CommonCrystal";
    const string UNCOMMON_CRISTAL = "Crystals/UncommonCrystal";
    const string RARE_CRISTAL = "Crystals/RareCrystal";
    const string EPIC_CRISTAL = "Crystals/EpicCrystal";
    const string LEGENDARY_CRISTAL = "Crystals/LegendaryCrystal";
    const string CRAFTING_PROCESS = "CraftingProcess";
    const string EXPERIENCE = "Experience";
    const string CLAIMED_LEVELS = "ClaimedLevelRewards";
    const string HAS_PASS = "HasPass";
    const string RECOVERING_KITTIES = "RecoveringKitties";
    private const string OWNED_EQUIPTABLES = "OwnedEquiptables";
    private const string SEASON_NUMBER = "SeasonNumber";

    bool hasSubscribed = false;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerData(string _data)
    {
        PlayerData = JsonConvert.DeserializeObject<PlayerData>(_data);
        if (PlayerData.SeasonNumber!=GameData.SeasonNumber)
        {
            PlayerData.SeasonNumber = GameData.SeasonNumber;
            PlayerData.Experience = 0;
            PlayerData.ClaimedLevelRewards.Clear();
            PlayerData.HasPass = false;
            SaveClaimedLevels();
            SaveExp();
            SaveHasPass();
            SaveSeasonNumber();
        }
        
    }

    public void CreatePlayerDataEmpty()
    {
        PlayerData = new PlayerData();
    }

    public void SetGameData(string _data)
    {
        GameData = JsonConvert.DeserializeObject<GameData>(_data);
    }

    public void SubscribeHandlers()
    {
        hasSubscribed = true;
        PlayerData.UpdatedSnacks += SaveSnacks;
        PlayerData.UpdatedJugOfMilk += SaveJugOfMilk;
        PlayerData.UpdatedGlassOfMilk += SaveGlassOfMilk;
        PlayerData.Crystals.UpdatedCommonCrystal += SaveCommonCristal;
        PlayerData.Crystals.UpdatedUncommonCrystal += SaveUncommonCristal;
        PlayerData.Crystals.UpdatedRareCrystal += SaveRareCristal;
        PlayerData.Crystals.UpdatedEpicCrystal += SaveEpicCristal;
        PlayerData.Crystals.UpdatedLegendaryCrystal += SaveLegendaryCristal;
        PlayerData.UpdatedCraftingProcess += SaveCraftingProcess;
        PlayerData.UpdatedClaimedLevels += SaveClaimedLevels;
        PlayerData.UpdatedHasPass += SaveHasPass;
        PlayerData.UpdatedExp += SaveExp;
        PlayerData.UpdatedRecoveringKitties += SaveRecoveringKittes;
        PlayerData.UpdatedEquiptables += SaveEquiptables;
        PlayerData.UpdatedSeasonNumber += SaveSeasonNumber;
    }

    private void OnDestroy()
    {
        if (!hasSubscribed)
        {
            return;
        }
        PlayerData.UpdatedSnacks -= SaveSnacks;
        PlayerData.UpdatedJugOfMilk -= SaveJugOfMilk;
        PlayerData.UpdatedGlassOfMilk -= SaveGlassOfMilk;
        PlayerData.Crystals.UpdatedCommonCrystal -= SaveCommonCristal;
        PlayerData.Crystals.UpdatedUncommonCrystal -= SaveUncommonCristal;
        PlayerData.Crystals.UpdatedRareCrystal -= SaveRareCristal;
        PlayerData.Crystals.UpdatedEpicCrystal -= SaveEpicCristal;
        PlayerData.Crystals.UpdatedLegendaryCrystal -= SaveLegendaryCristal;
        PlayerData.UpdatedCraftingProcess -= SaveCraftingProcess;
        PlayerData.UpdatedClaimedLevels -= SaveClaimedLevels;
        PlayerData.UpdatedHasPass -= SaveHasPass;
        PlayerData.UpdatedExp -= SaveExp;
        PlayerData.UpdatedRecoveringKitties -= SaveRecoveringKittes;
        PlayerData.UpdatedEquiptables -= SaveEquiptables;
        PlayerData.UpdatedSeasonNumber -= SaveSeasonNumber;
    }

    void SaveSnacks()
    {
        FirebaseManager.Instance.SaveValue(SNACKS,PlayerData.Snacks);
    }

    void SaveJugOfMilk()
    {
        FirebaseManager.Instance.SaveValue(JUG_OF_MILK, PlayerData.JugOfMilk);
    }

    void SaveGlassOfMilk()
    {
        FirebaseManager.Instance.SaveValue(GLASS_OF_MILK, PlayerData.GlassOfMilk);
    }

    void SaveCommonCristal()
    {
        FirebaseManager.Instance.SaveValue(COMMON_CRISTAL, PlayerData.Crystals.CommonCrystal);
    }

    void SaveUncommonCristal()
    {
        FirebaseManager.Instance.SaveValue(UNCOMMON_CRISTAL, PlayerData.Crystals.UncommonCrystal);
    }

    void SaveRareCristal()
    {
        FirebaseManager.Instance.SaveValue(RARE_CRISTAL, PlayerData.Crystals.RareCrystal);
    }

    void SaveEpicCristal()
    {
        FirebaseManager.Instance.SaveValue(EPIC_CRISTAL, PlayerData.Crystals.EpicCrystal);
    }

    void SaveLegendaryCristal()
    {
        FirebaseManager.Instance.SaveValue(LEGENDARY_CRISTAL, PlayerData.Crystals.LegendaryCrystal);
    }

    void SaveCraftingProcess()
    {
        FirebaseManager.Instance.SaveValue(CRAFTING_PROCESS, JsonConvert.SerializeObject(PlayerData.CraftingProcess));
    }

    void SaveClaimedLevels()
    {
        FirebaseManager.Instance.SaveValue(CLAIMED_LEVELS, JsonConvert.SerializeObject(PlayerData.ClaimedLevelRewards));
    }

    void SaveHasPass()
    {
        FirebaseManager.Instance.SaveValue(HAS_PASS, JsonConvert.SerializeObject(PlayerData.HasPass));
    }

    void SaveExp()
    {
        FirebaseManager.Instance.SaveValue(EXPERIENCE,PlayerData.Experience);
    }

    void SaveRecoveringKittes()
    {
        FirebaseManager.Instance.SaveValue(RECOVERING_KITTIES, JsonConvert.SerializeObject(PlayerData.RecoveringKitties));
    }

    void SaveEquiptables()
    {
        FirebaseManager.Instance.SaveValue(OWNED_EQUIPTABLES, JsonConvert.SerializeObject(PlayerData.OwnedEquiptables));
    }

    void SaveSeasonNumber()
    {
        FirebaseManager.Instance.SaveValue(SEASON_NUMBER,PlayerData.SeasonNumber);
    }
}
