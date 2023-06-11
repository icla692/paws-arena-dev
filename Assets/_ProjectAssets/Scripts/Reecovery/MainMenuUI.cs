using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button showLevels;
    [SerializeField] Image levelProgressDisplay;
    [Space()]
    [SerializeField] RecoveryHandler mainRecoveryHandler;
    [SerializeField] LevelsPanel levelsPanel;

    private void OnEnable()
    {
        mainRecoveryHandler.ShowRecovery(GameState.selectedNFT.RecoveryEndDate);
        GameState.selectedNFT.UpdatedRecoveryTime += CheckIfShouldStopRecovering;
        ValuablesManager.Instance.SeasonData.UpdatedExp += ShowLevelProgress;
        showLevels.onClick.AddListener(ShowLevels);

        ShowLevelProgress();
    }

    private void OnDisable()
    {
        GameState.selectedNFT.UpdatedRecoveryTime -= CheckIfShouldStopRecovering;
        ValuablesManager.Instance.SeasonData.UpdatedExp -= ShowLevelProgress;
        showLevels.onClick.RemoveListener(ShowLevels);
    }

    void CheckIfShouldStopRecovering()
    {
        if (GameState.selectedNFT.RecoveryEndDate <= DateTime.UtcNow)
        {
            mainRecoveryHandler.StopRecovery();
        }
    }

    void ShowLevels()
    {
        levelsPanel.Setup();
    }

    void ShowLevelProgress()
    {
        levelProgressDisplay.fillAmount = (float)ValuablesManager.Instance.SeasonData.ExperienceOnCurrentLevel / ValuablesManager.Instance.SeasonData.Scaler;
    }
}
