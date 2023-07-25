using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChallenge", menuName = "ScriptableObjects/Challenge")]
public class ChallengeSO : ScriptableObject
{
    public int Id;
    public string Description;
    public int AmountNeeded;
    public Sprite RewardSprite;
    public int RewardAmount;
    public ChallengeRewardType RewardType;
    public ChallengeCategory Category;

    public virtual void Setup(ChallengeData _data)
    {
        throw new Exception("Setup must be implemented");
    }

    public virtual void Subscribe()
    {
        throw new Exception("Subscribe must be implemented");
    }

    public virtual void Unsubscribe()
    {
        throw new Exception("Unsubscribe must be implemented");
    }

    public virtual void AddProgress<T>(T _progress)
    {
        throw new Exception("Add progress must be implemented");
    }
}
