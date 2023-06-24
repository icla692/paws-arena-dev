using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisenchantUI : MonoBehaviour
{
    [SerializeField] private List<DisenchantmentItemBackground> backgrounds;
    [SerializeField] private Sprite[] crystalSprites;
    [SerializeField] private Image itemBackground;
    [SerializeField] private Button disenchantButton;
    [SerializeField] private Image selectedItemDisplay;
    [SerializeField] private DisenchantmentItemDisplay itemPrefab;
    [SerializeField] private Transform itemsHolder;
    [SerializeField] private DisenchantedItemDisplay rewardDisplay;
    [SerializeField] private Button upArrow;
    [SerializeField] private Button downArrow;
    
    [Space] [SerializeField] private EquipmentsConfig equipments;

    private List<DisenchantmentItemDisplay> shownItems = new List<DisenchantmentItemDisplay>();
    private EquipmentData selectedEquipment;
    private float moveAmount = 1;
    
    public void Setup()
    {
        HideRightUI();
        ShowItems();
        gameObject.SetActive(true);
    }

    void ShowItems()
    {
        foreach (var _ownedEquiptableId in DataManager.Instance.PlayerData.OwnedEquiptables)
        {
            EquipmentData _equipmentData = equipments.GetEquipmentData(_ownedEquiptableId);
            if (_equipmentData.Thumbnail.name=="none")
            {
                continue;
            }
            DisenchantmentItemDisplay _item = Instantiate(itemPrefab, itemsHolder);
            _item.Setup(_equipmentData);
            shownItems.Add(_item);
        }
    }

    private void OnEnable()
    {
        upArrow.onClick.AddListener(MoveContentUp);
        downArrow.onClick.AddListener(MoveContentDown);
        DisenchantmentItemDisplay.OnEquipmentClicked += ShowEquipment;
        disenchantButton.onClick.AddListener(Disenchant);
    }

    private void OnDisable()
    {
        upArrow.onClick.RemoveListener(MoveContentUp);
        downArrow.onClick.RemoveListener(MoveContentDown);
        DisenchantmentItemDisplay.OnEquipmentClicked += ShowEquipment;
        disenchantButton.onClick.RemoveListener(Disenchant);
    }

    void MoveContentUp()
    {
        Vector3 _itemsPosition = itemsHolder.transform.position;
        _itemsPosition.y -= moveAmount;
        itemsHolder.transform.position = _itemsPosition;
    }

    void MoveContentDown()
    {
        Vector3 _itemsPosition = itemsHolder.transform.position;
        _itemsPosition.y += moveAmount;
        itemsHolder.transform.position = _itemsPosition;
    }
    
    void ShowEquipment(EquipmentData _equipmentData)
    {
        selectedEquipment = _equipmentData;
        itemBackground.sprite = backgrounds.Find(_element => _element.Rarity == selectedEquipment.Rarity).Background;
        selectedItemDisplay.sprite = selectedEquipment.Thumbnail;
        itemBackground.gameObject.SetActive(true);
        disenchantButton.gameObject.SetActive(true);
        selectedItemDisplay.gameObject.SetActive(true);
    }

    void Disenchant()
    {
        HideRightUI();
        switch (selectedEquipment.Rarity)
        {
            case EquipmentRarity.Common:
                DataManager.Instance.PlayerData.Crystals.CommonCrystal += 1;
                rewardDisplay.Setup(crystalSprites[0]);
                break;
            case EquipmentRarity.Uncommon:
                DataManager.Instance.PlayerData.Crystals.UncommonCrystal += 1;
                rewardDisplay.Setup(crystalSprites[1]);
                break;
            case EquipmentRarity.Rare:
                DataManager.Instance.PlayerData.Crystals.RareCrystal += 1;
                rewardDisplay.Setup(crystalSprites[2]);
                break;
            case EquipmentRarity.Epic:
                DataManager.Instance.PlayerData.Crystals.EpicCrystal += 1;
                rewardDisplay.Setup(crystalSprites[3]);
                break;
            case EquipmentRarity.Legendary:
                DataManager.Instance.PlayerData.Crystals.LegendaryCrystal += 1;
                rewardDisplay.Setup(crystalSprites[4]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        DataManager.Instance.PlayerData.RemoveOwnedEquipment(selectedEquipment.Id);
        ClearShownItems();
        ShowItems();
        selectedEquipment = null;
    }

    void HideRightUI()
    {
        itemBackground.gameObject.SetActive(false);
        disenchantButton.gameObject.SetActive(false);
        selectedItemDisplay.gameObject.SetActive(false);
    }

    public void Close()
    {
        ClearShownItems();
        gameObject.SetActive(false);
    }

    void ClearShownItems()
    {
        foreach (var _item in shownItems)
        {
            Destroy(_item.gameObject);
        }
        
        shownItems.Clear();
    }
}
