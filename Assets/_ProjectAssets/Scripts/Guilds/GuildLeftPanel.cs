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
    
    [SerializeField] private GameObject noGuildMessage;


    private void OnEnable()
    {
        DataManager.Instance.PlayerData.UpdatedGuild += ShowFlag;
        
        closeButton.onClick.AddListener(Close);
        myGuildButton.onClick.AddListener(ShowMyGuild);
        guildBattleButton.onClick.AddListener(ShowGuildBattle);
        topGuilds.onClick.AddListener(ShowTopGuilds);

        Invoke(nameof(ShowMyGuild),0.1f);
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

        flagImage.sprite = GuildSO.Get(DataManager.Instance.PlayerData.Guild.FlagId).Flag;
        flagImage.gameObject.SetActive(true);
    }

    private void ShowMyGuild()
    {
        ShowButtonAsSelected(myGuildButton);
        ShowFlag();
        OnShowMyGuild?.Invoke();
    }

    private void ShowGuildBattle()
    {
        if (!DataManager.Instance.PlayerData.IsInGuild)
        {
            noGuildMessage.SetActive(true);
            return;
        }
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
