using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static string nickname = "Neinitialized Name";
    //NFT
    public static Action<NFT> onSelectedNFT;
    public static string walletId;
    public static List<NFT> nfts { get; private set; }
    public static NFT selectedNFT { get; private set; }

    //Settings
    public static GameSettings gameSettings;

    //Inter-scene needed data
    public static GameResolveState gameResolveState = GameResolveState.DRAW;

    static GameState(){
        nfts = new List<NFT>();
        gameSettings = GameSettings.Default();
    }


    public static void SetSelectedNFT(NFT nft)
    {
        selectedNFT = nft;
        onSelectedNFT?.Invoke(nft);
    }

}
