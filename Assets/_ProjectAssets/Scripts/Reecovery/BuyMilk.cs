using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyMilk : MonoBehaviour
{
    [SerializeField] Button doneButton;
    [SerializeField] Button buyJugOfMilkButton;
    [SerializeField] Button buyGlassOfMilkButton;

    [SerializeField] TextMeshProUGUI jugOfMilkDisplay;
    [SerializeField] TextMeshProUGUI glassOfMilkDisplay;

    [SerializeField] Color normalAmountColor;
    [SerializeField] Color zeroAmountColor;

    public void Setup()
    {
        jugOfMilkDisplay.text = ValuablesManager.Instance.JugOfMilk.ToString();
        jugOfMilkDisplay.color = ValuablesManager.Instance.JugOfMilk == 0 ? zeroAmountColor : normalAmountColor;

        glassOfMilkDisplay.text = ValuablesManager.Instance.GlassOfMilk.ToString();
        glassOfMilkDisplay.color = ValuablesManager.Instance.GlassOfMilk == 0 ? zeroAmountColor : normalAmountColor;

        doneButton.onClick.AddListener(Done);
        buyJugOfMilkButton.onClick.AddListener(BuyJugOfMilk);
        buyGlassOfMilkButton.onClick.AddListener(BuyGlassOfMIlk);

        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        doneButton.onClick.AddListener(Done);
        buyJugOfMilkButton.onClick.AddListener(BuyJugOfMilk);
        buyGlassOfMilkButton.onClick.AddListener(BuyGlassOfMIlk);
    }

    void BuyJugOfMilk()
    {
        //todo buy jug milk
    }

    void BuyGlassOfMIlk()
    {
        //todo buy glass of milk
    }

    void Done()
    {
        gameObject.SetActive(false);
    }
}
