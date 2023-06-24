using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LuckyWheelClaimDisplay : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] Image iconDisplay;
    [SerializeField] TextMeshProUGUI nameDisplay;

    public void Setup(LuckyWheelRewardSO _reward)
    {
        closeButton.onClick.AddListener(Close);
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
        GameObject.FindObjectOfType<PUNRoomUtils>().TryLeaveRoom();
    }
}
