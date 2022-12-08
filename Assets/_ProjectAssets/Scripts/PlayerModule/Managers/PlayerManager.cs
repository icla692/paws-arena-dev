using Anura.Templates.MonoSingleton;
using UnityEngine;
using System;
using Anura.ConfigurationModule.Managers;
using Photon.Pun;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public event Action<int> onHealthUpdated;

    [SerializeField]
    private GameObject player1SpawnSquare;
    [SerializeField]
    private GameObject player2SpawnSquare;

    private PlayerComponent myPlayer;
    private int myPlayerHealth;
    [HideInInspector]
    public int otherPlayerHealth = int.MaxValue;

    public void RegisterMyPlayer(PlayerComponent playerComponent)
    {
        myPlayer = playerComponent;
        AreaEffectsManager.Instance.OnAreaDamage += AreaDamage;
        SetMyPlayerHealth(ConfigurationManager.Instance.Config.GetPlayerTotalHealth());
    }

    private void OnDestroy()
    {
        AreaEffectsManager.Instance.OnAreaDamage -= AreaDamage;
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
        value = Math.Min(100, value);

        myPlayerHealth = value;
        onHealthUpdated?.Invoke(myPlayerHealth);
    }

    public void Heal(int healValue)
    {
        Debug.Log($"Healing: {healValue}. New HP: {myPlayerHealth + healValue}");
        SetMyPlayerHealth(myPlayerHealth + healValue);
    }

    public void AreaDamage(Vector2 position, float area, int maxDamage, bool damageByDistance, bool hasPushForce, float pushForce)
    {
        Vector3 playerPos = myPlayer.transform.position;
        float dmgDistance = Vector3.Distance(playerPos, position);
        if (dmgDistance > area) return;

        float damagePercentage = (area - dmgDistance) / area;
        int dmgToBeDone = damageByDistance ? (int)Math.Floor(damagePercentage * maxDamage) : maxDamage;
        Debug.Log($"Got damage {dmgToBeDone} / {myPlayerHealth}");
        SetMyPlayerHealth(myPlayerHealth - dmgToBeDone);

        if (hasPushForce)
        {
            Vector2 direction = new Vector2(playerPos.x, playerPos.y) - position;
            PushPlayer(damagePercentage * pushForce, direction);
        }
    }

    public void DirectDamage(int damage)
    {
        SetMyPlayerHealth(myPlayerHealth - damage);
    }

    private void PushPlayer(float force, Vector2 direction)
    {
        myPlayer.GetComponent<Rigidbody2D>().AddForce(direction.normalized * force, ForceMode2D.Impulse);
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
