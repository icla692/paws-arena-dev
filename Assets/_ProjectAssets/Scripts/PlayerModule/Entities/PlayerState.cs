using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public event Action<bool> onFacingRightChanged;
    public event Action<float> onMovementDirectionChanged;
    public event Action<bool> onJumpStateChanged;
    public event Action<bool> onJumpImpulseQueuedChanged;

    public bool isFacingRight { get; private set; } = true;
    public float movementDirection { get; private set; } = 0;

    //has Jump -> start animation and whole coroutine
    //But jumps after 0.5s when impulse queued by animation clip
    public bool hasJump{ get; private set; } = false;
    public bool hasJumpImpulseQueued { get; private set; } = false;
    public bool isInAir { get; private set; } = false;

    public void SetIsFacingRight(bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;
        onFacingRightChanged?.Invoke(this.isFacingRight);
    }

    public void SetMovementDirection(float direction)
    {
        this.movementDirection = direction;
        onMovementDirectionChanged?.Invoke(this.movementDirection);
    }
    public void SetHasJump(bool hasJump)
    {
        this.hasJump = hasJump;
        onJumpStateChanged?.Invoke(this.hasJump);
    }

    public void SetQueueJumpImpulse(bool val)
    {
        hasJumpImpulseQueued = val;
        onJumpImpulseQueuedChanged?.Invoke(hasJumpImpulseQueued);
    }
    public void SetIsInAir(bool val)
    {
        this.isInAir = val;
    }
}
