using Newtonsoft.Json;
using System;

[Serializable]
public class CraftingProcess
{
    public ItemType Ingridiant;
    public DateTime DateStarted;

    [JsonIgnore] public static Action Finished;

    public string GetFinishTime()
    {
        CraftingRecepieSO _recepie = CraftingRecepieSO.Get(Ingridiant);
        DateTime _endDate = DateStarted.AddSeconds(_recepie.FusionTime);

        TimeSpan _endTime = _endDate - DateTime.UtcNow;

        if (_endTime.TotalSeconds < 0)
        {
            EndProduction();
            return "Finished";
        }

        float _secounds = _endTime.Seconds;
        float _minutes = _endTime.Minutes;
        float _hours = _endTime.Hours;

        string _finishText = string.Empty;
        _finishText += _hours < 10 ? "0" + _hours : _hours;
        _finishText += ":";
        _finishText += _minutes < 10 ? "0" + _minutes : _minutes;
        _finishText += ":";
        _finishText += _secounds < 10 ? "0" + _secounds : _secounds;

        return _finishText;
    }

    void EndProduction()
    {
        //todo validate with server
        //if server validated
        CraftingRecepieSO _recepie = CraftingRecepieSO.Get(Ingridiant);
        DataManager.Instance.PlayerData.CraftingProcess = null;
        switch (_recepie.EndProduct)
        {
            case ItemType.Gift:
                DataManager.Instance.PlayerData.GiftItem++;
                break;
            case ItemType.Common:
                DataManager.Instance.PlayerData.CommonCrystal++;
                break;
            case ItemType.Uncommon:
                DataManager.Instance.PlayerData.UncommonCrystal++;
                break;
            case ItemType.Rare:
                DataManager.Instance.PlayerData.RareCrystal++;
                break;
            case ItemType.Epic:
                DataManager.Instance.PlayerData.EpicCrystal++;
                break;
            case ItemType.Lengedary:
                DataManager.Instance.PlayerData.LegendaryCrystal++;
                break;
            default:
                throw new Exception("Don't know how to handle end resultat in recepie for :" + _recepie.EndProduct);
        }
        Finished?.Invoke();
    }
}
