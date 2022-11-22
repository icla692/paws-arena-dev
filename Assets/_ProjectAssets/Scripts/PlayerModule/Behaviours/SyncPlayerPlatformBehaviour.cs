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
        PUNRoomUtils.onPlayerJoined += OnPlayerJoined;
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            transform.position = SyncPlatformsBehaviour.Instance.myPos;
            playerCustomization.SetCat(GameState.selectedNFT.ids);

            photonView.RPC("SetCatStyle", RpcTarget.Others, GameState.selectedNFT.ids.ToArray());
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
        Player player = PhotonNetwork.PlayerList.First(player => player.UserId == userId);
        photonView.RPC("SetCatStyle", player, GameState.selectedNFT.ids.ToArray());
    }

    [PunRPC]
    public void SetCatStyle(string[] ids)
    {
        playerCustomization.SetCat(ids.ToList());
    }



}
