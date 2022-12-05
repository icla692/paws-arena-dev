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
//#if UNITY_WEBGL && !UNITY_EDITOR
//            ConnectWallet();
//#else
        await UniTask.Delay(1000);
        WalletConnected();

        await UniTask.Delay(1000);
        //ProvideNFTs("{ \"nfts\":[{ \"url\":\"https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/txxtf-bqkor-uwiaa-aaaaa-cqace-eaqca-aac5q-q\"}]}");

        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/jjzf6-5ikor-uwiaa-aaaaa-cqace-eaqca-aadai-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/ymih6-7akor-uwiaa-aaaaa-cqace-eaqca-aaeow-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/u62g2-kikor-uwiaa-aaaaa-cqace-eaqca-aacuw-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/xtc2f-jqkor-uwiaa-aaaaa-cqace-eaqca-aacd3-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/ry5ro-5ikor-uwiaa-aaaaa-cqace-eaqca-aaeb3-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/or7s5-qqkor-uwiaa-aaaaa-cqace-eaqca-aaaux-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/w6e26-xqkor-uwiaa-aaaaa-cqace-eaqca-aadoy-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/urzol-uikor-uwiaa-aaaaa-cqace-eaqca-aacaq-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/awrxn-lykor-uwiaa-aaaaa-cqace-eaqca-aacca-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/or3jr-dqkor-uwiaa-aaaaa-cqace-eaqca-aabec-a" });

        onNFTsReceived?.Invoke();
//#endif
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
