using Anura.ConfigurationModule.Managers;
using System;
using UnityEngine;

public class PlayerMotionBehaviour : MonoBehaviour
{
    [SerializeField] private Collider2D ceilingCollider;

    private PlayerState playerState;

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    private Vector3 velocity = Vector3.zero;
    private bool isMoving = false;

    private void Awake()
    {
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void RegisterPlayerState(PlayerState state)
    {
        playerState = state;
    }

    public void RegisterMovementCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Movement.performed += value => SetMovementDirection(value.ReadValue<float>());
        playerActions.Movement.canceled += _ => { SetMovementDirection(0); };
    }

    public void RegisterJumpCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Jump.started += _ =>
        {
            playerState.SetHasJump(true);
        };
    }

    private void Update()
    {
        TryApplyRotationCorrection();
    }

    private void FixedUpdate()
    {
        if (playerState == null) return;
        if (playerState.movementDirection == 0 && !playerState.hasJump)
            return;

        if(playerState.isInAir && CheckIfIsGrounded())
        {
            playerState.SetHasJump(false);
            playerState.SetIsInAir(false);
        }
        Move(playerState.movementDirection * Time.deltaTime * GetSpeed(), playerState.hasJumpImpulseQueued);
    }

    public void Move(float move, bool jump)
    {
        if (CheckIfIsGrounded() || GetAirControl())
        {
            var targetVelocity = new Vector2(move * 10f, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref velocity, GetMovementSmoothing());
        }

        if (CheckIfIsGrounded() && jump)
        {
            _rigidbody2D.AddForce(Vector2.up * GetJumpForce(), ForceMode2D.Impulse);
            playerState.SetQueueJumpImpulse(false);
        }else if(jump) //Jumped in-air. Wasted.
        {
            playerState.SetQueueJumpImpulse(false);
        }
    }

    private void SetMovementDirection(float value)
    {
        playerState.SetMovementDirection(value);
    }

    private float GetSpeed()
    {
        return ConfigurationManager.Instance.Config.GetPlayerSpeed();
    }

    private bool CheckIfIsGrounded()
    {
        return Physics2D.IsTouchingLayers(ceilingCollider);
    }
    private bool GetAirControl()
    {
        return ConfigurationManager.Instance.Config.GetAirControl();
    }
    private float GetMovementSmoothing()
    {
        return ConfigurationManager.Instance.Config.GetMovementSmoothing();
    }

    private float GetJumpForce()
    {
        return ConfigurationManager.Instance.Config.GetPlayerJumpForce();
    }

    private void TryApplyRotationCorrection()
    {
        if (!_transform.eulerAngles.z.IsBetween(-30, 30))
        {
            var euler = _transform.eulerAngles;
            if (euler.z > 180)
                euler.z = euler.z - 360;

            euler.z = Mathf.Clamp(euler.z, -30, 30);

            _transform.eulerAngles = euler;
        }
    }
}
