using Anura.ConfigurationModule.Managers;
using Anura.Extensions;
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
        if (!thisT.eulerAngles.z.IsBetween(-90,90))
        {
            thisT.eulerAngles = thisT.eulerAngles.WithZ(Mathf.Clamp(thisT.eulerAngles.z, -90, 90));
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
