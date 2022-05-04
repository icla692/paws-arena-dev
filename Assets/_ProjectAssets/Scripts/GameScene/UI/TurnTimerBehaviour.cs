using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTimerBehaviour : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    private RoomStateManager roomStateManager;
    private PhotonView photonView;
    private int moveTurnTime;
    private int shootTurnTime;
    private float startTime;

    private void OnEnable()
    {
        roomStateManager= RoomStateManager.Instance;
        photonView = GetComponent<PhotonView>();

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

        if (photonView.IsMine)
        {
            if (state is MyTurnMovementState)
            {
                UpdateTimer(moveTurnTime, () => { roomStateManager.SetState(new MyTurnShootingState()); });
            }else if(state is MyTurnShootingState)
            {
                UpdateTimer(shootTurnTime, () => { roomStateManager.SetState(new ProjectileLaunchedState()); });
            }
        }
        else
        {
            if (state is OtherPlayersMoveTurnState)
            {
                UpdateTimer(moveTurnTime, () => { roomStateManager.SetState(new OtherPlayersShootingState()); });
            }
            else if (state is OtherPlayersShootingState)
            {
                UpdateTimer(shootTurnTime, () => { roomStateManager.SetState(new ProjectileLaunchedState()); });
            }
        }
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (photonView.IsMine)
        {
            if (state is MyTurnMovementState || state is MyTurnShootingState)
            {
                startTime = Time.time;
            }
            else
            {
                startTime = -1;
            }
        }else
        {
            if (state is OtherPlayersMoveTurnState || state is OtherPlayersShootingState)
            {
                startTime = Time.time;
            }
            else
            {
                startTime = -1;
            }
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
