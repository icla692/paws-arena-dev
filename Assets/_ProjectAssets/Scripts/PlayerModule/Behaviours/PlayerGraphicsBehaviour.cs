using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGraphicsBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip jumpStartSound;
    [SerializeField]
    private AudioClip jumpEndSound;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerCustomization playerCustomization;
    private bool isMultiplayer;
    private PhotonView _photonView;
    private PlayerState playerState;

    private bool isFacingRight = true;

    void Start()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();

        _photonView = GetComponent<PhotonView>();
        if (!isMultiplayer)
        {
            _photonView.enabled = false;
        }

        SingleAndMultiplayerUtils.RpcOrLocal(this, _photonView, true, "SetCatNFT", RpcTarget.All, GameState.selectedNFT.ids.ToArray());
    }

    [PunRPC]
    public void SetCatNFT(string[] ids)
    {
        playerCustomization.SetCat(ids.ToList());
    }

    public void RegisterPlayerState(PlayerState playerState)
    {

        this.playerState = playerState;
        this.playerState.onJumpStateChanged += OnJumpStateChanged;
        this.playerState.onMovementDirectionChanged += OnMovementDirectionChanged;
        this.playerState.onMidJumpChanged += OnMidJumpStateChanged;

        PlayerManager.Instance.onHealthUpdated += OnHealthUpdated;
    }

    public void PreJumpAnimEnded()
    {
        if (!isMultiplayer || _photonView.IsMine)
        {
            this.playerState.SetQueueJumpImpulse(true);
            SFXManager.Instance.PlayOneShot(jumpStartSound);
        }
    }

    public void SetIsMidJump()
    {
        if (!isMultiplayer || _photonView.IsMine)
        {
            this.playerState.SetIsMidJump(true);
        }
    }

    public void AfterJump()
    {
        if (!isMultiplayer || _photonView.IsMine)
        {
            this.playerState.SetHasJump(false);
        }
    }

    private void OnJumpStateChanged(bool val)
    {
        if (val)
        {
            animator.SetBool("isJumping", true);
        }
    }

    private void OnMidJumpStateChanged(bool midJumpState)
    {
        if (!midJumpState)
        {
            animator.SetBool("isJumping", false);
            SFXManager.Instance.PlayOneShot(jumpEndSound);
        }
    }

    private void OnMovementDirectionChanged(float dir)
    {
        if (dir > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (dir < 0 && isFacingRight)
        {
            Flip();
        }

        animator.SetBool("isMoving", dir != 0);
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

    private void OnHealthUpdated(int health)
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
        }
    }

    public void SetShootingPhase(bool val)
    {
        animator.SetBool("isAiming", val);
    }
}
