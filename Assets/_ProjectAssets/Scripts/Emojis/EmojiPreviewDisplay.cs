using System;
using UnityEngine;
using UnityEngine.UI;

public class EmojiPreviewDisplay : MonoBehaviour
{
    public static Action<int> ShowEmoji;
    [SerializeField] Image previewDisplay;
    [SerializeField] Button clickButton;
    EmojiSO emojiSO;

    public void Setup(EmojiSO _emojiSO)
    {
        emojiSO = _emojiSO;
        previewDisplay.sprite = emojiSO.Preview;
        clickButton.onClick.AddListener(DisplayEmoji);
    }

    private void OnDestroy()
    {
        clickButton.onClick.RemoveListener(DisplayEmoji);
    }

    void DisplayEmoji()
    {
        ShowEmoji?.Invoke(emojiSO.Id);
    }
}
