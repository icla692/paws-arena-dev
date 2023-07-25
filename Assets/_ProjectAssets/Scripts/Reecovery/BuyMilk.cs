using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BuyMilk : MonoBehaviour
{
    [SerializeField] Button doneButton;
    [SerializeField] Button buyJugOfMilkButton;
    [SerializeField] Button buyGlassOfMilkButton;

    [SerializeField] TextMeshProUGUI jugOfMilkDisplay;
    [SerializeField] TextMeshProUGUI glassOfMilkDisplay;

    [SerializeField] Color normalAmountColor;
    [SerializeField] Color zeroAmountColor;

    [SerializeField] TextMeshProUGUI glassOfMilkPriceDisplay;
    [SerializeField] TextMeshProUGUI jugOfMilkPriceDisplay;

    public void Setup()
    {
        ShowGlassOfMilk();
        ShowJugOfMilk();

        doneButton.onClick.AddListener(Done);
        buyJugOfMilkButton.onClick.AddListener(BuyJugOfMilk);
        buyGlassOfMilkButton.onClick.AddListener(BuyGlassOfMIlk);

        DataManager.Instance.PlayerData.UpdatedJugOfMilk += ShowJugOfMilk;
        DataManager.Instance.PlayerData.UpdatedGlassOfMilk += ShowGlassOfMilk;

        glassOfMilkPriceDisplay.text = DataManager.Instance.GameData.GlassOfMilkPrice.ToString();
        jugOfMilkPriceDisplay.text = DataManager.Instance.GameData.JugOfMilkPrice.ToString();

        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        doneButton.onClick.AddListener(Done);
        buyJugOfMilkButton.onClick.AddListener(BuyJugOfMilk);
        buyGlassOfMilkButton.onClick.AddListener(BuyGlassOfMIlk);

        DataManager.Instance.PlayerData.UpdatedJugOfMilk -= ShowJugOfMilk;
        DataManager.Instance.PlayerData.UpdatedGlassOfMilk -= ShowGlassOfMilk;
    }

    void ShowJugOfMilk()
    {
        jugOfMilkDisplay.text = DataManager.Instance.PlayerData.JugOfMilk.ToString();
        jugOfMilkDisplay.color = DataManager.Instance.PlayerData.JugOfMilk == 0 ? zeroAmountColor : normalAmountColor;
    }

    void ShowGlassOfMilk()
    {
        glassOfMilkDisplay.text = DataManager.Instance.PlayerData.GlassOfMilk.ToString();
        glassOfMilkDisplay.color = DataManager.Instance.PlayerData.GlassOfMilk == 0 ? zeroAmountColor : normalAmountColor;
    }

    void BuyJugOfMilk()
    {
        StartCoroutine(BuyCooldown());
        if (DataManager.Instance.PlayerData.Snacks<DataManager.Instance.GameData.JugOfMilkPrice)
        {
            return;
        }

        DataManager.Instance.PlayerData.Snacks -= DataManager.Instance.GameData.JugOfMilkPrice;
        DataManager.Instance.PlayerData.JugOfMilk++;
    }

    void BuyGlassOfMIlk()
    {
        StartCoroutine(BuyCooldown());
        if (DataManager.Instance.PlayerData.Snacks< DataManager.Instance.GameData.GlassOfMilkPrice)
        {
            return;
        }

        DataManager.Instance.PlayerData.Snacks -= DataManager.Instance.GameData.GlassOfMilkPrice;
        DataManager.Instance.PlayerData.GlassOfMilk++;
    }

    void Done()
    {
        gameObject.SetActive(false);
    }

    IEnumerator BuyCooldown()
    {
        buyJugOfMilkButton.interactable = false;
        buyGlassOfMilkButton.interactable = false;
        yield return new WaitForSeconds(1);
        buyJugOfMilkButton.interactable = true;
        buyGlassOfMilkButton.interactable = true;
    }
}
