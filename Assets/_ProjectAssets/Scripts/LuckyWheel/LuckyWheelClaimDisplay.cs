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
        nameDisplay.text = _reward.Name;
        iconDisplay.sprite = _reward.Sprite;
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
