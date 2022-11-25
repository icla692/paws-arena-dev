using Anura.Templates.MonoSingleton;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
class NFTsPayload
{
    [SerializeField]
    public List<NFTPayload> nfts;
}

[System.Serializable]
class NFTPayload
{
    [SerializeField]
    public string url;
}

public class ExternalJSCommunication : MonoSingleton<ExternalJSCommunication>
{
    public event Action onWalletConnected;
    public event Action onNFTsReceived;

    [DllImport("__Internal")]
    private static extern void ConnectWallet();

    public async void TryConnectWallet()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            ConnectWallet();
#else
        await UniTask.Delay(1000);
        WalletConnected();

        await UniTask.Delay(1000);
        ProvideNFTs("{ \"nfts\":[{ \"url\":\"https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/txxtf-bqkor-uwiaa-aaaaa-cqace-eaqca-aac5q-q\"}]}");
#endif
    }

    [ContextMenu("Connect Wallet")]
    public void WalletConnected()
    {
        onWalletConnected?.Invoke();
    }

    public void ProvideNFTs(string nftsString)
    {
        GameState.nfts.Clear();

        NFTsPayload payload = JsonUtility.FromJson<NFTsPayload>(nftsString);
        foreach(NFTPayload nft in payload.nfts)
        {
            GameState.nfts.Add(new NFT() { imageUrl = nft.url });
        }

        onNFTsReceived?.Invoke();
    }
}
