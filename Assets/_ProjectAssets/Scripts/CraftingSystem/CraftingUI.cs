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
    [SerializeField] TextMeshProUGUI giftText;

    [SerializeField] Button commonButton;
    [SerializeField] Button uncommonButton;
    [SerializeField] Button rareButton;
    [SerializeField] Button epicButton;
    [SerializeField] Button legendaryButton;
    [SerializeField] Button craftItemButton;
    [SerializeField] TextMeshProUGUI itemAmountDisplay;

    //top frame
    [SerializeField] Image ingridiantImage;
    [SerializeField] TextMeshProUGUI craftText;
    [SerializeField] TextMeshProUGUI craftAmountDisplay;
    [SerializeField] Image endResultImage;
    [SerializeField] Button craftCrystalButton;
    [SerializeField] TextMeshProUGUI craftButtonText;

    CraftingRecepieSO showingRecepie;

    public void Setup()
    {
        ValuablesManager.Instance.UpdatedCommonCrystal += ShowCristals;
        ValuablesManager.Instance.UpdatedUncommonCrystal += ShowCristals;
        ValuablesManager.Instance.UpdatedRareCrystal += ShowCristals;
        ValuablesManager.Instance.UpdatedEpicCrystal += ShowCristals;
        ValuablesManager.Instance.UpdatedLegendaryCrystal += ShowCristals;
        CraftingProcess.Finished += FinishedCrafting;

        commonButton.onClick.AddListener(() => ShowRecepie(ItemType.Common));
        uncommonButton.onClick.AddListener(() => ShowRecepie(ItemType.Uncommon));
        rareButton.onClick.AddListener(() => ShowRecepie(ItemType.Rare));
        epicButton.onClick.AddListener(() => ShowRecepie(ItemType.Epic));
        craftCrystalButton.onClick.AddListener(CraftCrystal);
        craftItemButton.onClick.AddListener(CraftItem);

        ShowRecepie(ItemType.Common);
        ShowCristals();
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        ValuablesManager.Instance.UpdatedCommonCrystal -= ShowCristals;
        ValuablesManager.Instance.UpdatedUncommonCrystal -= ShowCristals;
        ValuablesManager.Instance.UpdatedRareCrystal -= ShowCristals;
        ValuablesManager.Instance.UpdatedEpicCrystal -= ShowCristals;
        ValuablesManager.Instance.UpdatedLegendaryCrystal -= ShowCristals;
        CraftingProcess.Finished -= FinishedCrafting;

        craftCrystalButton.onClick.RemoveListener(CraftCrystal);
        commonButton.onClick.RemoveAllListeners();
        uncommonButton.onClick.RemoveAllListeners();
        rareButton.onClick.RemoveAllListeners();
        epicButton.onClick.RemoveAllListeners();
        craftItemButton.onClick.RemoveListener(CraftItem);
    }

    void ShowCristals()
    {
        limeAmountDisplay.text = ValuablesManager.Instance.CommonCrystal.ToString();
        greenAmountDisplay.text = ValuablesManager.Instance.UncommonCrystal.ToString();
        blueAmountDisplay.text = ValuablesManager.Instance.RareCrystal.ToString();
        purpleAmountDisplay.text = ValuablesManager.Instance.EpicCrystal.ToString();
        orangeAmountDisplay.text = ValuablesManager.Instance.LegendaryCrystal.ToString();
        ShowBotFrame();
    }

    void ShowBotFrame()
    {
        CraftingRecepieSO _giftRecepie = CraftingRecepieSO.Get(ItemType.Lengedary);
        giftText.text = $"Combine {_giftRecepie.AmountNeeded} <color={_giftRecepie.IngridiantColor}>{_giftRecepie.Inggrdiant}</color> shards\nto get 1 <color={_giftRecepie.EndProductColor}>Legendary</color> item";
        if (ValuablesManager.Instance.LegendaryCrystal >= _giftRecepie.AmountNeeded)
        {
            itemAmountDisplay.text = $"<color=#00ff00>{ValuablesManager.Instance.LegendaryCrystal}</color>/<color={showingRecepie.EndProductColor}>{showingRecepie.AmountNeeded}</color>";
            craftItemButton.interactable = true;
        }
        else
        {
            itemAmountDisplay.text = $"<color=#ff0000>{ValuablesManager.Instance.LegendaryCrystal}</color>/<color={showingRecepie.EndProductColor}>{showingRecepie.AmountNeeded}</color>";
            craftItemButton.interactable = false;
        }
    }

    void ShowRecepie(ItemType _ingridiant)
    {
        showingRecepie = CraftingRecepieSO.Get(_ingridiant);

        ingridiantImage.sprite = showingRecepie.IngridiantSprite;
        craftText.text = $"Get 1 <color={showingRecepie.EndProductColor}>{showingRecepie.EndProduct}</color> sahrd by\ncombining {showingRecepie.AmountNeeded} <color={showingRecepie.IngridiantColor}>{showingRecepie.Inggrdiant}</color> shards";
        float _amountOfIngridiants;
        switch (showingRecepie.Inggrdiant)
        {
            case ItemType.Gift:
                _amountOfIngridiants = ValuablesManager.Instance.GiftItem;
                break;
            case ItemType.Common:
                _amountOfIngridiants = ValuablesManager.Instance.CommonCrystal;
                break;
            case ItemType.Uncommon:
                _amountOfIngridiants = ValuablesManager.Instance.UncommonCrystal;
                break;
            case ItemType.Rare:
                _amountOfIngridiants = ValuablesManager.Instance.RareCrystal;
                break;
            case ItemType.Epic:
                _amountOfIngridiants = ValuablesManager.Instance.EpicCrystal;
                break;
            case ItemType.Lengedary:
                _amountOfIngridiants = ValuablesManager.Instance.LegendaryCrystal;
                break;
            default:
                throw new System.Exception("Dont know how to check if player can craft: " + showingRecepie.Inggrdiant);
        }
        if (_amountOfIngridiants >= showingRecepie.AmountNeeded)
        {
            craftAmountDisplay.text = $"<color=#00ff00>{_amountOfIngridiants}</color>/<color={showingRecepie.EndProductColor}>{showingRecepie.AmountNeeded}</color>";
            craftCrystalButton.interactable = true;
        }
        else
        {
            craftAmountDisplay.text = $"<color=#ff0000>{_amountOfIngridiants}</color>/<color={showingRecepie.EndProductColor}>{showingRecepie.AmountNeeded}</color>";
            craftCrystalButton.interactable = false;
        }

        endResultImage.sprite = showingRecepie.EndProductSprite;
        ingridiantImage.SetNativeSize();
        endResultImage.SetNativeSize();
        craftButtonText.text = "Craft";

        if (ValuablesManager.Instance.CraftingProcess != null)
        {
            craftCrystalButton.interactable = false;
        }
    }

    void CraftCrystal()
    {
        //todo ask server if player can craft
        //if he can
        CraftingProcess _craftingProcess = new CraftingProcess();
        _craftingProcess.DateStarted = DateTime.UtcNow;
        _craftingProcess.Ingridiant = showingRecepie.Inggrdiant;

        ValuablesManager.Instance.CraftingProcess = _craftingProcess;

        switch (showingRecepie.Inggrdiant)
        {
            case ItemType.Gift:
                ValuablesManager.Instance.GiftItem -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Common:
                ValuablesManager.Instance.CommonCrystal -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Uncommon:
                ValuablesManager.Instance.UncommonCrystal -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Rare:
                ValuablesManager.Instance.RareCrystal -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Epic:
                ValuablesManager.Instance.EpicCrystal -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Lengedary:
                ValuablesManager.Instance.LegendaryCrystal -= showingRecepie.AmountNeeded;
                break;
            default:
                throw new System.Exception("Don't know how to start crafting process for ingridiant: " + showingRecepie.Inggrdiant);
        }

        ShowRecepie(showingRecepie.Inggrdiant);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void FinishedCrafting()
    {
        ShowCristals();
        ShowRecepie(showingRecepie.Inggrdiant);
        craftButtonText.text = "Craft";
    }

    void CraftItem()
    {
        //todo craft item
    }

    private void Update()
    {
        if (ValuablesManager.Instance.CraftingProcess != null)
        {
            craftButtonText.text = ValuablesManager.Instance.CraftingProcess.GetFinishTime();
        }
    }
}
