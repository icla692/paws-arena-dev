using Anura.ConfigurationModule.Managers;
using UnityEngine;

public class PlayerMotionBehaviour : MonoBehaviour
{
    private bool hasMovement;
    private bool hasJump;

    private float currentDirection;

    private Transform thisT;

    private PlayerMovementController characterController;

    private void Awake()
    {
        characterController = GetComponent<PlayerMovementController>();
        thisT = transform;
    }

    private void Update()
    {
        if (!thisT.eulerAngles.z.IsBetween(-30, 30))
        {
            var euler = thisT.eulerAngles;
            if (euler.z > 180) 
                euler.z = euler.z - 360;

            euler.z = Mathf.Clamp(euler.z, -30, 30);

            thisT.eulerAngles = euler;
        }
    }

    private void FixedUpdate()
    {
        if (!hasMovement && !hasJump)
            return;

        characterController.Move(currentDirection * Time.deltaTime * GetSpeed(), hasJump);
    }

    public void RegisterMovementCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Movement.started += _ => hasMovement = true;
        playerActions.Movement.performed += value => SetMovementDirection(value.ReadValue<float>());
        playerActions.Movement.canceled += _ => { hasMovement = false; SetMovementDirection(0); };
    }

    public void RegisterJumpCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Jump.started += _ => hasJump = true;
        playerActions.Jump.canceled += _ => hasJump = false;
    }

    private void SetMovementDirection(float value)
    {
        currentDirection = value;
    }

    private float GetSpeed()
    {
        return ConfigurationManager.Instance.Config.GetPlayerSpeed();
    }
}
