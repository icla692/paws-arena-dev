using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChallengesManager : MonoBehaviour
{
    public static ChallengesManager Instance;

    private readonly int amountOfChallenges=3;

    private bool isInit;
    private bool isSubscribed;
    private List<ChallengeSO> allChallenges = new List<ChallengeSO>();

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            allChallenges = Resources.LoadAll<ChallengeSO>("Challenges").ToList();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        if (isInit)
        {
            return;
        }

        isInit = true;
        StartCoroutine(CheckForReset());
    }

    IEnumerator CheckForReset()
    {
        if (DateTime.UtcNow>DataManager.Instance.PlayerData.Challenges.NextReset)
        {
            GenerateNewChallenges();
        }
        else
        {
            SubscribeEvents();
        }

        yield return new WaitForSeconds(1);
    }

    void SubscribeEvents()
    {
        if (isSubscribed)
        {
            return;
        }

        isSubscribed = true;
        foreach (var _challengeData in DataManager.Instance.PlayerData.Challenges.ChallengesData)
        {
            if (_challengeData.Completed)
            {
                continue;
            }
            if (_challengeData.Id==0||_challengeData.Id==1||_challengeData.Id==2)
            {
                EventsManager.OnWonGame += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==3)
            {
                EventsManager.OnCraftedItem += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==4)
            {
                EventsManager.OnCraftedCrystal += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==5||_challengeData.Id==6||_challengeData.Id==7)
            {
                EventsManager.OnGotExperience += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==8||_challengeData.Id==9||_challengeData.Id==10)
            {
                EventsManager.OnWonLeaderboardPoints += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==11||_challengeData.Id==12||_challengeData.Id==13)
            {
                EventsManager.OnWonGameWithFullHp += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==14||_challengeData.Id==15||_challengeData.Id==16)
            {
                EventsManager.OnLostGame += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==17||_challengeData.Id==18||_challengeData.Id==19)
            {
                EventsManager.OnDealtDamageToOpponent += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==20||_challengeData.Id==21||_challengeData.Id==22)
            {
                EventsManager.OnUsedMilkBottle += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==23||_challengeData.Id==24||_challengeData.Id==25)
            {
                EventsManager.OnHealedKitty += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==26||_challengeData.Id==27||_challengeData.Id==28||_challengeData.Id==29)
            {
                EventsManager.OnPlayedMatch += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==30||_challengeData.Id==31||_challengeData.Id==32)
            {
                EventsManager.OnUsedRocket += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==33||_challengeData.Id==34||_challengeData.Id==35)
            {
                EventsManager.OnUsedCannon += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==36||_challengeData.Id==37||_challengeData.Id==38)
            {
                EventsManager.OnUsedTripleRocket += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==39||_challengeData.Id==40||_challengeData.Id==41)
            {
                EventsManager.OnUsedAirplane += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==42||_challengeData.Id==43||_challengeData.Id==44)
            {
                EventsManager.OnUsedMouse += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==45||_challengeData.Id==46||_challengeData.Id==47)
            {
                EventsManager.OnUsedArrow += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==48||_challengeData.Id==49||_challengeData.Id==50)
            {
                EventsManager.OnWonGame += _challengeData.IncreaseAmount;
                EventsManager.OnLostGame += _challengeData.Reset;
            }
            else if (_challengeData.Id==51)
            {
                EventsManager.OnWonWithHpLessThan10 += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==52)
            {
                EventsManager.OnWonWithHpLessThan20 += _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==53)
            {
                EventsManager.OnWonWithHpLessThan30 += _challengeData.IncreaseAmount;
            }

            _challengeData.IsSubscribed = true;
        }
    }

    void UnsubscribeEvents()
    {
        if (!isSubscribed)
        {
            return;
        }

        isSubscribed = false;
        foreach (var _challengeData in DataManager.Instance.PlayerData.Challenges.ChallengesData)
        {
            if (_challengeData.Completed)
            {
                continue;
            }
            if (_challengeData.Id==0||_challengeData.Id==1||_challengeData.Id==2)
            {
                EventsManager.OnWonGame -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==3)
            {
                EventsManager.OnCraftedItem -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==4)
            {
                EventsManager.OnCraftedCrystal -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==5||_challengeData.Id==6||_challengeData.Id==7)
            {
                EventsManager.OnGotExperience -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==8||_challengeData.Id==9||_challengeData.Id==10)
            {
                EventsManager.OnWonLeaderboardPoints -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==11||_challengeData.Id==12||_challengeData.Id==13)
            {
                EventsManager.OnWonGameWithFullHp -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==14||_challengeData.Id==15||_challengeData.Id==16)
            {
                EventsManager.OnLostGame -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==17||_challengeData.Id==18||_challengeData.Id==19)
            {
                EventsManager.OnDealtDamageToOpponent -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==20||_challengeData.Id==21||_challengeData.Id==22)
            {
                EventsManager.OnUsedMilkBottle -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==23||_challengeData.Id==24||_challengeData.Id==25)
            {
                EventsManager.OnHealedKitty -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==26||_challengeData.Id==27||_challengeData.Id==28||_challengeData.Id==29)
            {
                EventsManager.OnPlayedMatch -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==30||_challengeData.Id==31||_challengeData.Id==32)
            {
                EventsManager.OnUsedRocket -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==33||_challengeData.Id==34||_challengeData.Id==35)
            {
                EventsManager.OnUsedCannon -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==36||_challengeData.Id==37||_challengeData.Id==38)
            {
                EventsManager.OnUsedTripleRocket -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==39||_challengeData.Id==40||_challengeData.Id==41)
            {
                EventsManager.OnUsedAirplane -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==42||_challengeData.Id==43||_challengeData.Id==44)
            {
                EventsManager.OnUsedMouse -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==45||_challengeData.Id==46||_challengeData.Id==47)
            {
                EventsManager.OnUsedArrow -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==48||_challengeData.Id==49||_challengeData.Id==50)
            {
                EventsManager.OnWonGame -= _challengeData.IncreaseAmount;
                EventsManager.OnLostGame -= _challengeData.Reset;
            }
            else if (_challengeData.Id==51)
            {
                EventsManager.OnWonWithHpLessThan10 -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==52)
            {
                EventsManager.OnWonWithHpLessThan20 -= _challengeData.IncreaseAmount;
            }
            else if (_challengeData.Id==53)
            {
                EventsManager.OnWonWithHpLessThan30 -= _challengeData.IncreaseAmount;
            }

            _challengeData.IsSubscribed = false;
        }
    }

    public void GenerateNewChallenges()
    {
        UnsubscribeEvents();
        List<ChallengeSO> _allChallenges = allChallenges.ToList().OrderBy(_element => Guid.NewGuid()).ToList();
        DataManager.Instance.PlayerData.Challenges.ChallengesData = new List<ChallengeData>();
        for (int i = 0; i < amountOfChallenges; i++)
        {
            ChallengeData _challengeData = new ChallengeData()
            {
                Id = _allChallenges[i].Id,
                Completed = false,
                Value = 0
            };
            DataManager.Instance.PlayerData.Challenges.ChallengesData.Add(_challengeData);
        }

        DateTime _nextReset = DateTime.UtcNow.AddDays(1);

        DataManager.Instance.PlayerData.Challenges.ClaimedLuckySpin = false;
        DataManager.Instance.PlayerData.Challenges.NextReset =
            new DateTime(_nextReset.Year, _nextReset.Month, _nextReset.Day, 0, 0, 0);
        DataManager.Instance.SaveChallenges();
    }

    public void CompletedChallenge(ChallengeData _challengeData)
    {
        ChallengeSO _challengeSO = allChallenges.Find(_element => _element.Id == _challengeData.Id);
        switch (_challengeSO.RewardType)
        {
            case ChallengeRewardType.SeasonExperience:
                DataManager.Instance.PlayerData.Experience += _challengeSO.RewardAmount;
                break;
            case ChallengeRewardType.JugOfMilk:
                DataManager.Instance.PlayerData.JugOfMilk += _challengeSO.RewardAmount;
                break;
            case ChallengeRewardType.Snacks:
                DataManager.Instance.PlayerData.Snacks += _challengeSO.RewardAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        DataManager.Instance.SaveChallenges();
    }



    public ChallengeSO Get(int _id)
    {
        return allChallenges.Find(_element => _element.Id == _id);
    }
}
