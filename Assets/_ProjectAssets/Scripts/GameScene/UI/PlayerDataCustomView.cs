using Anura.ConfigurationModule.Managers;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataCustomView : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI nicknameText;

    [SerializeField]
    private HealthUIBehaviour healthUIBehaviour;

    [SerializeField]
    private string parentPath;

    [SerializeField]
    private PhotonView photonview;

    void Start()
    {
        healthUIBehaviour.Init();
        if (photonview.IsMine)
        {
            PlayerManager.Instance.onHealthUpdated += OnHealthUpdated;
            photonview.RPC("SetNickname", RpcTarget.All, PhotonNetwork.NickName);
            OnHealthUpdated(ConfigurationManager.Instance.Config.GetPlayerTotalHealth());
        }

        Init();
    }

    public void Init()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.SetParent(GameObject.Find(parentPath).transform);
        int myseat = PUNGameRoomManager.Instance.GetMySeat();

        bool isMyPlayer = (myseat == 0 && photonview.IsMine || myseat == 1 && !photonview.IsMine);
        rt.anchorMin = rt.anchorMax = rt.pivot = isMyPlayer ? new Vector2(0, 1) : new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);

        //Mirror UI for right UI
        bool isPlayer2Data = (PhotonNetwork.LocalPlayer.IsMasterClient && !photonview.IsMine) || (!PhotonNetwork.LocalPlayer.IsMasterClient && photonview.IsMine);
        if (isPlayer2Data)
        {
            healthUIBehaviour.SetOrientationRight();
            nicknameText.alignment = TMPro.TextAlignmentOptions.Right;
        }

    }

    private void OnHealthUpdated(int val)
    {
        photonview.RPC("SetHealth", RpcTarget.All, val);
    }

    [PunRPC]
    public void SetNickname(string nickname)
    {
        nicknameText.text = nickname;
    }

    [PunRPC]
    public void SetHealth(int val)
    {
        healthUIBehaviour.OnHealthUpdated(val);
        if (!photonview.IsMine)
        {
            PlayerManager.Instance.otherPlayerHealth = val;
        }
    }
}
