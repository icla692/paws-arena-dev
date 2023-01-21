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

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            transform.position = SyncPlatformsBehaviour.Instance.myPos;

            var config = playerCustomization.SetCat(GameState.selectedNFT.imageUrl, GameState.selectedNFT.ids);
            string serializedConfig = JsonUtility.ToJson(config.GetSerializableObject());

            photonView.RPC("SetCatStyle", RpcTarget.Others, GameState.selectedNFT.imageUrl, serializedConfig);

            PUNRoomUtils.onPlayerJoined += OnPlayerJoined;
        }
        else
        {
            transform.position = SyncPlatformsBehaviour.Instance.theirPos;
            transform.localScale = new Vector3(-1, 1, 1);
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
