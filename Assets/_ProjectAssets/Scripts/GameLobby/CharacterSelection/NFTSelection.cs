using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NFTSelection : MonoBehaviour
{
    public Transform nftGridParent;
    public TMPro.TextMeshProUGUI furType;

    private async void OnEnable()
    {
        await PopulateGrid();
    }

    private async UniTask PopulateGrid()
    {
        List<NFT> nfts = GameState.nfts;

        //Grab all images from internet
        List<UniTask> tasks = new List<UniTask>();
        foreach (NFT nft in nfts)
        {
            tasks.Add(nft.GrabImage());
        }
        await UniTask.WhenAll(tasks.ToArray());

        //Attach to images
        int idx = 0;
        foreach (NFT nft in nfts)
        {
            RawImage picture = nftGridParent.GetChild(idx).GetChild(0).GetComponent<RawImage>();
            picture.texture = nft.imageTex;
            furType.text = nft.furType;
            idx++;
        }
    }
}
