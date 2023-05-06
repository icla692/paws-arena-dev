using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class NFT
{
    public string imageUrl;
    public string furType;
    public List<string> ids;
    public Texture2D imageTex;

    private XmlDocument doc;

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
