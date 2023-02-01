using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoSingleton<BotManager>
{
    public event Action<int> onHealthUpdated;

    [HideInInspector]
    public PlayerDataCustomView botUI;

    private BotPlayerComponent currentBot;
    private int maxHP;
    private int botHP;

    public void RegisterBot(BotPlayerComponent botComponent)
    {
        currentBot = botComponent;
        currentBot.GetComponent<BasePlayerComponent>().onDamageTaken += AreaDamage;
        maxHP = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();
        PlayerManager.Instance.otherPlayerTransform = botComponent.transform;
        SetBotHealth(maxHP);
    }

    private void OnDestroy()
    {
        currentBot.GetComponent<BasePlayerComponent>().onDamageTaken -= AreaDamage;
    }

    private void SetBotHealth(int value)
    {
        value = Math.Max(0, value);
        value = Math.Min(maxHP, value);

        botHP = value;

        botUI.SetHealth(botHP);
        PlayerManager.Instance.otherPlayerHealth = botHP;
        onHealthUpdated?.Invoke(botHP);
    }

    public void AreaDamage(int damage)
    {
        Debug.Log($"Got damage {damage} / {botHP}");
        SetBotHealth(botHP - damage);
    }
}
