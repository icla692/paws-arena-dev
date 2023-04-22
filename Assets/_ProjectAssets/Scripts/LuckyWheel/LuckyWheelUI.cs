using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using Newtonsoft.Json;

public class LuckyWheelUI : MonoBehaviour
{
    [SerializeField] GameObject playerPlatform;
    [SerializeField] LuckyWheel luckyWheel;
    [SerializeField] LuckyWheelClaimDisplay rewardDisplay;
    [SerializeField] Button respinButton;
    [SerializeField] Button claimButton;

    [SerializeField] decimal respinPrice;
    LuckyWheelRewardSO choosenReward;

    bool requestedToSeeReward = false;

    public async void RequestReward()
    {
        Debug.Log("Requestiong rewrd");
        int _rewardId = -1;
        try
        {
            string resp = await NetworkManager.GETRequestCoroutine("/leaderboard/spin-the-wheel?matchId=" + PhotonNetwork.CurrentRoom.Name,
            (code, err) =>
            {
                Debug.LogWarning($"Failed to get reward type {err} : {code}");
            }, true);

            Debug.Log($"Got reward type from server: {resp}");
            LuckyWheelRewardResponse _response = JsonConvert.DeserializeObject<LuckyWheelRewardResponse>(resp);
            _rewardId = _response.RewardType;
        }
        catch
        {
            Debug.LogWarning($"Failed getting reward id from server");
            _rewardId = 1;
        }
        Debug.Log("Got reward: " + _rewardId);
        choosenReward = LuckyWheelRewardSO.Get(_rewardId);
        if (requestedToSeeReward)
        {
            Setup();
        }
    }

    public void ShowReward()
    {
        requestedToSeeReward = true;
        if (choosenReward == null)
        {
            return;
        }

        Setup();
    }

    void Setup()
    {
        Debug.Log("Setting up wheel");
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
        Claim(choosenReward);
        //TODO tell server that client claimed reward
    }

    void Claim(LuckyWheelRewardSO _reward)
    {
        switch (_reward.Type)
        {
            case LuckyWheelRewardType.Lime:
                ValuablesManager.Instance.LimeCrystal++;
                break;
            case LuckyWheelRewardType.Green:
                ValuablesManager.Instance.GreenCrystal++;
                break;
            case LuckyWheelRewardType.Blue:
                ValuablesManager.Instance.BlueCrystal++;
                break;
            case LuckyWheelRewardType.Purple:
                ValuablesManager.Instance.PurpleCrystal++;
                break;
            case LuckyWheelRewardType.Orange:
                ValuablesManager.Instance.OrangeCrystal++;
                break;
            case LuckyWheelRewardType.Gift:
                ValuablesManager.Instance.GiftItem++;
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
        choosenReward = null;
        RequestReward();
    }

    void SpinWheel()
    {
        luckyWheel.Spin(EnableButtons, choosenReward);
    }

    void EnableButtons()
    {
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