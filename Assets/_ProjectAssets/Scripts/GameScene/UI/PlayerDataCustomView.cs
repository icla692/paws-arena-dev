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

    private bool isMultiplayer;

    void Start()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();

        healthUIBehaviour.Init();

        if (!isMultiplayer)
        {
            PlayerManager.Instance.onHealthUpdated += OnHealthUpdated;
            SetNickname("Test");
            OnHealthUpdated(ConfigurationManager.Instance.Config.GetPlayerTotalHealth());

        }
        else if (photonview.IsMine)
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
        rt.localScale = Vector3.one;

        int myseat = isMultiplayer ? PUNGameRoomManager.Instance.GetMySeat() : 0;

        bool isMyPlayer = (myseat == 0 && photonview.IsMine || myseat == 1 && !photonview.IsMine);
        rt.anchorMin = rt.anchorMax = rt.pivot = isMyPlayer ? new Vector2(0, 1) : new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);

        //Mirror UI for right UI
        bool isPlayer2Data = isMultiplayer && (PhotonNetwork.LocalPlayer.IsMasterClient && !photonview.IsMine) || (!PhotonNetwork.LocalPlayer.IsMasterClient && photonview.IsMine);
        if (isPlayer2Data)
        {
            healthUIBehaviour.SetOrientationRight();
            nicknameText.alignment = TMPro.TextAlignmentOptions.Right;
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-220, GetComponent<RectTransform>().anchoredPosition.y); 
        }

    }

    private void OnHealthUpdated(int val)
    {
        if (isMultiplayer)
        {
            photonview.RPC("SetHealth", RpcTarget.All, val);
        }
        else
        {
            SetHealth(val);
        }
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
