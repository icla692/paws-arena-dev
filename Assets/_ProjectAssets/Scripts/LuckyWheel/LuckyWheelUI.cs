using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyWheelUI : MonoBehaviour
{
    [SerializeField] GameObject playerPlatform;
    [SerializeField] LuckyWheel luckyWheel;
    [SerializeField] LuckyWheelClaimDisplay rewardDisplay;
    [SerializeField] Button respinButton;
    [SerializeField] Button claimButton;

    [SerializeField] decimal respinPrice;
    LuckyWheelRewardSO reward;

    public void Setup()
    {
        playerPlatform.SetActive(false);
        gameObject.SetActive(true);

        respinButton.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(false);

        claimButton.onClick.AddListener(ClaimReward);
        respinButton.onClick.AddListener(Respin);
        SpinWheel();
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(ClaimReward);
        respinButton.onClick.RemoveListener(Respin);
    }

    void ClaimReward()
    {
        Claim(reward);
    }

    void Claim(LuckyWheelRewardSO _reward)
    {
        //todo implement me claim reward
        switch (_reward.Type)
        {
            case LuckyWheelRewardType.Lime:
                break;
            case LuckyWheelRewardType.Green:
                break;
            case LuckyWheelRewardType.Blue:
                break;
            case LuckyWheelRewardType.Purple:
                break;
            case LuckyWheelRewardType.Orange:
                break;
            case LuckyWheelRewardType.Gift:
                break;
            default:
                throw new System.Exception("Dont know how to reward reward type: " + _reward.Type);
        }

        rewardDisplay.Setup(_reward);
    }

    void Respin()
    {
        respinButton.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(false);
        reward = null;
        SpinWheel();
    }

    void SpinWheel()
    {
        luckyWheel.Spin(SetReward);
    }

    void SetReward(LuckyWheelRewardSO _reward)
    {
        reward = _reward;
        StartCoroutine(ShowRewardAnimationRoutine());
    }

    IEnumerator ShowRewardAnimationRoutine()
    {

        yield return new WaitForSeconds(1);
        claimButton.gameObject.SetActive(true);
        respinButton.gameObject.SetActive(true);
    }

    public void Close()
    {
        playerPlatform.SetActive(true);
        gameObject.SetActive(false);
    }
}
