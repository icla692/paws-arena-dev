using Anura.ConfigurationModule.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTimerBehaviour : MonoBehaviour
{
    public RoomStateManager roomStateManager;
    public TMPro.TextMeshProUGUI text;

    private int moveTurnTime;
    private int shootTurnTime;
    private float startTime;

    private void OnEnable()
    {
        moveTurnTime = ConfigurationManager.Instance.Config.GetMoveTurnDurationInSeconds();
        shootTurnTime = ConfigurationManager.Instance.Config.GetShootTurnDurationInSeconds();
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void Update()
    {
        if (startTime <= 0) return;

        IRoomState state = RoomStateManager.Instance.currentState;

        if (state is MyTurnMovementState)
        {
            UpdateTimer(moveTurnTime, () => { roomStateManager.SetState(new MyTurnShootingState()); });
        }
        else if(state is OtherPlayersMoveTurnState)
        {
            UpdateTimer(moveTurnTime, () => { roomStateManager.SetState(new OtherPlayersShootingState()); });
        }
        else if(state is MyTurnShootingState || state is OtherPlayersShootingState)
        {
            UpdateTimer(shootTurnTime, () => { roomStateManager.SetState(new ProjectileLaunchedState()); });
        }
    }

    private void OnStateUpdated(IRoomState state)
    {
        if(state is MyTurnMovementState || state is OtherPlayersMoveTurnState || state is MyTurnShootingState)
        {
            startTime = Time.time;
        }
        else
        {
            startTime = -1;
        }
    }

    private void UpdateTimer(int totalTime, Action onFinished)
    {
        float passedTime = Time.time - startTime;
        if (passedTime >= totalTime)
        {
            startTime = -1;
            SetTimerText(0);
            onFinished?.Invoke();
        }
        else
        {
            SetTimerText(moveTurnTime - passedTime);
        }
    }

    private void SetTimerText(float time)
    {
        text.text = "" + (int)Math.Floor(time);
    }
}
