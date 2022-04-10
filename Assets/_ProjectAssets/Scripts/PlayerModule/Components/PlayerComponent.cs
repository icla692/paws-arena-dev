using Anura.ConfigurationModule.Managers;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    private bool hasMovement;
    private bool hasJump;

    private float currentDirection;

    private PlayerMovement characterController;

    private void Awake()
    {
        characterController = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        var playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        
        RegisterMovementCallbacks(playerActions);
        RegisterJumpCallbacks(playerActions);
    }

    private void FixedUpdate()
    {
        if (!hasMovement && !hasJump)
            return;

        characterController.Move(currentDirection * Time.deltaTime * GetSpeed(), hasJump);
    }

    private float GetSpeed()
    {
        return ConfigurationManager.Instance.Config.GetPlayerSpeed();
    }

    private void RegisterJumpCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Jump.started += _ => hasJump = true;
        playerActions.Jump.canceled += _ => hasJump = false;
    }

    private void RegisterMovementCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Movement.started += _ => hasMovement = true;
        playerActions.Movement.performed += value => SetCurrentDirection(value.ReadValue<float>());
        playerActions.Movement.canceled += _ => { hasMovement = false; SetCurrentDirection(0); };
    }

    private void SetCurrentDirection(float value)
    {
        currentDirection = value;
    }
}
