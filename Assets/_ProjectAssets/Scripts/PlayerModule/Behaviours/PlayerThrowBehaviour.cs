using Anura.ConfigurationModule.Managers;
using UnityEngine;

public class PlayerThrowBehaviour : MonoBehaviour
{
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject bullet;

    public void RegisterThrowCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Throw.started += _ => Launch();
    }

    private void Launch()
    {
        var obj = Instantiate(bullet, launchPoint.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(launchPoint.up* GetBulletSpeed(), ForceMode2D.Impulse);
        RoomStateManager.Instance.SetState(GameSceneStates.PROJECTILE_LAUNCHED);
    }

    private float GetBulletSpeed()
    {
        return ConfigurationManager.Instance.Config.GetBulletSpeed();
    }
}
