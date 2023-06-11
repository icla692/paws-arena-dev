using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardPanel : MonoBehaviour
{
    public static Action OnClosePressed;
    [SerializeField] Button closeButton;
    [SerializeField] Image iconDisplay;
    [SerializeField] TextMeshProUGUI nameDisplay;
    [SerializeField] GameObject holder;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        LevelRewardDisplay.OnClaimed += Setup;
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        LevelRewardDisplay.OnClaimed -= Setup;
    }


    public void Setup(LevelRewardBase _reward)
    {
        nameDisplay.text = _reward.Name;
        iconDisplay.sprite = _reward.Sprite;
        iconDisplay.SetNativeSize();
        holder.SetActive(true);
    }

    void Close()
    {
        OnClosePressed?.Invoke();
        holder.SetActive(false);
    }
}
