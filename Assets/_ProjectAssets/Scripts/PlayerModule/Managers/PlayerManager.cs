using Anura.Templates.MonoSingleton;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private PlayerComponent player;

    public PlayerComponent GetPlayer()
    {
        return player;
    }
}
