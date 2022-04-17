using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public event Action<bool> onFacingRightChanged;
    public event Action<float> onMovementDirectionChanged;
    public event Action<bool> onJumpStateChanged;
    public bool isFacingRight { get; private set; } = true;
    public float movementDirection { get; private set; } = 0;
    public bool hasJump{ get; private set; } = false;

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
}
