using UnityEngine;
using TMPro;

public class CrystalsTotalDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    private void OnEnable()
    {
        DataManager.Instance.PlayerData.UpdatedCommonCrystal += Show;
        DataManager.Instance.PlayerData.UpdatedUncommonCrystal += Show;
        DataManager.Instance.PlayerData.UpdatedRareCrystal += Show;
        DataManager.Instance.PlayerData.UpdatedLegendaryCrystal += Show;
        DataManager.Instance.PlayerData.UpdatedEpicCrystal += Show;

        Show();
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.UpdatedCommonCrystal -= Show;
        DataManager.Instance.PlayerData.UpdatedUncommonCrystal -= Show;
        DataManager.Instance.PlayerData.UpdatedRareCrystal -= Show;
        DataManager.Instance.PlayerData.UpdatedLegendaryCrystal -= Show;
        DataManager.Instance.PlayerData.UpdatedEpicCrystal -= Show;
    }

    void Show()
    {
        display.text = DataManager.Instance.PlayerData.TotalCrystalsAmount.ToString();
    }
}
