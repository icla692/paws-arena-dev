using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Extensions;
using Photon.Pun;
using System;
using UnityEngine;

public class PlayerThrowBehaviour : MonoBehaviour
{
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject bullet;

    [SerializeField] private LineRenderer lineIndicatorSpeed;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioClip chargingSfx;

    private Config config => ConfigurationManager.Instance.Config;

    private bool isStarted;

    private float timeElapsed;
    private Vector3 valueToLerp;

    private void OnEnable()
    {
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        if(state is ProjectileLaunchedState && isStarted)
        {
            ThrowCompleted();
        }
    }

    private void Update()
    {
        if (!isStarted)
            return;

        timeElapsed += Time.deltaTime;

        if (timeElapsed > config.GetPressTimer().x)
        {
            valueToLerp = valueToLerp.WithX(Mathf.Lerp(0, 3, timeElapsed / config.GetPressTimer().y));
            lineIndicatorSpeed.SetPosition(1, valueToLerp);
        }
        

        if(timeElapsed > config.GetValidIndicatorTime())
        {
            isStarted = false;
            lineIndicatorSpeed.SetPosition(1, Vector3.zero);
        }
    }

    public void RegisterThrowCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Throw.started += _ => ThrowStarted();
        playerActions.Throw.canceled += _ => ThrowCompleted();
    }

    private void ThrowStarted()
    {
        timeElapsed = 0;
        isStarted = true;

        particles.Play();
        SFXManager.Instance.PlayOneShot(chargingSfx);
    }
    private void ThrowCompleted()
    {
        isStarted = false;
        lineIndicatorSpeed.SetPosition(1, Vector3.zero);

        particles.Stop();
        SFXManager.Instance.StopOneShot();

        if (config.GetValidIndicatorTime() < timeElapsed || config.GetPressTimer().x > timeElapsed)
            return;
        
        Launch();
    }

    private void Launch()
    {
        var obj = PhotonNetwork.Instantiate(bullet.name, launchPoint.position, Quaternion.Euler(transform.rotation.eulerAngles));
        obj.GetComponent<Rigidbody2D>().AddForce(launchPoint.up* GetBulletSpeed(), ForceMode2D.Impulse);
        RoomStateManager.Instance.SetState(new ProjectileLaunchedState());
    }

    private float GetBulletSpeed()
    {
        return config.GetBulletSpeed(timeElapsed);
    }
}