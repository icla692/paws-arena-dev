using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkinsPanel : MonoBehaviour
{
    [SerializeField] private Button doneButton;
    [SerializeField] private Button showSkinsPanel;
    [SerializeField] private Button resetButton;
    [SerializeField] private WeaponSkinDisplay displayPrefab;
    [SerializeField] private Transform skinsHolder;

    [Space] [SerializeField] private Button airplaneButton;
    [SerializeField] private Button cannonButton;
    [SerializeField] private Button crossbowButton;
    [SerializeField] private Button mouseButton;
    [SerializeField] private Button rocketButton;
    [SerializeField] private Button splitRocketButton;

    private List<GameObject> shownSkins = new();
    private WeaponSkinType showingWeapons;
    private bool hasChangedSkin;

    private void OnEnable()
    {
        airplaneButton.onClick.AddListener(ShowAirplaneSkins);
        cannonButton.onClick.AddListener(ShowCannonSkins);
        crossbowButton.onClick.AddListener(ShowCrossBowSkins);
        mouseButton.onClick.AddListener(ShowMouseButton);
        rocketButton.onClick.AddListener(ShowRocketButton);
        splitRocketButton.onClick.AddListener(ShowSplitRocketButton);
        doneButton.onClick.AddListener(Close);
        showSkinsPanel.onClick.AddListener(Close);
        resetButton.onClick.AddListener(ResetSkins);
        WeaponSkinDisplay.OnSelectedWeapon += SelectSkin;
    }

    private void OnDisable()
    {
        airplaneButton.onClick.RemoveListener(ShowAirplaneSkins);
        cannonButton.onClick.RemoveListener(ShowCannonSkins);
        crossbowButton.onClick.RemoveListener(ShowCrossBowSkins);
        mouseButton.onClick.RemoveListener(ShowMouseButton);
        rocketButton.onClick.RemoveListener(ShowRocketButton);
        splitRocketButton.onClick.RemoveListener(ShowSplitRocketButton);
        doneButton.onClick.RemoveListener(Close);
        showSkinsPanel.onClick.RemoveListener(Close);
        resetButton.onClick.RemoveListener(ResetSkins);
        WeaponSkinDisplay.OnSelectedWeapon -= SelectSkin;
    }

    private void ShowAirplaneSkins()
    {
        Show(WeaponSkinType.Airplane);
        SelectButton(airplaneButton);
    }

    private void ShowCannonSkins()
    {
        Show(WeaponSkinType.Cannon);
        SelectButton(cannonButton);
    }

    private void ShowCrossBowSkins()
    {
        Show(WeaponSkinType.CrossBow);
        SelectButton(crossbowButton);
    }

    private void ShowMouseButton()
    {
        Show(WeaponSkinType.Mouse);
        SelectButton(mouseButton);
    }

    private void ShowRocketButton()
    {
        Show(WeaponSkinType.Rocket);
        SelectButton(rocketButton);
    }

    private void ShowSplitRocketButton()
    {
        Show(WeaponSkinType.SplitRocket);
        SelectButton(splitRocketButton);
    }

    private void SelectButton(Button _buttonToSelect)
    {
        Deselect(airplaneButton);
        Deselect(cannonButton);
        Deselect(crossbowButton);
        Deselect(mouseButton);
        Deselect(rocketButton);
        Deselect(splitRocketButton);
        
        Select(_buttonToSelect);
        
        void Deselect(Button _button)
        {
            ButtonHoverable _buttonHoverable = _button.GetComponent<ButtonHoverable>();
            if (_buttonHoverable==null)
            {
                return;
            }
            
            _buttonHoverable.Deselect();
        }

        void Select(Button _button)
        {
            ButtonHoverable _buttonHoverable = _button.GetComponent<ButtonHoverable>();
            if (_buttonHoverable==null)
            {
                return;
            }
            
            _buttonHoverable.Select();
        }
    }


    private void Close()
    {
        if (hasChangedSkin)
        {
            DataManager.Instance.PlayerData.TriggerSaveSelectedWeaponSkins();
            hasChangedSkin = false;
        }

        gameObject.SetActive(false);
    }

    private void ResetSkins()
    {
        DataManager.Instance.PlayerData.SelectStartingWeaponSkins(true);
        Show(showingWeapons);
    }

    private void SelectSkin(WeaponSkinSO _skinData)
    {
        DataManager.Instance.PlayerData.SelectSkin(_skinData, false);
        hasChangedSkin = true;
        Show(showingWeapons);
    }

    public void Show(WeaponSkinType _weaponSkinType = WeaponSkinType.Airplane)
    {
        showingWeapons = _weaponSkinType;
        ShowSkins();
        gameObject.SetActive(true);
    }

    private void ShowSkins()
    {
        ClearShownSkins();
        foreach (var _weaponSkinId in DataManager.Instance.PlayerData.WeaponSkins)
        {
            WeaponSkinSO _weaponSkinSo = WeaponSkinSO.Get(_weaponSkinId);
            if (_weaponSkinSo.Type != showingWeapons)
            {
                continue;
            }

            WeaponSkinDisplay _skinDisplay = Instantiate(displayPrefab, skinsHolder);
            _skinDisplay.Setup(_weaponSkinSo);
            shownSkins.Add(_skinDisplay.gameObject);
        }
    }

    private void ClearShownSkins()
    {
        foreach (var _shownSkin in shownSkins)
        {
            Destroy(_shownSkin);
        }

        shownSkins.Clear();
    }
}