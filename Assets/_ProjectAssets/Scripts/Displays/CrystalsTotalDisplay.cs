using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrystalsTotalDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private TextMeshProUGUI glowDisplay;
    [SerializeField] private Outline[] outlines;

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

    private void Show()
    {
        display.text = DataManager.Instance.PlayerData.Crystals.TotalCrystalsAmount.ToString();
        glowDisplay.text = DataManager.Instance.PlayerData.Crystals.TotalCrystalsAmount.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        glowDisplay.gameObject.SetActive(true);
        foreach (var _outline in outlines)
        {
            _outline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glowDisplay.gameObject.SetActive(false);
        foreach (var _outline in outlines)
        {
            _outline.enabled = false;
        }
    }
}
