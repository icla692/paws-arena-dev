using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolvingGameState : IRoomState
{
    public GameResolveState state;
    private RoomStateManager context;

    public ResolvingGameState(GameResolveState state)
    {
        this.state = state;
    }
    public void Init(RoomStateManager context)
    {
        this.context = context;
        context.StartCoroutine(EndingCoroutine());
    }

    private IEnumerator EndingCoroutine()
    {
        GameState.gameResolveState = this.state;
        yield return new WaitForSeconds(3f);
        context.LoadAfterGameScene(state);
    }

    public void OnExit()
    {
    }
}
