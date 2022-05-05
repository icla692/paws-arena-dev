using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolvingGameState : IRoomState
{
    public GameResolveState state;
    public ResolvingGameState(GameResolveState state)
    {
        this.state = state;
    }
    public void Init(RoomStateManager context)
    {
    }

    public void OnExit()
    {
    }
}
