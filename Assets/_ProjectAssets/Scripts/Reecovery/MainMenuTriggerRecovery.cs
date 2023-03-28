using System;
using UnityEngine;

public class MainMenuTriggerRecovery : MonoBehaviour
{
    [SerializeField] RecoveryHandler mainRecoveryHandler;

    private void OnEnable()
    {
        mainRecoveryHandler.ShowRecovery(GameState.selectedNFT.RecoveryEndDate);
        GameState.selectedNFT.UpdatedRecoveryTime += CheckIfShouldStopRecovering;
    }

    private void OnDisable()
    {
        GameState.selectedNFT.UpdatedRecoveryTime -= CheckIfShouldStopRecovering;
    }

    void CheckIfShouldStopRecovering()
    {
        if (GameState.selectedNFT.RecoveryEndDate <= DateTime.UtcNow)
        {
            mainRecoveryHandler.StopRecovery();
        }
    }
}
