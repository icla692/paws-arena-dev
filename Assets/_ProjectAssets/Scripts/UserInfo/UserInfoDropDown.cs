using System;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoDropDown : MonoBehaviour
{
    [SerializeField] Button seasonButton;
    [SerializeField] Button switchKittyButton;
    [SerializeField] private LobbyUIManager lobbyUI;
    [SerializeField] private LevelsPanel levelsPanel;

    float animationLength = 0.1f;
    bool isOpen;

    private void OnEnable()
    {
        seasonButton.onClick.AddListener(ShowSeason);
        switchKittyButton.onClick.AddListener(SwitchKitty);
        Close();
    }

    private void OnDisable()
    {
        seasonButton.onClick.RemoveListener(ShowSeason);
        switchKittyButton.onClick.RemoveListener(SwitchKitty);
    }

    public void HandleClick()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Show();
        }
    }

    public void Close()
    {
        gameObject.LeanScale(Vector3.zero, animationLength);
        isOpen = false;
    }

    public void Show()
    {
        gameObject.LeanScale(Vector3.one, animationLength);
        isOpen = true;
    }

    public void SwitchKitty()
    {
        lobbyUI.OpenNFTSelectionScreen();
        Close();
    }

    public void ShowSeason()
    {
        levelsPanel.Setup();
        Close();
    }
}
