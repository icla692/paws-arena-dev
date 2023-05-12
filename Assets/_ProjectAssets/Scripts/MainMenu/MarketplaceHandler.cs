using UnityEngine;
using UnityEngine.UI;

public class MarketplaceHandler : MonoBehaviour
{
    const string MARKETPLACE_URL_KEY = "https://toniq.io/marketplace/ickitties";
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(GoToMarketplace);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GoToMarketplace);
    }

    void GoToMarketplace()
    {
        Application.OpenURL(MARKETPLACE_URL_KEY);
    }
}
