using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Newtonsoft.Json;
using TMPro;

public class LuckyWheelUI : MonoBehaviour
{
    [SerializeField] GameObject playerPlatform;
    [SerializeField] LuckyWheel luckyWheel;
    [SerializeField] LuckyWheelClaimDisplay rewardDisplay;
    [SerializeField] Button respinButton;
    [SerializeField] Button claimButton;
    [SerializeField] GameObject insuficiantSnacksForRespin;
    [SerializeField] TextMeshProUGUI insuficiantSnacksText;
    [SerializeField] private EquipmentsConfig equipments;
    [SerializeField] private bool CloseOnClaim;
    [SerializeField] private BuyMilk buyMilk;
    LuckyWheelRewardSO choosenReward;
    public static EquipmentData EquipmentData = null;

    bool requestedToSeeReward = false;
    int currentRespinPrice;

    public async void RequestReward()
    {
        int _rewardId = -1;
        try
        {
            string resp = await NetworkManager.GETRequestCoroutine("/leaderboard/spin-the-wheel?matchId=" + PhotonNetwork.CurrentRoom.Name,
            (code, err) =>
            {
                Debug.LogWarning($"Failed to get reward type {err} : {code}");
            }, true);

            LuckyWheelRewardResponse _response = JsonConvert.DeserializeObject<LuckyWheelRewardResponse>(resp);
            _rewardId = _response.RewardType;
        }
        catch
        {
            _rewardId = 1;
        }
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
        if (playerPlatform!=null)
        {
            playerPlatform.SetActive(false);
        }
        gameObject.SetActive(true);

        respinButton.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(false);
        
        currentRespinPrice = DataManager.Instance.GameData.RespinPrice;
        SpinWheel();
    }

    private void OnEnable()
    {
        claimButton.onClick.AddListener(ClaimReward);
        respinButton.onClick.AddListener(Respin);
    }

    private void OnDisable()
    {
        claimButton.onClick.RemoveListener(ClaimReward);
        respinButton.onClick.RemoveListener(Respin);
    }

    void ClaimReward()
    {
        Claim(choosenReward);
    }

    void Claim(LuckyWheelRewardSO _reward)
    {
        switch (_reward.Type)
        {
            case ItemType.Common:
                DataManager.Instance.PlayerData.Crystals.CommonCrystal++;
                break;
            case ItemType.Uncommon:
                DataManager.Instance.PlayerData.Crystals.UncommonCrystal++;
                break;
            case ItemType.Rare:
                DataManager.Instance.PlayerData.Crystals.RareCrystal++;
                break;
            case ItemType.Epic:
                DataManager.Instance.PlayerData.Crystals.EpicCrystal++;
                break;
            case ItemType.Legendary:
                DataManager.Instance.PlayerData.Crystals.LegendaryCrystal++;
                break;
            case ItemType.Gift:
                EquipmentData = equipments.CraftItem();
                DataManager.Instance.PlayerData.AddOwnedEquipment(EquipmentData.Id);
                break;
            default:
                throw new System.Exception("Dont know how to reward reward type: " + _reward.Type);
        }

        rewardDisplay.Setup(_reward,CloseAfterClaim);
    }

    void CloseAfterClaim()
    {
        if (CloseOnClaim)
        {
            gameObject.SetActive(false);
        }
    }

    void Respin()
    {
        if (DataManager.Instance.PlayerData.Snacks< currentRespinPrice)
        {
            insuficiantSnacksForRespin.SetActive(true);
            insuficiantSnacksText.text = $"You don't have enaught Snacks.\n(takes {currentRespinPrice} for respin)";
            buyMilk.Setup();
            return;
        }

        DataManager.Instance.PlayerData.Snacks -= currentRespinPrice;
        respinButton.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(false);
        choosenReward = null;
        currentRespinPrice *= 2;
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
