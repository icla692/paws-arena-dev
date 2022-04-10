using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    public PhotonManager photonManager;
    public LobbyUIManager lobbyUIManager;

    [Header("Internals")]
    public List<SeatGameobject> seats;

    private void OnEnable()
    {
        Init();
        PhotonManager.onPlayerJoined += OnPlayerJoined;
        PhotonManager.onPlayerLeft += OnPlayerLeft;
        PhotonManager.OnRoomLeft += OnRoomLeft;
    }

    private void OnDisable()
    {
        PhotonManager.onPlayerJoined -= OnPlayerJoined;
        PhotonManager.onPlayerLeft -= OnPlayerLeft;
        PhotonManager.OnRoomLeft -= OnRoomLeft;
    }

    private void Init()
    {
        foreach(SeatGameobject seat in seats)
        {
            FreeSeat(seat);
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            photonManager.AddPlayerCustomProperty("seat", "0");
            OccupySeat(seats[0], PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            List<Player> players = photonManager.GetOtherPlayers();
            if (players.Count == 1)
            {
                int otherPlayerSeat = Int32.Parse(players[0].CustomProperties["seat"].ToString());
                OccupySeat(seats[otherPlayerSeat], players[0].NickName);

                int mySeat = (otherPlayerSeat + 1) % 2;
                photonManager.AddPlayerCustomProperty("seat", "" + mySeat);
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
        photonManager.TryLeaveRoom();
    }

    private void OnRoomLeft()
    {
        lobbyUIManager.OpenCharacterSelectionScreen();
    }
}
