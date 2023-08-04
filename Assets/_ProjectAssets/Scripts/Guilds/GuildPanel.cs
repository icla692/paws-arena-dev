using System;
using UnityEngine;

public class GuildPanel : GuildPanelBase
{
    [SerializeField] private NoGuildPanel noGuildPanel;
    [SerializeField] private GuildBattlePanel guildBattlePanel;

    [SerializeField] private GameObject noGuildMessage;

    public override void Setup()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        GuildLeftPanel.OnShowMyGuild += ShowMyGuild;
        GuildLeftPanel.OnShowGuildBattle += ShowGuildBattle;
        GuildLeftPanel.OnClose += Close;
    }

    private void OnDisable()
    {
        GuildLeftPanel.OnShowMyGuild -= ShowMyGuild;
        GuildLeftPanel.OnShowGuildBattle -= ShowGuildBattle;
        GuildLeftPanel.OnClose -= Close;
    }

    private void ShowMyGuild()
    {
        if (!DataManager.Instance.PlayerData.IsInGuild)
        {
            SwitchToPanel(noGuildPanel);
            return;
        }
        else
        {
            //todo show guild panel
        }
    }

    private void ShowGuildBattle()
    {
        if (!DataManager.Instance.PlayerData.IsInGuild)
        {
            noGuildMessage.SetActive(true);
            return;
        }
        
        SwitchToPanel(guildBattlePanel);
    }

    private void SwitchToPanel(GuildPanelBase _panel)
    {
        noGuildPanel.Close();
        
        _panel.Setup();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
