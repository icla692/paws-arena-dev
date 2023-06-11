using UnityEngine;
using System;

[Serializable]
public class LevelRewardBase : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public LevelRewardType Type;
    [field: SerializeField] public bool IsPremium { get; private set; }


    public virtual void Claim()
    {

    }

}
