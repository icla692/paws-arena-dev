using Anura.Templates.MonoSingleton;
using UnityEngine;
using NaughtyAttributes;
using System;
using Anura.ConfigurationModule.ScriptableObjects;
using Anura.ConfigurationModule.Managers;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public event Action<int> onHealthUpdated;
    [SerializeField] private PlayerComponent player;

    private PlayerComponent myPlayer;
    private int myPlayerHealth;

    public PlayerComponent GetPlayer()
    {
        return player;
    }

    public void RegisterMyPlayer(PlayerComponent playerComponent)
    {
        myPlayer = playerComponent;
        SetMyPlayerHealth(ConfigurationManager.Instance.Config.GetPlayerTotalHealth());
    }

    private void SetMyPlayerHealth(int value)
    {
        myPlayerHealth = value;
        onHealthUpdated?.Invoke(myPlayerHealth);
    }

    [ContextMenu("Test damage")]
    public void TestDamage()
    {
        SetMyPlayerHealth(myPlayerHealth - 10);
    }
}
