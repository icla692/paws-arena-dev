using Anura.Templates.MonoSingleton;
using UnityEngine;
using System;
using Anura.ConfigurationModule.Managers;
using Photon.Pun;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public event Action<int> onHealthUpdated;
    public event Action<int> onDamageTaken;

    [SerializeField]
    private GameObject player1SpawnSquare;
    [SerializeField]
    private GameObject player2SpawnSquare;

    [HideInInspector]
    public PlayerComponent myPlayer;
    [HideInInspector]
    public int myPlayerHealth;
    [HideInInspector]
    public int otherPlayerHealth = int.MaxValue;
    [HideInInspector]
    public Transform otherPlayerTransform;
    [HideInInspector]
    public PlayerComponent OtherPlayerComponent;
    private int maxHP;

    public void RegisterMyPlayer(PlayerComponent playerComponent)
    {
        myPlayer = playerComponent;
        myPlayer.GetComponent<BasePlayerComponent>().onDamageTaken += OnDamageTaken;

        maxHP = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();
        SetMyPlayerHealth(maxHP);
    }

    private void OnDestroy()
    {
        myPlayer.GetComponent<BasePlayerComponent>().onDamageTaken -= OnDamageTaken;
    }

    [ContextMenu("Test_Take50Damage")]
    public void TEST_TakeDamage()
    {
        OnDamageTaken(50);
    }

    private void OnDamageTaken(int damage)
    {
        Debug.Log($"Got damage {myPlayerHealth} - {damage} = {myPlayerHealth - damage}");
        SetMyPlayerHealth(myPlayerHealth - damage);
    }

    public GameResolveState GetWinnerByDeath()
    {
        if (myPlayerHealth > 0 && otherPlayerHealth > 0)
        {
            return GameResolveState.NO_WIN;
        }
        else if (myPlayerHealth <= 0 && otherPlayerHealth <= 0)
        {
            return GameResolveState.DRAW;
        }
        else if ((myPlayerHealth > 0 && PhotonNetwork.LocalPlayer.IsMasterClient) || (otherPlayerHealth > 0 && !PhotonNetwork.LocalPlayer.IsMasterClient))
        {
            return GameResolveState.PLAYER_1_WIN;
        }
        else return GameResolveState.PLAYER_2_WIN;
    }

    public GameResolveState GetWinnerByHealth()
    {
        if (myPlayerHealth > otherPlayerHealth)
        {
            return GameResolveState.PLAYER_1_WIN;
        }
        else if (myPlayerHealth < otherPlayerHealth)
        {
            return GameResolveState.PLAYER_2_WIN;
        }
        else return GameResolveState.DRAW;
    }

    public GameResolveState GetWinnerByLoserIndex(int idx)
    {
        if (idx == 0)
        {
            return GameResolveState.PLAYER_2_WIN;
        }
        else return GameResolveState.PLAYER_1_WIN;
    }

    private void SetMyPlayerHealth(int value)
    {
        value = Math.Max(0, value);
        value = Math.Min(maxHP, value);

        myPlayerHealth = value;
        onHealthUpdated?.Invoke(myPlayerHealth);
    }

    public void Heal(int healValue)
    {
        Debug.Log($"Healing: {healValue}. New HP: {myPlayerHealth + healValue}");
        SetMyPlayerHealth(myPlayerHealth + healValue);
    }


    public void DirectDamage(int damage)
    {
        SetMyPlayerHealth(myPlayerHealth - damage);
    }

    public Vector2 GetPlayer1SpawnPos()
    {
        return GetRandomPosInSquare(player1SpawnSquare);
    }

    public Vector2 GetPlayer2SpawnPos()
    {
        return GetRandomPosInSquare(player2SpawnSquare);
    }

    private Vector2 GetRandomPosInSquare(GameObject square)
    {
        Vector2 minRange = square.transform.position - square.transform.lossyScale / 2.0f;
        Vector2 maxRange = square.transform.position + square.transform.lossyScale / 2.0f;
        float xPos = UnityEngine.Random.Range(minRange.x, maxRange.x);
        float yPos = UnityEngine.Random.Range(minRange.y, maxRange.y);
        return new Vector2(xPos, yPos);
    }
}
