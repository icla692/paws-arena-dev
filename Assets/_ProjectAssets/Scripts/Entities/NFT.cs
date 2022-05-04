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

    public async UniTask GrabImage()
    {
        XmlDocument doc = await NFTImageLoader.LoadSVGXML(imageUrl);
        imageTex = NFTImageLoader.LoadNFT(doc);
        furType = NFTImageLoader.GetFurType(doc);
    }
}
