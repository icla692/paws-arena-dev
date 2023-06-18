using UnityEngine;
using TMPro;

public class SnackDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI snackAmountDisplay;

    private void OnEnable()
    {
        DataManager.Instance.PlayerData.UpdatedSnacks += ShowSnacks;
        ShowSnacks();
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.UpdatedSnacks -= ShowSnacks;
    }

    void ShowSnacks()
    {
        snackAmountDisplay.text = DataManager.Instance.PlayerData.Snacks.ToString();
    }
}
