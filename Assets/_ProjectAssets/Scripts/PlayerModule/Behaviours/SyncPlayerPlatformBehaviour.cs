using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SyncPlayerPlatformBehaviour : MonoBehaviour
{
    [SerializeField]
    private PlayerCustomization playerCustomization;

    [HideInInspector]
    public bool isBot = false;
    
    private PhotonView photonView;

    private void Awake()
    {
        transform.position = new Vector3(-100, 0, 0);
    }

    private async void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine && !isBot)
        {
            var config = playerCustomization.SetCat(GameState.selectedNFT.imageUrl, GameState.selectedNFT.ids);
            string serializedConfig = JsonUtility.ToJson(config.GetSerializableObject());

            photonView.RPC("SetCatStyle", RpcTarget.Others, GameState.selectedNFT.imageUrl, serializedConfig);

            PUNRoomUtils.onPlayerJoined += OnPlayerJoined;
        }

        if (isBot)
        {
            playerCustomization.wrapper.SetActive(false);
        }

        PlatformPose pose = SyncPlatformsBehaviour.Instance.GetMySeatPosition(photonView, isBot);
        transform.position = pose.pos;
        transform.localScale = pose.scale;

        if (isBot)
        {
            NFT nft = new NFT()
            {
                imageUrl = GameState.botInfo.kittyUrl
            };

            await nft.GrabImage();
            playerCustomization.wrapper.SetActive(true);
            playerCustomization.SetTransientCat(nft.imageUrl, nft.ids);
        }
    }

    private void OnDestroy()
    {
        PUNRoomUtils.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(string nickname, string userId)
    {
        Debug.Log($"!! On Player Joined !! {nickname}");
        Player player = PhotonNetwork.PlayerList.First(player => player.UserId == userId);
        string serializedConfig = JsonUtility.ToJson(KittiesCustomizationService.GetCustomization(GameState.selectedNFT.imageUrl).GetSerializableObject());
        photonView.RPC("SetCatStyle", player, GameState.selectedNFT.imageUrl, serializedConfig);
    }

    [PunRPC]
    public void SetCatStyle(string url, string configJson)
    {
        KittyCustomization customization = JsonUtility.FromJson<KittyCustomization.KittyCustomizationSerializable>(configJson).GetNonSerializable();
        playerCustomization.SetTransientCat(url, customization);
    }



}
