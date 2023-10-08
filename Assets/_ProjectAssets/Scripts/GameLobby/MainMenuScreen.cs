using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    public TMPro.TextMeshProUGUI betCoins;
    public Transform playerPlatformPosition;
    public GameObject playerPlatformPrefab;

    private GameObject playerPlatform;

    private void OnEnable()
    {
        playerPlatform = GameObject.Instantiate(playerPlatformPrefab, playerPlatformPosition);
        playerPlatform.transform.position = Vector3.zero;
    }

    private void OnDisable()
    {
        if(playerPlatform != null)
        {
            Destroy(playerPlatform);
        }
    }


}
