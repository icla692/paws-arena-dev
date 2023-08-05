using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HasGuildPanel : GuildPanelBase
{
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI leaderNameDisplay;
    [SerializeField] private Image guildBadgeDisplay;
    [SerializeField] private Image kittyIconDisplay;
    [SerializeField] private TextMeshProUGUI membersDisplay;
    [SerializeField] private TextMeshProUGUI winsDisplay;
    [SerializeField] private GuildPlayerDisplay guildPlayerPrefab;
    [SerializeField] private Transform playersHolder;
    [SerializeField] private Button upArrow;
    [SerializeField] private Button downArrow;

    private List<GameObject> shownPlayers = new();
    private float moveAmount = 1;

    public override void Setup()
    {
        ShowGuildData();
        ClearShownPlayers();
        ShowPlayers();
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        upArrow.onClick.AddListener(MovePlayersUp);
        downArrow.onClick.AddListener(MovePlayersDown);
    }

    private void OnDisable()
    {
        upArrow.onClick.RemoveListener(MovePlayersUp);
        downArrow.onClick.RemoveListener(MovePlayersDown);
    }

    private void ShowGuildData()
    {
        GuildData _guild = DataManager.Instance.PlayerData.Guild;
        GuildSO _guildSO = GuildSO.Get(_guild.FlagId);
        nameDisplay.text = _guild.Name;
        leaderNameDisplay.text = _guild.Players.Find(_element => _element.IsLeader).Name;
        guildBadgeDisplay.sprite = _guildSO.Badge;
        kittyIconDisplay.sprite = _guildSO.Kitty;
        winsDisplay.text = "Matches won:"+_guild.MatchesWon;
        membersDisplay.text = $"Members: {_guild.Players.Count}/{DataManager.Instance.GameData.GuildMaxPlayers}";
    }

    private void ClearShownPlayers()
    {
        foreach (var _player in shownPlayers)
        {
            Destroy(_player);
        }
        
        shownPlayers.Clear();
    }

    private void ShowPlayers()
    {
        foreach (var _player in DataManager.Instance.PlayerData.Guild.Players)
        {
            GuildPlayerDisplay _playerDisplay = Instantiate(guildPlayerPrefab, playersHolder);
            _playerDisplay.Setup(_player);
            shownPlayers.Add(_playerDisplay.gameObject);
        }
    }

    private void MovePlayersUp()
    {
        Vector3 _itemsPosition = playersHolder.transform.position;
        _itemsPosition.y -= moveAmount;
        playersHolder.transform.position = _itemsPosition;
    }

    private void MovePlayersDown()
    {
        Vector3 _itemsPosition = playersHolder.transform.position;
        _itemsPosition.y += moveAmount;
        playersHolder.transform.position = _itemsPosition;
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
