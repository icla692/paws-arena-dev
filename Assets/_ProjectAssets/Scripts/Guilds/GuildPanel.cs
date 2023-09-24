using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuildPanel : GuildPanelBase
{
    [SerializeField] private NoGuildPanel noGuildPanel;
    [SerializeField] private GuildBattlePanel guildBattlePanel;
    [SerializeField] private CreateGuildPanel createGuildPanel;
    [SerializeField] private JoinGuildPanel joinGuildPanel;
    [SerializeField] private HasGuildPanel hasGuildPanel;
    [SerializeField] private GuildTopGuildsPanel topGuildPanel;
    [SerializeField] private SearchGuilds searchGuilds;
    [SerializeField] private LevelRewardPanel rewardPrefab;
    [SerializeField] private EquipmentsConfig equipmentsConfig;

    private LevelRewardPanel rewardPanel;
    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        GuildLeftPanel.OnShowMyGuild += ShowMyGuild;
        GuildLeftPanel.OnShowGuildBattle += ShowGuildBattle;
        GuildLeftPanel.OnClose += Close;
        GuildLeftPanel.OnShowTopGuilds += ShowTopGuilds;
        GuildLeftPanel.OnShowSearchGuilds += OnShowSearchGuilds;
        NoGuildPanel.OnShowCreateGuild += ShowCreateGuild;
        NoGuildPanel.OnShowJoinGuild += ShowJoinGuild;
        DataManager.Instance.PlayerData.UpdatedGuild += ShowMyGuild;
        JoinGuildPanel.OnJoinedGuild += ShowMyGuild;
        SearchGuilds.OnJoinedGuild += ShowMyGuild;

        ShowRewards();
    }

    private void OnDisable()
    {
        GuildLeftPanel.OnShowMyGuild -= ShowMyGuild;
        GuildLeftPanel.OnShowGuildBattle -= ShowGuildBattle;
        GuildLeftPanel.OnClose -= Close;
        GuildLeftPanel.OnShowTopGuilds -= ShowTopGuilds;
        GuildLeftPanel.OnShowSearchGuilds -= OnShowSearchGuilds;
        NoGuildPanel.OnShowCreateGuild -= ShowCreateGuild;
        NoGuildPanel.OnShowJoinGuild -= ShowJoinGuild;
        DataManager.Instance.PlayerData.UpdatedGuild -= ShowMyGuild;
        JoinGuildPanel.OnJoinedGuild -= ShowMyGuild;
        SearchGuilds.OnJoinedGuild += ShowMyGuild;
    }

    private void ShowMyGuild()
    {
        if (!DataManager.Instance.PlayerData.IsInGuild)
        {
            SwitchToPanel(noGuildPanel);
        }
        else
        {
            SwitchToPanel(hasGuildPanel);
        }
    }

    private void ShowTopGuilds()
    {
        SwitchToPanel(topGuildPanel);
    }

    private void OnShowSearchGuilds()
    {
        SwitchToPanel(searchGuilds);
    }

    private void ShowGuildBattle()
    {
        SwitchToPanel(guildBattlePanel);
    }

    private void ShowCreateGuild()
    {
        if (DataManager.Instance.PlayerData.IsInGuild)
        {
            return;
        }

        SwitchToPanel(createGuildPanel);
    }

    private void ShowJoinGuild()
    {
        if (DataManager.Instance.PlayerData.IsInGuild)
        {
            return;
        }

        SwitchToPanel(joinGuildPanel);
    }

    private void SwitchToPanel(GuildPanelBase _panel)
    {
        noGuildPanel.Close();
        guildBattlePanel.Close();
        createGuildPanel.Close();
        joinGuildPanel.Close();
        hasGuildPanel.Close();
        topGuildPanel.Close();
        searchGuilds.Close();

        _panel.Setup();
    }

    private void ShowRewards()
    {
        if (DataManager.Instance.PlayerData.GuildBattleReward.Count==0)
        {
            return;
        }

        int _counter = 0;
        List<GuildBattleReward> _rewardsToShow = DataManager.Instance.PlayerData.GuildBattleReward.ToList();

        foreach (var _battleReward in DataManager.Instance.PlayerData.GuildBattleReward)
        {
            GuildRewardSO _rewardSO = GuildRewardSO.Get(_battleReward.TypeId);
            switch (_rewardSO.Type)
            {
                case ItemType.Common:
                    DataManager.Instance.PlayerData.Crystals.CommonCrystal += _battleReward.Amount;
                    break;
                case ItemType.Uncommon:
                    DataManager.Instance.PlayerData.Crystals.UncommonCrystal += _battleReward.Amount;
                    break;
                case ItemType.Rare:
                    DataManager.Instance.PlayerData.Crystals.RareCrystal += _battleReward.Amount;
                    break;
                case ItemType.Epic:
                    DataManager.Instance.PlayerData.Crystals.EpicCrystal += _battleReward.Amount;
                    break;
                case ItemType.Legendary:
                    DataManager.Instance.PlayerData.Crystals.LegendaryCrystal += _battleReward.Amount;
                    break;
                case ItemType.Item:
                    DataManager.Instance.PlayerData.AddOwnedEquipment(_battleReward.Amount);
                    break;
                case ItemType.GlassOfMilk:
                    DataManager.Instance.PlayerData.GlassOfMilk += _battleReward.Amount;
                    break;
                case ItemType.JugOfMilk:
                    DataManager.Instance.PlayerData.JugOfMilk += _battleReward.Amount;
                    break;
                case ItemType.Cookie:
                    DataManager.Instance.PlayerData.Snacks += _battleReward.Amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        DataManager.Instance.PlayerData.ClearBattleRewards();

        LevelRewardPanel.OnClosePressed += ShowNextReward;
        ShowNextReward();

        void ShowNextReward()
        {
            if (_counter>= _rewardsToShow.Count)
            {
                LevelRewardPanel.OnClosePressed -= ShowNextReward;
                return;
            }

            if (rewardPanel != null)
            {
                Destroy(rewardPanel);
            }

            GuildRewardSO _rewardSO = GuildRewardSO.Get(_rewardsToShow[_counter].TypeId);
            Sprite _sprite = default;
            switch (_rewardSO.Type)
            {
                case ItemType.Item:
                    _sprite = equipmentsConfig.GetEquipmentData(_rewardsToShow[_counter].Amount).Thumbnail;
                    break;
                default:
                    _sprite = _rewardSO.Sprite;
                    break;
            }
            rewardPanel = Instantiate(rewardPrefab, transform);
            rewardPanel.Setup(_rewardSO, _sprite);
            _counter++;
        }
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
