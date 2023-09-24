using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuildBattlePanel : GuildPanelBase
{
    [SerializeField] private GameObject messageDisplay;
    [SerializeField] private GameObject holderDisplay;
    [SerializeField] private TextMeshProUGUI timerDisplay;
    [SerializeField] private EquipmentsConfig equipmentsConfig;
    [SerializeField] private Image rewardOnChestDisplay;
    [SerializeField] private GuildBattleGuildDisplay firstGuildDisplay;
    [SerializeField] private GuildBattleGuildDisplay secondGuildDisplay;
    [SerializeField] private Sprite guildBattleVictory;
    [SerializeField] private Sprite guildBattleDefeat;
    [SerializeField] private List<Image> guildBattleHistory;
    [SerializeField] private GuildPlayerDisplay playerPrefab;
    [SerializeField] private Transform playersHolder;
    [SerializeField] private Button showDown;
    [SerializeField] private Button showUp;
    [SerializeField] private GameObject chest;

    private int moveAmount = 1;
    private List<GameObject> shownPlayers = new();
    private bool isBattleActive;

    private void OnEnable()
    {
        showUp.onClick.AddListener(MoveContentUp);
        showDown.onClick.AddListener(MoveContentDown);
    }

    private void OnDisable()
    {
        showUp.onClick.RemoveListener(MoveContentUp);
        showDown.onClick.RemoveListener(MoveContentDown); 
    }

    private void MoveContentUp()
    {
        Vector3 _itemsPosition = playersHolder.transform.position;
        _itemsPosition.y -= moveAmount;
        playersHolder.transform.position = _itemsPosition;
    }
    
    private void MoveContentDown()
    {
        Vector3 _itemsPosition = playersHolder.transform.position;
        _itemsPosition.y += moveAmount;
        playersHolder.transform.position = _itemsPosition;
    }

    public override void Setup()
    {
        gameObject.SetActive(true);
        messageDisplay.SetActive(false);
        holderDisplay.SetActive(false);
        if (string.IsNullOrEmpty(DataManager.Instance.PlayerData.Guild.BattleId))
        {
            messageDisplay.SetActive(true);
            return;
        }

        isBattleActive = true;
        holderDisplay.SetActive(true);
        GuildBattle _guildBattle = DataManager.Instance.PlayerData.Guild.Battle;
        SetupGuildDisplay(_guildBattle.GuildOneId, firstGuildDisplay);
        SetupGuildDisplay(_guildBattle.GuildTwoId, secondGuildDisplay);
        SetupChest(_guildBattle);
        ShowBattleHistory();
        ShowParticipants();
    }

    private void SetupGuildDisplay(string _guildId, GuildBattleGuildDisplay _guildDisplay)
    {
        GuildData _guildData = null;
        foreach (var _guild in DataManager.Instance.GameData.Guilds.Values)
        {
            if (_guild.Id==_guildId)
            {
                _guildData = _guild;
                break;
            }
        }

        if (_guildData==null)
        {
            return;
        }
        
        GuildSO _guildSO = GuildSO.Get(_guildData.KittyId);

        if (_guildSO == null)
        {
            return;
        }
        _guildDisplay.Setup(_guildSO.Badges[_guildData.FlagIndex],_guildData.Name,_guildData.SumOfPoints.ToString(), _guildSO.KittyColor);
    }

    private void SetupChest(GuildBattle _guildBattle)
    {
        rewardOnChestDisplay.sprite = GetRewardSprite(_guildBattle.Rewards[0]);
        Vector3 _position = chest.transform.position;
        int _mover = 100;
        if (firstGuildDisplay.Score>secondGuildDisplay.Score)
        {
            _position.x -= _mover;
        }
        else if (secondGuildDisplay.Score>firstGuildDisplay.Score)
        {
            _position.x += _mover;
        }

        chest.transform.position = _position;
    }

    private void ShowBattleHistory()
    {
        foreach (var _historyDisplay in guildBattleHistory)
        {
            _historyDisplay.gameObject.SetActive(false);
        }

        for (var _index = 0; _index < DataManager.Instance.PlayerData.Guild.BattlesHistory.Count; _index++)
        {
            var _battleResult = DataManager.Instance.PlayerData.Guild.BattlesHistory[_index];
            guildBattleHistory[_index].sprite = _battleResult == 1 ? guildBattleVictory : guildBattleDefeat;
            guildBattleHistory[_index].gameObject.SetActive(true);
        }
    }

    private void ShowParticipants()
    {
        ClearParticipants();

        foreach (var _playerData in DataManager.Instance.PlayerData.Guild.Players.ToList().OrderBy(_player => _player
        .Points).ToList())
        {
            GuildPlayerDisplay _playerDisplay = Instantiate(playerPrefab, playersHolder);
            _playerDisplay.Setup(_playerData,false);
            shownPlayers.Add(_playerDisplay.gameObject);
        }
    }

    private void ClearParticipants()
    {
        foreach (var _player in shownPlayers)
        {
            Destroy(_player);
        }
        
        shownPlayers.Clear();
    }
    
    private void Update()
    {
        ShowTimer();
    }

    private void ShowTimer()
    {
        if (!isBattleActive)
        {
            timerDisplay.text = "Ended";
            return;
        }

        TimeSpan _difference = DataManager.Instance.PlayerData.Guild.Battle.EndDate - DateTime.Now;

        if (_difference.TotalSeconds<0)
        {
            isBattleActive = false;
            return;
        }

        timerDisplay.text = "Battle ends in:\n";
        timerDisplay.text += $"{_difference.Hours:D2}:{_difference.Minutes:D2}:{_difference.Seconds:D2}";
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    private Sprite GetRewardSprite(GuildBattleReward _reward)
    {
        GuildRewardSO _rewardSO = GuildRewardSO.Get(_reward.TypeId);

        switch (_rewardSO.Type)
        {
            case ItemType.Common:
                return _rewardSO.Sprite;
            case ItemType.Uncommon:
                return _rewardSO.Sprite;
            case ItemType.Rare:
                return _rewardSO.Sprite;
            case ItemType.Epic:
                return _rewardSO.Sprite;
            case ItemType.Legendary:
                return _rewardSO.Sprite;
            case ItemType.Item:
                EquipmentData _equipmentData = equipmentsConfig.GetEquipmentData(_reward.Amount);
                return _equipmentData.Thumbnail;
            case ItemType.GlassOfMilk:
                return _rewardSO.Sprite;
            case ItemType.JugOfMilk:
                return _rewardSO.Sprite;
            case ItemType.Cookie:
                return _rewardSO.Sprite;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
