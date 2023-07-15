using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeDisplay : MonoBehaviour
{
    [SerializeField] private Image rewardDisplay;
    [SerializeField] private TextMeshProUGUI descDisplay;
    [SerializeField] private TextMeshProUGUI progressDisplay;
    [SerializeField] private GameObject completed;
    
    public void Setup(ChallengeData _data)
    {
        ChallengeSO _challengeSO = ChallengesManager.Instance.Get(_data.Id);
        if (_data.Completed)
        {
            completed.SetActive(true);
            descDisplay.text = string.Empty;
            progressDisplay.text = string.Empty;
        }
        else
        {
            completed.SetActive(false);
            descDisplay.text = _challengeSO.Description;
            progressDisplay.text = $"{_data.Value}/{_challengeSO.AmountNeeded}";
        }

        rewardDisplay.sprite = _challengeSO.RewardSprite;
    }
}
