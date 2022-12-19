using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayerComponent : MonoBehaviour
{
    [SerializeField]
    private PlayerGraphicsBehaviour playerGraphicsBehaviour;
    [SerializeField]
    private GameObject weaponWrapper;

    private PlayerMotionBehaviour playerMotionBehaviour;
    private PlayerState state;

    private BotInputActions.PlayerActions playerActions;

    void Awake()
    {
        playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
        RoomStateManager.OnStateUpdated += OnStateUpdatedForBot;

        SetupBot();
    }

    private void OnDestroy()
    {
        if (state != null)
        {
            state.onWeaponIdxChanged -= OnWeaponOutChanged;
            PlayerActionsBar.WeaponIndexUpdated -= ChangeWeaponState;
        }

        RoomStateManager.OnStateUpdated -= OnStateUpdatedForBot;
    }

    private void SetupBot()
    {
        state = new PlayerState();
        state.onWeaponIdxChanged += OnWeaponOutChanged;

        playerActions = GameInputManager.Instance.GetBotActionMap().GetPlayerActions();
        playerMotionBehaviour.RegisterMovementCallbacks(playerActions);
        playerMotionBehaviour.RegisterJumpCallbacks(playerActions);
        playerMotionBehaviour.RegisterPlayerState(state);

        playerGraphicsBehaviour.RegisterPlayerState(state);

        var playerIndicatorBehaviour = GetComponentInChildren<PlayerIndicatorBehaviour>();
        playerIndicatorBehaviour.RegisterDirectionCallbacks(playerActions);

        var playerThrowBehaviour = GetComponentInChildren<PlayerThrowBehaviour>();
        playerThrowBehaviour.RegisterThrowCallbacks(playerActions);

        PlayerActionsBar.WeaponIndexUpdated += ChangeWeaponState;

        ChangeWeaponState(-1);
        playerActions.Disable();
    }

    private void OnStateUpdatedForBot(IRoomState roomState)
    {
        state.SetHasWeaponOut(-1);
        if (roomState is BotTurnState)
        {
            playerActions.Enable();
        }
        else
        {
            playerActions.Disable();
        }
    }

    private void OnWeaponOutChanged(int val)
    {
        playerMotionBehaviour.SetIsPaused(val >= 0);

        weaponWrapper.SetActive(val >= 0);
        weaponWrapper.GetComponent<WeaponBehaviour>().Init(val);
    }


    private void ChangeWeaponState(int idx)
    {
        if (state.weaponIdx == idx)
        {
            idx = -1;
        }

        state.SetHasWeaponOut(idx);
    }
}
