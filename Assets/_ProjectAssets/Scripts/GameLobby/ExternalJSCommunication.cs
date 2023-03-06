using Anura.ConfigurationModule.Managers;
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

    public void TryConnectWallet()
    {

        if (ConfigurationManager.Instance.GameConfig.env == GameEnvironment.DEV)
        {
            MockConnectWallet();
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
            ConnectWallet();
#else
        MockConnectWallet();
#endif
    }


    private async void MockConnectWallet()
    {
        await UniTask.Delay(1000);
        WalletConnected();

        await UniTask.Delay(1000);

        GameState.principalId = "u4s77-qtma7-sriuf-r7rzc-d2new-penyr-qhaap-z3lrx-b2u7e-d4wmv-gqe-dev-dev";

        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?&tokenid=hvtag-6ykor-uwiaa-aaaaa-cqace-eaqca-aaabd-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/jairp-5ykor-uwiaa-aaaaa-cqace-eaqca-aacdq-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/4gya5-nakor-uwiaa-aaaaa-cqace-eaqca-aaabu-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/jxrpt-fikor-uwiaa-aaaaa-cqace-eaqca-aaajx-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/hs6xe-7ikor-uwiaa-aaaaa-cqace-eaqca-aaaad-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/ivtko-hikor-uwiaa-aaaaa-cqace-eaqca-aaaql-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/xtmbl-nqkor-uwiaa-aaaaa-cqace-eaqca-aaacj-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/czfvb-oakor-uwiaa-aaaaa-cqace-eaqca-aaand-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/hs6xe-7ikor-uwiaa-aaaaa-cqace-eaqca-aaaad-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/ulm6p-xqkor-uwiaa-aaaaa-cqace-eaqca-aaaaa-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/umbjn-wakor-uwiaa-aaaaa-cqace-eaqca-aaaba-q" });
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
    }

    [ContextMenu("Connect Wallet")]
    public void WalletConnected()
    {
        onWalletConnected?.Invoke();
    }

    public void ProvidePrincipalId(string principalId)
    {
        GameState.principalId = principalId;
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
