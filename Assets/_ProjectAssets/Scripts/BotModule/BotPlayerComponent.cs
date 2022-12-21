using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayerComponent : MonoBehaviour
{
    [SerializeField]
    private BasePlayerComponent basePlayerComponent;
    [SerializeField]
    private PlayerGraphicsBehaviour playerGraphicsBehaviour;

    private PlayerMotionBehaviour playerMotionBehaviour;

    private BotInputActions.PlayerActions playerActions;

    void Awake()
    {
        playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
        RoomStateManager.OnStateUpdated += OnStateUpdatedForBot;

        StartCoroutine(Init());
    }

    private void OnDestroy()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdatedForBot;
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(1.5f);
        SetupBot();
    }

    private void SetupBot()
    {
        BotManager.Instance.RegisterBot(this);
        basePlayerComponent.PreSetup();

        playerActions = GameInputManager.Instance.GetBotActionMap().GetPlayerActions();
        playerMotionBehaviour.RegisterMovementCallbacks(playerActions);
        playerMotionBehaviour.RegisterJumpCallbacks(playerActions);
        playerMotionBehaviour.RegisterPlayerState(basePlayerComponent.state);

        playerGraphicsBehaviour.RegisterPlayerState(basePlayerComponent.state);
        BotManager.Instance.onHealthUpdated += playerGraphicsBehaviour.OnHealthUpdated;

        var playerIndicatorBehaviour = GetComponentInChildren<PlayerIndicatorBehaviour>();
        playerIndicatorBehaviour.RegisterDirectionCallbacks(playerActions);

        var playerThrowBehaviour = GetComponentInChildren<PlayerThrowBehaviour>();
        playerThrowBehaviour.RegisterThrowCallbacks(playerActions);


        basePlayerComponent.PostSetup();
        playerActions.Disable();
    }

    private void OnStateUpdatedForBot(IRoomState roomState)
    {
        basePlayerComponent.state.SetHasWeaponOut(-1);

        if (roomState is BotTurnState)
        {
            playerActions.Enable();
        }
        else
        {
            playerActions.Disable();
        }
    }
}
