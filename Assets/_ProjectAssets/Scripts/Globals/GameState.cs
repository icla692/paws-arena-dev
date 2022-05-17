using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static Action<NFT> onSelectedNFT;
    public static string walletId;
    public static List<NFT> nfts { get; private set; }
    public static NFT selectedNFT { get; private set; }

    static GameState(){
        nfts = new List<NFT>();
    }


    public static void SetSelectedNFT(NFT nft)
    {
        selectedNFT = nft;
        onSelectedNFT?.Invoke(nft);
    }

}
