using System;
using UnityEngine;

public class GuildPanel : GuildPanelBase
{
    [SerializeField] private NoGuildPanel noGuildPanel;
    [SerializeField] private GuildBattlePanel guildBattlePanel;
    [SerializeField] private CreateGuildPanel createGuildPanel;
    [SerializeField] private JoinGuildPanel joinGuildPanel;
    [SerializeField] private HasGuildPanel hasGuildPanel;
    
    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        GuildLeftPanel.OnShowMyGuild += ShowMyGuild;
        GuildLeftPanel.OnShowGuildBattle += ShowGuildBattle;
        GuildLeftPanel.OnClose += Close;
        NoGuildPanel.OnShowCreateGuild += ShowCreateGuild;
        NoGuildPanel.OnShowJoinGuild += ShowJoinGuild;
        DataManager.Instance.PlayerData.UpdatedGuild += ShowMyGuild;
    }

    private void OnDisable()
    {
        GuildLeftPanel.OnShowMyGuild -= ShowMyGuild;
        GuildLeftPanel.OnShowGuildBattle -= ShowGuildBattle;
        GuildLeftPanel.OnClose -= Close;
        NoGuildPanel.OnShowCreateGuild -= ShowCreateGuild;
        NoGuildPanel.OnShowJoinGuild -= ShowJoinGuild;
        DataManager.Instance.PlayerData.UpdatedGuild -= ShowMyGuild;
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

        _panel.Setup();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
