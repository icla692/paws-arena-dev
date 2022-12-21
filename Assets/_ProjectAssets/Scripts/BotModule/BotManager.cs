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
        AreaEffectsManager.Instance.OnAreaDamage += AreaDamage;
        maxHP = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();
        SetBotHealth(maxHP);
    }

    private void OnDestroy()
    {
        AreaEffectsManager.Instance.OnAreaDamage -= AreaDamage;
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

    public void AreaDamage(Vector2 position, float area, int maxDamage, bool damageByDistance, bool hasPushForce, float pushForce)
    {
        Vector3 playerPos = currentBot.transform.position;
        float dmgDistance = Vector3.Distance(playerPos, position);
        if (dmgDistance > area) return;

        float damagePercentage = (area - dmgDistance) / area;
        int dmgToBeDone = damageByDistance ? (int)Math.Floor(damagePercentage * maxDamage) : maxDamage;
        Debug.Log($"Got damage {dmgToBeDone} / {botHP}");
        SetBotHealth(botHP - dmgToBeDone);

        if (hasPushForce)
        {
            Vector2 direction = new Vector2(playerPos.x, playerPos.y) - position;
            Push(damagePercentage * pushForce, direction);
        }
    }

        private void Push(float force, Vector2 direction)
        {
            currentBot.GetComponent<Rigidbody2D>().AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
}
