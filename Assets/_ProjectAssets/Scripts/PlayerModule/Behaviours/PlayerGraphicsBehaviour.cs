using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerGraphicsBehaviour : MonoBehaviour
{
    private Animator _animator;
    private PlayerState playerState;

    private bool isFacingRight = true;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void RegisterPlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
        this.playerState.onMovementDirectionChanged += OnMovementDirectionChanged;
        this.playerState.onJumpStateChanged += OnJumpStateChanged;
    }

    public void PreJumpAnimEnded()
    {
        this.playerState.SetQueueJumpImpulse(true);
    }

    public void JumpIsInAir()
    {
        this.playerState.SetIsInAir(true);
    }

    private void OnJumpStateChanged(bool jumpState)
    {
        _animator.SetBool("isJumping", jumpState);
    }

    private void OnMovementDirectionChanged(float dir)
    {
        if(dir > 0 && !isFacingRight)
        {
            Flip();
        }else if(dir < 0 && isFacingRight)
        {
            Flip();
        }

        _animator.SetBool("isMoving", dir != 0);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    [ContextMenu("Kill Kitty")]
    public void KillKitty()
    {
        _animator.SetBool("isDead", true);
    }
}