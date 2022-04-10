using Anura.Templates.MonoSingleton;

public class GameInputManager : MonoSingleton<GameInputManager>
{
    private GameInputActions gameInput;
    private PlayerActionMap playerActionMap;

    protected override void Awake()
    {
        base.Awake();

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
