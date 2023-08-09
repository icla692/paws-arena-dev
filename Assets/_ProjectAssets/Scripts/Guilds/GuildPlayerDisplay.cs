using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GuildPlayerDisplay : MonoBehaviour
{
    public static Action<GuildPlayerData> OnKickPlayer;
    [SerializeField] private TextMeshProUGUI placeDisplay;
    [SerializeField] private GameObject leaderIcon;
    [SerializeField] private TextMeshProUGUI levelDisplay;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI pointsDisplay;
    [SerializeField] private Button kickButton;
    private GuildPlayerData playerData;

    public GuildPlayerData PlayerData => playerData;
    
    
    public void Setup(GuildPlayerData _playerData)
    {
        placeDisplay.text = _playerData.Place + ".";
        leaderIcon.SetActive(_playerData.IsLeader);
        levelDisplay.text = _playerData.Level.ToString();
        nameDisplay.text = _playerData.Name;
        pointsDisplay.text = _playerData.Points.ToString();
        kickButton.gameObject.SetActive(_playerData.IsLeader);
        if (_playerData.Id==DataManager.Instance.PlayerData.PlayerId)
        {
            kickButton.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        kickButton.onClick.AddListener(KickPlayer);
    }

    private void OnDisable()
    {
        kickButton.onClick.RemoveListener(KickPlayer);
    }

    private void KickPlayer()
    {
        OnKickPlayer?.Invoke(playerData);
    }
}
