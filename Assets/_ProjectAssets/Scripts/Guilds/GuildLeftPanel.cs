using System;
using UnityEngine;
using UnityEngine.UI;

public class GuildLeftPanel : MonoBehaviour
{
    public static Action OnShowMyGuild;
    public static Action OnShowGuildBattle;
    public static Action OnClose;
    
    [SerializeField] private Image flagImage;
    [SerializeField] private Button myGuildButton;
    [SerializeField] private Button guildBattleButton;
    [SerializeField] private Button topGuilds;
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private Sprite selectedButton;
    [SerializeField] private Sprite notSelectedButton;
    
    

    private void OnEnable()
    {
        DataManager.Instance.PlayerData.UpdatedGuild += ShowFlag;
        
        closeButton.onClick.AddListener(Close);
        myGuildButton.onClick.AddListener(ShowMyGuild);
        guildBattleButton.onClick.AddListener(ShowGuildBattle);
        topGuilds.onClick.AddListener(ShowTopGuilds);

        ShowMyGuild();
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.UpdatedGuild -= ShowFlag;
        
        closeButton.onClick.RemoveListener(Close);
        myGuildButton.onClick.RemoveListener(ShowMyGuild);
        guildBattleButton.onClick.RemoveListener(ShowGuildBattle);
        topGuilds.onClick.RemoveListener(ShowTopGuilds);
    }

    private void Close()
    {
        OnClose?.Invoke();
    }

    private void ShowFlag()
    {
        if (!DataManager.Instance.PlayerData.IsInGuild)
        {
            flagImage.gameObject.SetActive(false);
            return;
        }
    }

    private void ShowMyGuild()
    {
        ShowButtonAsSelected(myGuildButton);
        ShowFlag();
        OnShowMyGuild?.Invoke();
    }

    private void ShowGuildBattle()
    {
        ShowButtonAsSelected(guildBattleButton);
        OnShowGuildBattle?.Invoke();
    }

    private void ShowButtonAsSelected(Button _button)
    {
        myGuildButton.image.sprite = notSelectedButton;
        guildBattleButton.image.sprite = notSelectedButton;

        _button.image.sprite = selectedButton;
    }

    private void ShowTopGuilds()
    {
        Debug.Log("Show top guilds");
    }
}
