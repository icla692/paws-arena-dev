using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisenchantedItemDisplay : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] Image iconDisplay;
    [SerializeField] TextMeshProUGUI nameDisplay;

    public void Setup(Sprite _sprite)
    {
        closeButton.onClick.AddListener(Close);

        nameDisplay.text = _sprite.name;
        iconDisplay.sprite = _sprite;
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
