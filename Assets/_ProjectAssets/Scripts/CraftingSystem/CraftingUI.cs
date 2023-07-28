using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI limeAmountDisplay;
    [SerializeField] TextMeshProUGUI greenAmountDisplay;
    [SerializeField] TextMeshProUGUI blueAmountDisplay;
    [SerializeField] TextMeshProUGUI purpleAmountDisplay;
    [SerializeField] TextMeshProUGUI orangeAmountDisplay;

    [SerializeField] Button commonButton;
    [SerializeField] Button uncommonButton;
    [SerializeField] Button rareButton;
    [SerializeField] Button epicButton;
    [SerializeField] Button legendaryButton;

    //top frame
    [SerializeField] GameObject topHolder;
    [SerializeField] Image ingridiantImage;
    [SerializeField] TextMeshProUGUI craftText;
    [SerializeField] TextMeshProUGUI craftAmountDisplay;
    [SerializeField] Image endResultImage;
    [SerializeField] Button craftCrystalButton;
    [SerializeField] TextMeshProUGUI craftButtonText;
    [SerializeField] Image shardBackground;

    //bot frame
    [SerializeField] Image botFrameImage;
    [SerializeField] TextMeshProUGUI botFrameText;
    [SerializeField] TextMeshProUGUI botAmountDisplay;
    [SerializeField] Button botCraftItemButton;

    [Space]
    [Space]
    [SerializeField] EquipmentsConfig equipments;
    [SerializeField] CraftedItemDisplay itemDisplay;
    [SerializeField] private CraftFinishedDisplay craftingFinished;
    CraftingRecepieSO showingRecepie;
    
    public void Setup()
    {
        DataManager.Instance.PlayerData.Crystals.UpdatedCommonCrystal += ShowCristals;
        DataManager.Instance.PlayerData.Crystals.UpdatedUncommonCrystal += ShowCristals;
        DataManager.Instance.PlayerData.Crystals.UpdatedRareCrystal += ShowCristals;
        DataManager.Instance.PlayerData.Crystals.UpdatedEpicCrystal += ShowCristals;
        DataManager.Instance.PlayerData.Crystals.UpdatedLegendaryCrystal += ShowCristals;
        CraftingProcess.OnFinishedCrafting += FinishedCrafting;

        commonButton.onClick.AddListener(() => ShowRecepie(ItemType.Common));
        uncommonButton.onClick.AddListener(() => ShowRecepie(ItemType.Uncommon));
        rareButton.onClick.AddListener(() => ShowRecepie(ItemType.Rare));
        epicButton.onClick.AddListener(() => ShowRecepie(ItemType.Epic));
        legendaryButton.onClick.AddListener(() => ShowRecepie(ItemType.Legendary));
        craftCrystalButton.onClick.AddListener(CraftCrystal);
        botCraftItemButton.onClick.AddListener(CraftItem);

        ShowRecepie(ItemType.Common);
        ShowCristals();
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.Crystals.UpdatedCommonCrystal -= ShowCristals;
        DataManager.Instance.PlayerData.Crystals.UpdatedUncommonCrystal -= ShowCristals;
        DataManager.Instance.PlayerData.Crystals.UpdatedRareCrystal -= ShowCristals;
        DataManager.Instance.PlayerData.Crystals.UpdatedEpicCrystal -= ShowCristals;
        DataManager.Instance.PlayerData.Crystals.UpdatedLegendaryCrystal -= ShowCristals;
        CraftingProcess.OnFinishedCrafting -= FinishedCrafting;

        craftCrystalButton.onClick.RemoveListener(CraftCrystal);
        commonButton.onClick.RemoveAllListeners();
        uncommonButton.onClick.RemoveAllListeners();
        rareButton.onClick.RemoveAllListeners();
        epicButton.onClick.RemoveAllListeners();
        legendaryButton.onClick.RemoveAllListeners();
        botCraftItemButton.onClick.RemoveListener(CraftItem);
    }

    void ShowCristals()
    {
        limeAmountDisplay.text = DataManager.Instance.PlayerData.Crystals.CommonCrystal.ToString();
        greenAmountDisplay.text = DataManager.Instance.PlayerData.Crystals.UncommonCrystal.ToString();
        blueAmountDisplay.text = DataManager.Instance.PlayerData.Crystals.RareCrystal.ToString();
        purpleAmountDisplay.text = DataManager.Instance.PlayerData.Crystals.EpicCrystal.ToString();
        orangeAmountDisplay.text = DataManager.Instance.PlayerData.Crystals.LegendaryCrystal.ToString();
        ShowRecepie(showingRecepie.Inggrdiant);
    }

    void ShowRecepie(ItemType _ingridiant)
    {
        showingRecepie = CraftingRecepieSO.Get(_ingridiant);

        if (_ingridiant == ItemType.Common)
        {
            ShowBotFrame(_ingridiant);
            topHolder.SetActive(false);
            return;
        }
        
        topHolder.SetActive(true);

        CraftingRecepieSO _topRecepie = CraftingRecepieSO.Get((ItemType)((int)_ingridiant-1));
        ingridiantImage.sprite = _topRecepie.EndProductSprite;
        craftText.text = $"Get 1 <color={_topRecepie.EndProductColor}>{_topRecepie.EndProduct}</color> shard by\ncombining {_topRecepie.AmountNeeded} <color={_topRecepie.IngridiantColor}>{_topRecepie.Inggrdiant}</color> shards";
        float _amountOfIngridiants;
        switch (_topRecepie.Inggrdiant)
        {
            case ItemType.Common:
                _amountOfIngridiants = DataManager.Instance.PlayerData.Crystals.CommonCrystal;
                break;
            case ItemType.Uncommon:
                _amountOfIngridiants = DataManager.Instance.PlayerData.Crystals.UncommonCrystal;
                break;
            case ItemType.Rare:
                _amountOfIngridiants = DataManager.Instance.PlayerData.Crystals.RareCrystal;
                break;
            case ItemType.Epic:
                _amountOfIngridiants = DataManager.Instance.PlayerData.Crystals.EpicCrystal;
                break;
            case ItemType.Legendary:
                _amountOfIngridiants = DataManager.Instance.PlayerData.Crystals.LegendaryCrystal;
                break;
            default:
                throw new System.Exception("Dont know how to check if player can craft: " + _topRecepie.Inggrdiant);
        }
        if (_amountOfIngridiants >= showingRecepie.AmountNeeded)
        {
            craftAmountDisplay.text = $"<color=#00ff00>{_amountOfIngridiants}</color>/<color={_topRecepie.EndProductColor}>{_topRecepie.AmountNeeded}</color>";
            craftCrystalButton.interactable = true;
        }
        else
        {
            craftAmountDisplay.text = $"<color=#ff0000>{_amountOfIngridiants}</color>/<color={_topRecepie.EndProductColor}>{_topRecepie.AmountNeeded}</color>";
            craftCrystalButton.interactable = false;
        }

        endResultImage.sprite = _topRecepie.IngridiantSprite;
        ingridiantImage.SetNativeSize();
        endResultImage.SetNativeSize();
        craftButtonText.text = "Craft";
        shardBackground.sprite = showingRecepie.TopOfferBackground;

        if (DataManager.Instance.PlayerData.CraftingProcess != null)
        {
            craftCrystalButton.interactable = false;
        }

        ShowBotFrame(_ingridiant);
    }

    void ShowBotFrame(ItemType _ingridiant)
    {
        CraftingRecepieSO _recepie = CraftingRecepieSO.Get(_ingridiant);
        botFrameText.text = $"Combine {_recepie.BotAmountNeeded} <color={_recepie.IngridiantColor}>{_recepie.Inggrdiant}</color> shards\nto get 1 <color={_recepie.IngridiantColor}>{_recepie.Inggrdiant}</color> item";
        float _amountGot = 0;
        switch (_ingridiant)
        {
            case ItemType.Common:
                _amountGot = DataManager.Instance.PlayerData.Crystals.CommonCrystal;
                break;
            case ItemType.Uncommon:
                _amountGot = DataManager.Instance.PlayerData.Crystals.UncommonCrystal;
                break;
            case ItemType.Rare:
                _amountGot = DataManager.Instance.PlayerData.Crystals.RareCrystal;
                break;
            case ItemType.Epic:
                _amountGot = DataManager.Instance.PlayerData.Crystals.EpicCrystal;
                break;
            case ItemType.Legendary:
                _amountGot = DataManager.Instance.PlayerData.Crystals.LegendaryCrystal;
                break;
            default:
                throw new Exception("Don't know how to show bot frame for item: " + _ingridiant);
        }
        botAmountDisplay.text = $"<color={_recepie.IngridiantColor}>{_amountGot}</color>/<color={showingRecepie.IngridiantColor}>{showingRecepie.BotAmountNeeded}</color>";
        if (_amountGot >= _recepie.BotAmountNeeded)
        {
            botCraftItemButton.interactable = true;
        }
        else
        {
            botCraftItemButton.interactable = false;
        }

        botFrameImage.sprite = _recepie.BottomOfferBackground;
    }

    void CraftCrystal()
    {
        CraftingRecepieSO _topRecepie = CraftingRecepieSO.Get((ItemType)((int)showingRecepie.Inggrdiant-1));

        CraftingProcess _craftingProcess = new CraftingProcess();
        _craftingProcess.DateStarted = DateTime.UtcNow;
        _craftingProcess.Ingridiant = _topRecepie.Inggrdiant;

        DataManager.Instance.PlayerData.CraftingProcess = _craftingProcess;

        switch (_topRecepie.Inggrdiant)
        {
            case ItemType.Common:
                DataManager.Instance.PlayerData.Crystals.CommonCrystal -= _topRecepie.AmountNeeded;
                break;
            case ItemType.Uncommon:
                DataManager.Instance.PlayerData.Crystals.UncommonCrystal -= _topRecepie.AmountNeeded;
                break;
            case ItemType.Rare:
                DataManager.Instance.PlayerData.Crystals.RareCrystal -= _topRecepie.AmountNeeded;
                break;
            case ItemType.Epic:
                DataManager.Instance.PlayerData.Crystals.EpicCrystal -= _topRecepie.AmountNeeded;
                break;
            case ItemType.Legendary:
                DataManager.Instance.PlayerData.Crystals.LegendaryCrystal -= _topRecepie.AmountNeeded;
                break;
            default:
                throw new System.Exception("Don't know how to start crafting process for ingredient: " + _topRecepie.Inggrdiant);
        }

        ShowRecepie(showingRecepie.Inggrdiant);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void FinishedCrafting()
    {
        craftingFinished.Setup($"Congratulations, you just crafted a {showingRecepie.EndProduct} shard");
        ShowCristals();
        ShowRecepie(showingRecepie.Inggrdiant);
        craftButtonText.text = "Craft";
        EventsManager.OnCraftedCrystal?.Invoke();
    }

    void CraftItem()
    {
        EquipmentData _equipmentData = equipments.CraftItem(showingRecepie);
        ShowItem(_equipmentData);
        DataManager.Instance.PlayerData.AddOwnedEquipment(_equipmentData.Id);
        switch (showingRecepie.Inggrdiant)
        {
            case ItemType.Common:
                DataManager.Instance.PlayerData.Crystals.CommonCrystal -= showingRecepie.BotAmountNeeded;
                break;
            case ItemType.Uncommon:
                DataManager.Instance.PlayerData.Crystals.UncommonCrystal -= showingRecepie.BotAmountNeeded;
                break;
            case ItemType.Rare:
                DataManager.Instance.PlayerData.Crystals.RareCrystal -= showingRecepie.BotAmountNeeded;
                break;
            case ItemType.Epic:
                DataManager.Instance.PlayerData.Crystals.EpicCrystal -= showingRecepie.BotAmountNeeded;
                break;
            case ItemType.Legendary:
                DataManager.Instance.PlayerData.Crystals.LegendaryCrystal -= showingRecepie.BotAmountNeeded;
                break;
            default:
                throw new Exception("Don't know how to craft item for: " + showingRecepie.Inggrdiant);
        }
        EventsManager.OnCraftedItem?.Invoke();
    }

    async void ShowItem(EquipmentData _equipmentData)
    {
        itemDisplay.Setup(_equipmentData);
    }

    private void Update()
    {
        if (DataManager.Instance.PlayerData.CraftingProcess != null)
        {
            craftButtonText.text = DataManager.Instance.PlayerData.CraftingProcess.GetFinishTime();
        }
    }
}
