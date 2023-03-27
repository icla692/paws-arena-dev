using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SeatGameobject
{
    [SerializeField]
    public TextMeshProUGUI occupierNickname;
}
public class GameMatchingScreen : MonoBehaviour
{
    [Header("Managers")]
    public PUNRoomUtils punRoomUtils;
    public SyncPlatformsBehaviour syncPlatformsBehaviour;

    [Header("Internals")]
    public GameObject notices;
    public List<SeatGameobject> seats;
    public Countdown countdown;

    private void OnEnable()
    {
        Init();
        PUNRoomUtils.onPlayerJoined += OnPlayerJoined;
        PUNRoomUtils.onPlayerLeft += OnPlayerLeft;
    }

    private void OnDisable()
    {
        PUNRoomUtils.onPlayerJoined -= OnPlayerJoined;
        PUNRoomUtils.onPlayerLeft -= OnPlayerLeft;
    }

    private void Init()
    {
        notices.SetActive(false);

        SetSeats();

        //Setting up Room
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int mapIdx = UnityEngine.Random.Range(0, 7);
            punRoomUtils.AddRoomCustomProperty("mapIdx", mapIdx);

            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                StartGame();
            }
        }
        else
        {
            List<Player> players = punRoomUtils.GetOtherPlayers();
            if (players.Count == 1)
            {
                notices.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"PUN: Inconsistency! There are {players.Count} players in room??");
            }
        }

        StartCoroutine(BringBotAfterSeconds(UnityEngine.Random.Range(10, 20)));
    }

    public void SetSeats()
    {
        foreach (SeatGameobject seat in seats)
        {
            FreeSeat(seat);
        }

        List<Player> players = punRoomUtils.GetOtherPlayers();
        if (players.Count == 1)
        {
            int otherPlayerSeat = Int32.Parse(players[0].CustomProperties["seat"].ToString());
            OccupySeat(seats[otherPlayerSeat], players[0].NickName);

            int mySeat = (otherPlayerSeat + 1) % 2;
            OccupySeat(seats[mySeat], PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            OccupySeat(seats[0], PhotonNetwork.LocalPlayer.NickName);
        }

    }

    private IEnumerator BringBotAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            BringBot();
        }
    }

    [ContextMenu("Bring Bot")]
    public async void BringBot()
    {
        var resp = await NetworkManager.GETRequestCoroutine("/user/player2", (code, err) => { }, true);

        if (ConfigurationManager.Instance.GameConfig.enableDevLogs)
        {
            Debug.Log("[HTTP][Player2]" + resp);
        }

        BotInformation botInformation = JsonUtility.FromJson<BotInformation>(resp);

        GameState.botInfo = botInformation;
        PhotonNetwork.CurrentRoom.MaxPlayers = 1;
        OccupySeat(seats[1], botInformation.nickname);
        syncPlatformsBehaviour.InstantiateBot();
        StartSinglePlayerGame();
    }

    private void OccupySeat(SeatGameobject seat, string nickName)
    {
        seat.occupierNickname.text = nickName;

    }

    private void FreeSeat(SeatGameobject seat)
    {
        seat.occupierNickname.text = "-";
    }

    private void OnPlayerJoined(string opponentNickname, string userId)
    {
        int mySeat = Int32.Parse(PhotonNetwork.LocalPlayer.CustomProperties["seat"].ToString());
        int otherSeat = (mySeat + 1) % 2;
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            notices.SetActive(true);

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    StartGame();
                }
            }
        }
        OccupySeat(seats[otherSeat], opponentNickname);
    }

    private void OnPlayerLeft()
    {
        int mySeat = Int32.Parse(PhotonNetwork.LocalPlayer.CustomProperties["seat"].ToString());
        int otherSeat = (mySeat + 1) % 2;
        notices.SetActive(false);
        FreeSeat(seats[otherSeat]);
        MakeRoomVisible();
    }

    public void TryExitRoom()
    {
        punRoomUtils.TryLeaveRoom();
    }

    public void StartGame()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        GetComponent<PhotonView>().RPC("StartGameRoutine", RpcTarget.All);
    }

    public void StartSinglePlayerGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = PhotonNetwork.CurrentRoom.IsVisible = false;
        GetComponent<PhotonView>().RPC("StartSinglePlayerGameRoutine", RpcTarget.All);
    }

    private void MakeRoomVisible()
    {
        PhotonNetwork.CurrentRoom.IsOpen = PhotonNetwork.CurrentRoom.IsVisible = true;
    }


    [PunRPC]
    public void StartGameRoutine()
    {
        StartCountdown("GameScene");
    }
    [PunRPC]
    public void StartSinglePlayerGameRoutine()
    {
        StartCountdown("PlayerTest_new");
    }

    private void StartCountdown(string sceneName)
    {
        PhotonNetwork.Loadl
        countdown.StartCountDown(() =>
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(sceneName);
            }
        });
    }
}
