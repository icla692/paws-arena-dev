using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LuckyWheelClaimDisplay : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] Image iconDisplay;
    [SerializeField] TextMeshProUGUI nameDisplay;
    private Action callBack;

    public void Setup(LuckyWheelRewardSO _reward, Action _callBack)
    {
        closeButton.onClick.AddListener(Close);
        callBack = _callBack;
        if (LuckyWheelUI.EquipmentData==null)
        {
            nameDisplay.text = _reward.Name;
            iconDisplay.sprite = _reward.Sprite;   
        }
        else
        {
            nameDisplay.text = LuckyWheelUI.EquipmentData.Thumbnail.name;
            iconDisplay.sprite = LuckyWheelUI.EquipmentData.Thumbnail;
            LuckyWheelUI.EquipmentData = null;
        }
        iconDisplay.SetNativeSize();
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    void Close()
    {
        closeButton.interactable = false;
        PUNRoomUtils _roomUtilities= GameObject.FindObjectOfType<PUNRoomUtils>();
        if (_roomUtilities!=null)
        {
            _roomUtilities.TryLeaveRoom();
        }
        else
        {
            gameObject.SetActive(false);
            callBack?.Invoke();
        }
    }
}
