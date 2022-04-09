using UnityEngine;

public class PlayerActionMap
{
    public GameInputActions.PlayerActions playerActions;
    public PlayerActionMap(GameInputActions.PlayerActions playerActions)
    {
        this.playerActions = playerActions;
        AddEvent();
    }

    public void AddEvent()
    {
        playerActions.Movement.performed += Movement_performed;
    }

    private void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ReadValue<float>());
    }

    public void SetActivePlayerActionMap(bool value)
    {
        if (value)
        {
            playerActions.Enable();
        }
        else
        {
            playerActions.Disable();
        }
    }
}
