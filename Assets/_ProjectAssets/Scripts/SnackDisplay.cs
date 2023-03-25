using UnityEngine;
using TMPro;

public class SnackDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI snackAmountDisplay;

    private void OnEnable()
    {
        ValuablesManager.Instance.UpdatedSnacks += ShowSnacks;
        ShowSnacks();
    }

    private void OnDisable()
    {
        ValuablesManager.Instance.UpdatedSnacks -= ShowSnacks;
    }

    void ShowSnacks()
    {
        snackAmountDisplay.text = ValuablesManager.Instance.Snacks.ToString();
    }
}
