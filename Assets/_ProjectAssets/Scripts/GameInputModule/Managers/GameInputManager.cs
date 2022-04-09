using Anura.Templates.MonoSingleton;

public class GameInputManager : MonoSingleton<GameInputManager>
{
    private GameInputActions gameInput;
    private PlayerActionMap playerActionMap;

    private void Awake()
    {
        gameInput = new GameInputActions();
        SetActiveGameInput(true);
        playerActionMap = new PlayerActionMap(gameInput.Player);
    }

    public void SetActiveGameInput(bool value)
    {
        if (value)
        {
            gameInput.Enable();
        }
        else
        {
            gameInput.Disable();
        }
    }
    public PlayerActionMap GetPlayerActionMap()
    {
        return playerActionMap;
    }
}
