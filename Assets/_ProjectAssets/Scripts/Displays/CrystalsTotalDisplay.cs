using UnityEngine;
using TMPro;

public class CrystalsTotalDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    private void OnEnable()
    {
        DataManager.Instance.PlayerData.Crystals.UpdatedCommonCrystal += Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedUncommonCrystal += Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedRareCrystal += Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedLegendaryCrystal += Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedEpicCrystal += Show;

        Show();
    }

    private void OnDisable()
    {
        DataManager.Instance.PlayerData.Crystals.UpdatedCommonCrystal -= Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedUncommonCrystal -= Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedRareCrystal -= Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedLegendaryCrystal -= Show;
        DataManager.Instance.PlayerData.Crystals.UpdatedEpicCrystal -= Show;
    }

    void Show()
    {
        display.text = DataManager.Instance.PlayerData.Crystals.TotalCrystalsAmount.ToString();
    }
}
