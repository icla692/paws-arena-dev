using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticesBehaviour : MonoBehaviour
{
    public GameObject wrapper;
    public TMPro.TextMeshProUGUI label;

    private void OnEnable()
    {
        wrapper.SetActive(false);
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (state is WaitingForAllPlayersToJoinState)
        {
            SetStatus("Waiting for players to join");
        }else if(state is StartingGameState)
        {
            SetStatus("Let's go");
        }else if(state is MyTurnMovementState)
        {
            SetStatus("Your Turn to Move");
        }else if(state is MyTurnShootingState)
        {
            SetStatus("Your Turn to Shoot");
        }
        else if(state is OtherPlayersMoveTurnState)
        {
            SetStatus("Other's Turn to Move");
        }
        else if (state is OtherPlayersShootingState)
        {
            SetStatus("Other's Turn to Shoot");
        }
        else if (state is ProjectileLaunchedState)
        {
            SetStatus("Attack Launched");
        }
    }

    private void SetStatus(string value)
    {
        wrapper.SetActive(true);
        label.text = value;
    }
}
