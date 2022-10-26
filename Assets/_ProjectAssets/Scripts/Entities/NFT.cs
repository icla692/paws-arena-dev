using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class NFT
{
    public string imageUrl;
    public string furType;
    public Texture2D imageTex;

    private XmlDocument doc;

    public async UniTask GrabImage()
    {
        doc = await NFTImageLoader.LoadSVGXML(imageUrl);
        if (imageTex == null)
        {
            imageTex = NFTImageLoader.LoadNFT(doc);
        }
        if (furType == null)
        {
            furType = NFTImageLoader.GetFurType(doc);
        }
    }

    public void GetItems()
    {
    }
}
