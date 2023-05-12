using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class NFT
{
    public string imageUrl;
    public string furType;
    public List<string> ids;
    public Texture2D imageTex;
    DateTime recoveryEndDate;
    private XmlDocument doc;

    public bool CanFight => RecoveryEndDate < DateTime.UtcNow;
    public int MinutesUntilHealed => (int)(RecoveryEndDate - DateTime.UtcNow).TotalMinutes;

    public Action UpdatedRecoveryTime;

    public DateTime RecoveryEndDate
    {
        get
        {
            return recoveryEndDate;
        }
        set
        {
            recoveryEndDate = value;
            UpdatedRecoveryTime?.Invoke();
        }
    }

    public async UniTask GrabImage()
    {
        doc = await NFTImageLoader.LoadSVGXML(imageUrl);
        if (imageTex == null)
        {
            //imageTex = NFTImageLoader.LoadNFT(doc);
            imageTex = NFTImageLoader.LoadNFTLocal(doc);
        }
        if (furType == null)
        {
            furType = NFTImageLoader.GetFurType(doc);
        }
        if (ids == null)
        {
            ids = NFTImageLoader.GetIds(doc);
        }
    }
}
