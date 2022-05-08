using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticesBehaviour : MonoBehaviour
{
    public GameObject wrapper;
    public TMPro.TextMeshProUGUI label;

    public GameObject endGameNotice;

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

        endGameNotice.SetActive(false);

        if (state is WaitingForAllPlayersToJoinState)
        {
            SetStatus("Waiting for players to join");
        }else if(state is StartingGameState)
        {
            SetStatus("Let's go");
        }else if(state is MyTurnMovementState)
        {
            SetStatus("");
        }
        else if(state is MyTurnShootingState)
        {
            SetStatus("");
        }
        else if(state is OtherPlayersMoveTurnState)
        {
            SetStatus("");
        }
        else if (state is OtherPlayersShootingState)
        {
            SetStatus("");
        }
        else if (state is ProjectileLaunchedState)
        {
            SetStatus("Attack Launched");
        }else if(state is ResolvingGameState)
        {
            ResolvingGameState crtState = (ResolvingGameState)state;
            endGameNotice.SetActive(true);
            if(crtState.state == GameResolveState.PLAYER_1_WIN)
            {
                SetStatus("Player 1 won!!");
            }else if(crtState.state == GameResolveState.PLAYER_2_WIN)
            {
                SetStatus("Player 2 won!!");
            }else if(crtState.state == GameResolveState.DRAW)
            {
                SetStatus("It's a draw!");
            }
        }
    }

    private void SetStatus(string value)
    {
        wrapper.SetActive(true);
        label.text = value;
    }
}
