using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelRewardDisplay : MonoBehaviour
{
    public static Action<LevelRewardBase> OnClaimed;
    [SerializeField] Image rewardDisplay;
    [SerializeField] Image background;
    [SerializeField] Sprite normalBackground;
    [SerializeField] Sprite premiumBackground;

    [SerializeField] Button claimButton;
    [SerializeField] Image claimImage;
    [SerializeField] Sprite normalClaim;
    [SerializeField] Sprite premiumClaim;
    [SerializeField] GameObject lockedImage;
    [SerializeField] GameObject claimedObject;
    [SerializeField] GameObject shadowPanel;
    [SerializeField] bool isPremium;

    LevelRewardBase reward;
    int level;
    bool canClaim;

    public bool CanClaim => canClaim;

    public void Setup(LevelRewardBase _reward, int _level)
    {
        reward = _reward;
        level = _level;
        background.sprite = _reward.IsPremium ? premiumBackground : normalBackground;
        claimImage.sprite = _reward.IsPremium ? premiumClaim : normalClaim;
        rewardDisplay.gameObject.SetActive(true);
        rewardDisplay.sprite = _reward.Sprite;
        if (ValuablesManager.Instance.SeasonData.HasClaimed(_reward, level))
        {
            claimedObject.SetActive(true);
        }
        else
        {
            if (level <= ValuablesManager.Instance.SeasonData.Level)
            {
                claimButton.onClick.AddListener(ClaimReward);
                if (_reward.IsPremium && !ValuablesManager.Instance.SeasonData.HasPass)
                {
                    lockedImage.gameObject.SetActive(true);
                    claimButton.interactable = false;
                }
                else
                {
                    canClaim = true;
                }

                claimButton.gameObject.SetActive(true);
            }
            else
            {
                if (_reward.IsPremium)
                {
                    lockedImage.SetActive(true);
                }
            }
        }

        if (!claimButton.gameObject.activeSelf||!claimButton.interactable)
        {
            shadowPanel.SetActive(true);
        }
    }

   public void ClaimReward()
    {
        reward.Claim();
        OnClaimed?.Invoke(reward);
        ClaimedReward _claimedReward = new ClaimedReward()
        {
            IsPremium = reward.IsPremium,
            Level = level,
        };
        ValuablesManager.Instance.SeasonData.AddCollectedLevelReward(_claimedReward);
        SetupEmpty();
        Setup(reward,level);

    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveAllListeners();
    }

    public void SetupEmpty()
    {
        background.sprite = isPremium ? premiumBackground : normalBackground;
        claimButton.interactable = true;
        claimButton.gameObject.SetActive(false);
        lockedImage.SetActive(false);
        rewardDisplay.gameObject.SetActive(false);
        canClaim = false;
        shadowPanel.SetActive(false);
        claimedObject.SetActive(false);
    }
}
