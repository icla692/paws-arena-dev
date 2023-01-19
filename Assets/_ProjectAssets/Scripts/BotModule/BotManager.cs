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

    [Tooltip("1 travel step = this * [width of character]")]
    public float stepLength = 0.9f;

    [Tooltip("The maximum number of steps that the bot will consider travelling in one turn.\n" +
        "Increasing this value will result in a prolonged thinking time.")]
    public int maxTravelSteps = 5;

    [Tooltip("This parameter affects how the bot decides which weapon to use in a given location.\n" +
        "[Fast Mode]: Will use the first good weapon found. Better for performance.\n" +
        "[Complete Mode]: Slower but will evaluate all weapons for the location before making a decision.")]
    public LocationWeaponEvaluationMode locationWeaponEvaluationMode = LocationWeaponEvaluationMode.Complete;

    [Header("Weights for evaluating a location. Set to zero to turn off.")]

    [Tooltip("How far from the enemy is the best shot achievable from the location.")]
    public int weightBestShotDistance = 100;

    [Tooltip("Is a direct hit possible from the location.")]
    public int weightDirectHit = 75;

    [Tooltip("How much to prioritize walking away instead of towards the enemy.")]
    public int weightDirectionImportance = 20;

    [Tooltip("How much to prioritize staying on a high altitude.")]
    public int weightHeightImportance = 20;

    [Tooltip("How much to prioritize moving away from a location if damage was taken.")]
    public int weightMoveAwayIfShot = 40;

    [Header("Performance")]

    [Tooltip("How many simulations the bot is allowed to perform per frame. " +
        "Reducing this will result in a single frame being calculated faster, but more frames will be required.")]
    public int maxSimulationsPerFrame = 1000;

    [Tooltip("The maximum amount of time in seconds that the bot is allowed to spend on thinking. " +
        "If this is exceeded, the bot will abort any remaining calculations and work with what it has.")]
    public float maxThinkingTime = 5;

    [Header("Advanced. Use with caution.")]

    [Tooltip("When moving to a location, the bot stops when the remaining distance is this value.")]
    public float movementArrivalDistance = 0.2f;

    [Tooltip("How many frames to wait before checking whether a jump or change of direction is required when moving.")]
    public int movementUpdateFrames = 10;

    [Tooltip("How many seconds to wait before deciding it is stuck and launching a movement reevaluation.")]
    public float movementStuckTime = 3;

    [Tooltip("[0, 1] range. Consider only location scores within this margin of the top choice.")]
    public float locationDecidingMargin = 0.05f;

    [Tooltip("Pairs with the locationDecidingMargin parameter, limits the max number of choices to consider.")]
    public int locationDecidingConsiderTopChoices = 10;

    [Tooltip("The maximum time that a projectile is expected to fly before hitting anything. " +
        "Reducing this would increase performance, but a too small value can ruin the bot's aiming ability.")]
    public float simulationMaxTime = 5;

    [Tooltip("Increasing this improves performance but reduces the simulation's accuracy.")]
    public float simulationInterval = 0.05f;

    [Tooltip("The minimum shooting angle considered in simulations.")]
    public float simulationAngleMin = 0;

    [Tooltip("The maximum shooting angle considered in simulations.")]
    public float simulationAngleMax = 180;

    [Tooltip("In simulations, angles are first attempted in increments of 2^this, then 2^(this-1), ...")]
    public int simulationAngleIncrement = 3;

    [Tooltip("The minimum shooting power considered in simulations.")]
    public float simulationPowerMin = 0.1f;

    [Tooltip("The maximum shooting power considered in simulations.")]
    public float simulationPowerMax = 1.0f;

    [Tooltip("In simulations, power is incremented with this value.")]
    public float simulationPowerIncrement = 0.05f;

    [Tooltip("")]

    [Header("Debug")]
    public bool debugBotAI = false;

    public int WeightsTotal =>
        weightBestShotDistance +
        weightDirectHit +
        weightDirectionImportance +
        weightHeightImportance +
        weightMoveAwayIfShot;
}

public class BotManager : MonoSingleton<BotManager>
{
    public event Action<int> onHealthUpdated;

    public BotConfiguration configuration;

    [Header("Dependencies")]
    [Header("These map bounds are also used to reference the height (e.g. lowest and highest Y points) of the map, so make sure they are the correct height.")]
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
        currentBot.GetComponent<BasePlayerComponent>().onDamageTaken += AreaDamage;
        maxHP = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();
        SetBotHealth(maxHP);
    }

    public void RegisterBotEnemy(PlayerComponent playerComponent)
    {
        Enemy.AddRange(playerComponent.gameObject.GetComponentsInChildren<Collider2D>());
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

    private void Push(float force, Vector2 direction)
    {
        currentBot.GetComponent<Rigidbody2D>().AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    public int GetHP()
    {
        return botHP;
    }
}
