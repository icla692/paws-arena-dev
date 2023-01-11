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

    private BotAI botAI;

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
        yield return new WaitForEndOfFrame();
        SetupBot();
        SetupAI();
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

        BotPlayerAPI.Instance.Init(playerMotionBehaviour, playerIndicatorBehaviour.indicatorCircle);

        basePlayerComponent.PostSetup();
        playerActions.Disable();
    }

    private void SetupAI()
    {
        botAI = gameObject.AddComponent<BotAI>();
    }

    private void OnStateUpdatedForBot(IRoomState roomState)
    {
        basePlayerComponent.state.SetHasWeaponOut(-1);

        if (roomState is BotTurnState)
        {
            playerActions.Enable();
            botAI.Play();
        }
        else
        {
            playerActions.Disable();
            botAI.Wait();
        }
    }
}
