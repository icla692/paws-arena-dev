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
    CraftingRecepieSO showingRecepie;

    public void Setup()
    {
        DataManager.Instance.PlayerData.UpdatedCommonCrystal += ShowCristals;
        DataManager.Instance.PlayerData.UpdatedUncommonCrystal += ShowCristals;
        DataManager.Instance.PlayerData.UpdatedRareCrystal += ShowCristals;
        DataManager.Instance.PlayerData.UpdatedEpicCrystal += ShowCristals;
        DataManager.Instance.PlayerData.UpdatedLegendaryCrystal += ShowCristals;
        CraftingProcess.Finished += FinishedCrafting;

        commonButton.onClick.AddListener(() => ShowRecepie(ItemType.Common));
        uncommonButton.onClick.AddListener(() => ShowRecepie(ItemType.Uncommon));
        rareButton.onClick.AddListener(() => ShowRecepie(ItemType.Rare));
        epicButton.onClick.AddListener(() => ShowRecepie(ItemType.Epic));
        legendaryButton.onClick.AddListener(() => ShowRecepie(ItemType.Lengedary));
        craftCrystalButton.onClick.AddListener(CraftCrystal);
        botCraftItemButton.onClick.AddListener(CraftItem);

        ShowRecepie(ItemType.Common);
        ShowCristals();
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.UpdatedCommonCrystal -= ShowCristals;
        DataManager.Instance.PlayerData.UpdatedUncommonCrystal -= ShowCristals;
        DataManager.Instance.PlayerData.UpdatedRareCrystal -= ShowCristals;
        DataManager.Instance.PlayerData.UpdatedEpicCrystal -= ShowCristals;
        DataManager.Instance.PlayerData.UpdatedLegendaryCrystal -= ShowCristals;
        CraftingProcess.Finished -= FinishedCrafting;

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
        limeAmountDisplay.text = DataManager.Instance.PlayerData.CommonCrystal.ToString();
        greenAmountDisplay.text = DataManager.Instance.PlayerData.UncommonCrystal.ToString();
        blueAmountDisplay.text = DataManager.Instance.PlayerData.RareCrystal.ToString();
        purpleAmountDisplay.text = DataManager.Instance.PlayerData.EpicCrystal.ToString();
        orangeAmountDisplay.text = DataManager.Instance.PlayerData.LegendaryCrystal.ToString();
        ShowRecepie(showingRecepie.Inggrdiant);
    }

    void ShowRecepie(ItemType _ingridiant)
    {
        showingRecepie = CraftingRecepieSO.Get(_ingridiant);

        if (_ingridiant == ItemType.Lengedary)
        {
            ShowBotFrame(_ingridiant);
            topHolder.SetActive(false);
            return;
        }
        else
        {
            topHolder.SetActive(true);
        }

        ingridiantImage.sprite = showingRecepie.IngridiantSprite;
        craftText.text = $"Get 1 <color={showingRecepie.EndProductColor}>{showingRecepie.EndProduct}</color> sahrd by\ncombining {showingRecepie.AmountNeeded} <color={showingRecepie.IngridiantColor}>{showingRecepie.Inggrdiant}</color> shards";
        float _amountOfIngridiants;
        switch (showingRecepie.Inggrdiant)
        {
            case ItemType.Gift:
                _amountOfIngridiants = DataManager.Instance.PlayerData.GiftItem;
                break;
            case ItemType.Common:
                _amountOfIngridiants = DataManager.Instance.PlayerData.CommonCrystal;
                break;
            case ItemType.Uncommon:
                _amountOfIngridiants = DataManager.Instance.PlayerData.UncommonCrystal;
                break;
            case ItemType.Rare:
                _amountOfIngridiants = DataManager.Instance.PlayerData.RareCrystal;
                break;
            case ItemType.Epic:
                _amountOfIngridiants = DataManager.Instance.PlayerData.EpicCrystal;
                break;
            case ItemType.Lengedary:
                _amountOfIngridiants = DataManager.Instance.PlayerData.LegendaryCrystal;
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
                _amountGot = DataManager.Instance.PlayerData.CommonCrystal;
                break;
            case ItemType.Uncommon:
                _amountGot = DataManager.Instance.PlayerData.UncommonCrystal;
                break;
            case ItemType.Rare:
                _amountGot = DataManager.Instance.PlayerData.RareCrystal;
                break;
            case ItemType.Epic:
                _amountGot = DataManager.Instance.PlayerData.EpicCrystal;
                break;
            case ItemType.Lengedary:
                _amountGot = DataManager.Instance.PlayerData.LegendaryCrystal;
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
        //todo ask server if player can craft
        //if he can
        CraftingProcess _craftingProcess = new CraftingProcess();
        _craftingProcess.DateStarted = DateTime.UtcNow;
        _craftingProcess.Ingridiant = showingRecepie.Inggrdiant;

        DataManager.Instance.PlayerData.CraftingProcess = _craftingProcess;

        switch (showingRecepie.Inggrdiant)
        {
            case ItemType.Gift:
                DataManager.Instance.PlayerData.GiftItem -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Common:
                DataManager.Instance.PlayerData.CommonCrystal -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Uncommon:
                DataManager.Instance.PlayerData.UncommonCrystal -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Rare:
                DataManager.Instance.PlayerData.RareCrystal -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Epic:
                DataManager.Instance.PlayerData.EpicCrystal -= showingRecepie.AmountNeeded;
                break;
            case ItemType.Lengedary:
                DataManager.Instance.PlayerData.LegendaryCrystal -= showingRecepie.AmountNeeded;
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
        ShowItem();
        //todo craft item
        Debug.Log("Should craft item :)");
        switch (showingRecepie.Inggrdiant)
        {
            case ItemType.Common:
                DataManager.Instance.PlayerData.CommonCrystal -= showingRecepie.BotAmountNeeded;
                break;
            case ItemType.Uncommon:
                DataManager.Instance.PlayerData.UncommonCrystal -= showingRecepie.BotAmountNeeded;
                break;
            case ItemType.Rare:
                DataManager.Instance.PlayerData.RareCrystal -= showingRecepie.BotAmountNeeded;
                break;
            case ItemType.Epic:
                DataManager.Instance.PlayerData.EpicCrystal -= showingRecepie.BotAmountNeeded;
                break;
            case ItemType.Lengedary:
                DataManager.Instance.PlayerData.LegendaryCrystal -= showingRecepie.BotAmountNeeded;
                break;
            default:
                throw new Exception("Don't know how to craft item for: " + showingRecepie.Inggrdiant);
        }
    }

    async void ShowItem()
    {
        //todo ask server for the item
        //todo delete this random item generator
        Sprite _rewardItem = equipments.eyes[UnityEngine.Random.Range(0, equipments.eyes.Count)];
        itemDisplay.Setup(_rewardItem);
        //todo add item to items list
    }

    private void Update()
    {
        if (DataManager.Instance.PlayerData.CraftingProcess != null)
        {
            craftButtonText.text = DataManager.Instance.PlayerData.CraftingProcess.GetFinishTime();
        }
    }
}
