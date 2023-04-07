using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecoveryDropDown : MonoBehaviour
{
    [SerializeField] GameObject healMessageHolder;
    [SerializeField] BuyMilk buyMilkPanel;
    [SerializeField] GameObject kittyIsFull;

    [SerializeField] Button healButton;
    [SerializeField] Button buyButton;

    [SerializeField] TextMeshProUGUI jugOfMilkDisplay;
    [SerializeField] TextMeshProUGUI glassOfMilkDisplay;

    [SerializeField] Color normalAmountColor;
    [SerializeField] Color zeroAmountColor;

    [SerializeField] Button jugOfMilkButton;
    [SerializeField] Button glassOfMilkButton;
    RecoveryOption recoveryOption;

    float animationLength = 0.1f;
    bool isOpen;

    private void OnEnable()
    {
        recoveryOption = RecoveryOption.JugOfMilk;
        jugOfMilkButton.onClick.AddListener(SelectJugOfMilk);
        glassOfMilkButton.onClick.AddListener(SelectGlassOfMilk);
        ShowSelecteRecoveryOption();
    }

    private void OnDisable()
    {
        jugOfMilkButton.onClick.RemoveListener(SelectJugOfMilk);
        glassOfMilkButton.onClick.RemoveListener(SelectGlassOfMilk);
    }

    void SelectJugOfMilk()
    {
        recoveryOption = RecoveryOption.JugOfMilk;
        ShowSelecteRecoveryOption();
    }

    void SelectGlassOfMilk()
    {
        recoveryOption = RecoveryOption.GlassOfMilk;
        ShowSelecteRecoveryOption();
    }

    void ShowSelecteRecoveryOption()
    {
        Image _jugOfMilkImage = jugOfMilkButton.GetComponent<Image>();
        Image _glassOfMilkImage = glassOfMilkButton.GetComponent<Image>();
        Color _jugOfMilkColor = _jugOfMilkImage.color;
        Color _glassOfMilkColor = _glassOfMilkImage.color;

        if (recoveryOption == RecoveryOption.JugOfMilk)
        {
            _jugOfMilkColor.a = 1;
            _glassOfMilkColor.a = 0;
        }
        else
        {
            _jugOfMilkColor.a = 0;
            _glassOfMilkColor.a = 1;
        }

        _jugOfMilkImage.color = _jugOfMilkColor;
        _glassOfMilkImage.color = _glassOfMilkColor;
    }

    private void Start()
    {
        Close();
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
        buyButton.onClick.RemoveListener(BuyMilk);
        healButton.onClick.RemoveListener(Heal);
    }

    public void Show()
    {
        gameObject.LeanScale(Vector3.one, animationLength);
        isOpen = true;
        buyButton.onClick.AddListener(BuyMilk);
        healButton.onClick.AddListener(Heal);

        jugOfMilkDisplay.text = ValuablesManager.Instance.JugOfMilk.ToString();
        jugOfMilkDisplay.color = ValuablesManager.Instance.JugOfMilk == 0 ? zeroAmountColor : normalAmountColor;

        glassOfMilkDisplay.text = ValuablesManager.Instance.GlassOfMilk.ToString();
        glassOfMilkDisplay.color = ValuablesManager.Instance.GlassOfMilk == 0 ? zeroAmountColor : normalAmountColor;
    }

    public void Heal()
    {
        if (GameState.selectedNFT.CanFight)
        {
            kittyIsFull.gameObject.SetActive(true);
            return;
        }

        if (recoveryOption == RecoveryOption.JugOfMilk)
        {
            if (ValuablesManager.Instance.JugOfMilk > 0)
            {
                ValuablesManager.Instance.JugOfMilk--;
                GameState.selectedNFT.RecoveryEndDate = DateTime.UtcNow;
                //TODO tell server that player used jug of milk to recover kittie
            }
            else
            {
                healMessageHolder.SetActive(true);
            }
        }
        else
        {
            if (ValuablesManager.Instance.GlassOfMilk > 0)
            {
                ValuablesManager.Instance.GlassOfMilk--;
                GameState.selectedNFT.RecoveryEndDate = GameState.selectedNFT.RecoveryEndDate.AddMinutes(15);
                //TODO tell server that player used glass of milk to recover kittie
            }
            else
            {
                healMessageHolder.SetActive(true);
            }
        }


        Close();
    }

    public void BuyMilk()
    {
        buyMilkPanel.Setup();
        Close();
    }
}
