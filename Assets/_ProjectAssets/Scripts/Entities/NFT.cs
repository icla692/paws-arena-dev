using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFT
{
    public string imageUrl;

    public Texture2D imageTex;

    public async UniTask GrabImage()
    {
        //Tex seems to be SVG :(
        //imageTex = await NetworkManager.GetImage(imageUrl);
    }
}
