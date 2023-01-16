using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoSingleton<BotManager>
{
    public event Action<int> onHealthUpdated;

    [Header("Dependencies")]
    public Collider2D leftMapBound;
    public Collider2D rightMapBound;
    public GameObject bulletPrefab;  // Must contain rigidbody and collider

    public List<Collider2D> Enemy { get; private set; } = new List<Collider2D>();

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

    public void RegisterBotEnemy(PlayerComponent playerComponent)
    {
        Enemy.AddRange(playerComponent.gameObject.GetComponentsInChildren<Collider2D>());
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

    public void AreaDamage(Vector2 position, float area, int maxDamage, bool damageByDistance, bool hasPushForce, float pushForce, int bulletCount)
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
