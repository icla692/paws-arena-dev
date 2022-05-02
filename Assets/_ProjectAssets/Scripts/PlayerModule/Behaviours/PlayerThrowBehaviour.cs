using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Extensions;
using System;
using UnityEngine;

public class PlayerThrowBehaviour : MonoBehaviour
{
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject bullet;

    [SerializeField] private LineRenderer lineIndicatorSpeed;

    private Config config => ConfigurationManager.Instance.Config;

    private bool isStarted;

    private float timeElapsed;
    private Vector3 valueToLerp;

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

    }
    private void ThrowCompleted()
    {
        isStarted = false;
        lineIndicatorSpeed.SetPosition(1, Vector3.zero);

        if (config.GetValidIndicatorTime() < timeElapsed || config.GetPressTimer().x > timeElapsed)
            return;
        
        Launch();
    }

    private void Launch()
    {
        //Debug.Log(transform.rotation.eulerAngles);
        
        var obj = Instantiate(bullet, launchPoint.position, Quaternion.Euler(transform.rotation.eulerAngles));
        obj.GetComponent<Rigidbody2D>().AddForce(launchPoint.up * GetBulletSpeed(), ForceMode2D.Impulse);
        RoomStateManager.Instance.SetState(new ProjectileLaunchedState());
    }

    private float GetBulletSpeed()
    {
        return config.GetBulletSpeed(timeElapsed);
    }
}