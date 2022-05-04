using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static string walletId;
    public static List<NFT> nfts { get; private set; }

    static GameState(){
        nfts = new List<NFT>();
    }

}
