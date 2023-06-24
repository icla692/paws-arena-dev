using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftedItemDisplay : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] Image iconDisplay;
    [SerializeField] TextMeshProUGUI nameDisplay;

    public void Setup(EquipmentData _equipmentData)
    {
        closeButton.onClick.AddListener(Close);

        nameDisplay.text = _equipmentData.Thumbnail.name;
        iconDisplay.sprite = _equipmentData.Thumbnail;
        iconDisplay.SetNativeSize();
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    void Close()
    {
        gameObject.SetActive(false);
    }
}
