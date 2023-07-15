using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Image levelProgressDisplay;
    [SerializeField] TextMeshProUGUI levelDisplay;
    [Space()]
    [SerializeField] RecoveryHandler mainRecoveryHandler;

    private void OnEnable()
    {
        mainRecoveryHandler.ShowRecovery(GameState.selectedNFT.RecoveryEndDate,GameState.selectedNFT.imageUrl);
        GameState.selectedNFT.UpdatedRecoveryTime += CheckIfShouldStopRecovering;
        DataManager.Instance.PlayerData.UpdatedExp += ShowLevelProgress;

        ShowLevelProgress();
    }

    private void OnDisable()
    {
        GameState.selectedNFT.UpdatedRecoveryTime -= CheckIfShouldStopRecovering;
        DataManager.Instance.PlayerData.UpdatedExp -= ShowLevelProgress;
    }

    void CheckIfShouldStopRecovering()
    {
        if (GameState.selectedNFT.RecoveryEndDate <= DateTime.UtcNow)
        {
            mainRecoveryHandler.StopRecovery();
        }
    }


    void ShowLevelProgress()
    {
        levelProgressDisplay.fillAmount = (float)DataManager.Instance.PlayerData.ExperienceOnCurrentLevel / DataManager.Instance.PlayerData.ExperienceForNextLevel;
        levelDisplay.text = DataManager.Instance.PlayerData.Level.ToString();
    }
}
