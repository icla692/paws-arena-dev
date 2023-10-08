using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkinDisplay : MonoBehaviour
{
    public static Action<WeaponSkinSO> OnSelectedWeapon;
    [SerializeField] private Image weaponDisplay;
    [SerializeField] private Button weaponSelection;
    [SerializeField] private GameObject selectedDisplay;

    private WeaponSkinSO weaponSkinSo;
    
    public void Setup(WeaponSkinSO _weaponSkinSo)
    {
        weaponSkinSo = _weaponSkinSo;
        weaponDisplay.sprite = weaponSkinSo.Preview;
        selectedDisplay.SetActive(DataManager.Instance.PlayerData.SelectedWeaponSkins.Contains(weaponSkinSo.Id));
    }

    private void OnEnable()
    {
        weaponSelection.onClick.AddListener(Select);
    }

    private void OnDisable()
    {
        weaponSelection.onClick.RemoveListener(Select);
    }

    private void Select()
    {
        if (selectedDisplay.activeSelf)
        {
            return;
        }
        OnSelectedWeapon?.Invoke(weaponSkinSo);
    }
}
