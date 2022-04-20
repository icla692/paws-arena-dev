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
    public GameObject freeSeatParent;

    [SerializeField]
    public GameObject occupiedSeatParent;

    [SerializeField]
    public TextMeshProUGUI occupierNickname;
}
public class GameMatchingScreen : MonoBehaviour
{
    [Header("Managers")]
    public PUNRoomUtils punRoomUtils;

    [Header("Internals")]
    public GameObject startButton;
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
        startButton.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);

        foreach(SeatGameobject seat in seats)
        {
            FreeSeat(seat);
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            punRoomUtils.AddPlayerCustomProperty("seat", "0");
            OccupySeat(seats[0], PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            List<Player> players = punRoomUtils.GetOtherPlayers();
            if (players.Count == 1)
            {
                int otherPlayerSeat = Int32.Parse(players[0].CustomProperties["seat"].ToString());
                OccupySeat(seats[otherPlayerSeat], players[0].NickName);

                int mySeat = (otherPlayerSeat + 1) % 2;
                punRoomUtils.AddPlayerCustomProperty("seat", "" + mySeat);
                OccupySeat(seats[mySeat], PhotonNetwork.LocalPlayer.NickName);
            }
            else
            {
                Debug.LogWarning($"PUN: Inconsistency! There are {players.Count} players in room??");
            }
        }

    }

    private void OccupySeat(SeatGameobject seat, string nickName)
    {
        seat.freeSeatParent.SetActive(false);
        seat.occupiedSeatParent.SetActive(true);
        seat.occupierNickname.text = nickName;

    }

    private void FreeSeat(SeatGameobject seat)
    {
        seat.freeSeatParent.SetActive(true);
        seat.occupiedSeatParent.SetActive(false);
        seat.occupierNickname.text = "000000";
    }

    private void OnPlayerJoined(string opponentNickname)
    {
        int mySeat = Int32.Parse(PhotonNetwork.LocalPlayer.CustomProperties["seat"].ToString());
        int otherSeat = (mySeat + 1) % 2;
        OccupySeat(seats[otherSeat], opponentNickname);
    }

    private void OnPlayerLeft()
    {
        int mySeat = Int32.Parse(PhotonNetwork.LocalPlayer.CustomProperties["seat"].ToString());
        int otherSeat = (mySeat + 1) % 2;
        FreeSeat(seats[otherSeat]);
    }

    public void TryExitRoom()
    {
        punRoomUtils.TryLeaveRoom();
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        countdown.StartCountDown(() => {
            PhotonNetwork.LoadLevel("GameScene");
        });
    }
}
