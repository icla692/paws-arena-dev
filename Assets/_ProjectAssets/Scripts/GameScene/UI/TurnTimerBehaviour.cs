using Anura.ConfigurationModule.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTimerBehaviour : MonoBehaviour
{
    public RoomStateManager roomStateManager;
    public TMPro.TextMeshProUGUI text;

    private int turnTime;
    private bool hasRoundFinishedFromOtherReasons = false;
    private void OnEnable()
    {
        turnTime = ConfigurationManager.Instance.Config.GetTurnDurationInSeconds();
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        //It might act weird if we jump from MyTurnState directly to OtherPlayersState
        if(state is MyTurnState || state is OtherPlayersTurnState)
        {
            hasRoundFinishedFromOtherReasons = false;
            StartCoroutine(TurnCountdown());
        }
        else
        {
            hasRoundFinishedFromOtherReasons = true;
        }
    }

    private IEnumerator TurnCountdown()
    {
        var startTime = Time.time;
        float passedTime = Time.time - startTime;
        while (passedTime < turnTime && !hasRoundFinishedFromOtherReasons)
        {
            yield return new WaitForSeconds(0.25f);
            SetTimerText(turnTime - passedTime);
            passedTime = Time.time - startTime;
        }

        if (!hasRoundFinishedFromOtherReasons)
        {
            roomStateManager.SetState(new ProjectileLaunchedState());
        }
    }

    private void SetTimerText(float time)
    {
        text.text = "" + (int)Math.Floor(time);
    }
}
