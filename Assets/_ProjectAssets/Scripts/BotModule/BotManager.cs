using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BotAIAim;

[Serializable]
public class BotConfiguration
{
    public enum LocationWeaponEvaluationMode
    {
        Fast,
        Complete
    }

    [Header("General")]

    [Tooltip("1 travel step = 0.9 * (width of character)")]
    public float maxTravelSteps = 5;

    [Tooltip("This parameter affects how the bot decides which weapon to use in a given location.\n" +
        "[Fast Mode]: Will use the first good weapon found. Better for performance.\n" +
        "[Complete Mode]: Slower but will evaluate all weapons for the location before making a decision.")]
    public LocationWeaponEvaluationMode locationWeaponEvaluationMode = LocationWeaponEvaluationMode.Complete;

    [Header("Weights for evaluating a location. Set to zero to turn off.")]

    [Tooltip("How far from the enemy is the best shot achievable from the location.")]
    public int weightBestShotDistance = 25;

    [Tooltip("Is a direct hit possible from the location.")]
    public int weightDirectHit = 75;

    [Tooltip("How much to prioritize walking away instead of towards the enemy.")]
    public int weightDirectionImportance = 25;

    [Tooltip("How much to prioritize staying on a high altitude.")]
    public int weightHeightImportance = 25;

    [Header("Performance")]

    [Tooltip("How many simulations the bot is allowed to perform per frame. " +
        "Reducing this will result in a single frame being calculated faster, but more frames will be required.")]
    public int maxSimulationsPerFrame = 1000;

    [Tooltip("The maximum amount of time in seconds that the bot is allowed to spend on thinking. " +
        "If this is exceeded, the bot will abort any remaining calculations and work with what it has.")]
    public float maxThinkingTime = 5;

    [Header("Debug")]
    public bool debugBotAI = false;

    public int WeightsTotal =>
        weightBestShotDistance +
        weightDirectHit +
        weightDirectionImportance +
        weightHeightImportance;
}

public class BotManager : MonoSingleton<BotManager>
{
    public event Action<int> onHealthUpdated;

    public BotConfiguration configuration;

    [Header("Dependencies")]
    public Collider2D leftMapBound;  
    public Collider2D rightMapBound;
    public List<WeaponData> weaponsData;

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
