using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyWheelUI : MonoBehaviour
{
    [SerializeField] GameObject playerPlatform;
    [SerializeField] LuckyWheel luckyWheel;
    [SerializeField] Button spinButton;
    [SerializeField] Button respinButton;
    [SerializeField] Button claimButton;

    LuckyWheelRewardSO reward;

    public void Setup()
    {
        playerPlatform.SetActive(false);
        gameObject.SetActive(true);

        respinButton.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(false);

        claimButton.onClick.AddListener(ClaimReward);
        spinButton.onClick.AddListener(Spin);
        respinButton.onClick.AddListener(Respin);
    }

    //Called from animation event
    void EnableSpinButton()
    {
        spinButton.interactable = true;
        spinButton.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(ClaimReward);
        spinButton.onClick.RemoveListener(Spin);
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

        Debug.Log("Claimed reward");
        Close();
    }

    void Spin()
    {
        spinButton.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(true);
        claimButton.interactable = false;
        SpinWheel();
    }

    void Respin()
    {
        respinButton.interactable = false;
        respinButton.gameObject.SetActive(false);
        claimButton.interactable = false;
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
        claimButton.interactable = true;
        respinButton.interactable = true;
        respinButton.gameObject.SetActive(true);
    }

    void Close()
    {
        playerPlatform.SetActive(true);
        gameObject.SetActive(false);
    }
}
