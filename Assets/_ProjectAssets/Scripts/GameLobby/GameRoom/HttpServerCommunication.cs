using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.colorfulcoding.GameRoom
{
    public class HttpServerCommunication : MonoBehaviour
    {
        private void Start()
        {
            RegisterStartOfTheMatch();
        }

        private async void RegisterStartOfTheMatch()
        {
            Debug.Log($"RoomId: {PhotonNetwork.CurrentRoom.Name}");

            LeaderboardPostRequestEntity req = new LeaderboardPostRequestEntity()
            {
                matchId = PhotonNetwork.CurrentRoom.Name,
                kittyUrl = GameState.selectedNFT.imageUrl,
                status = PhotonNetwork.CurrentRoom.masterClientId == PhotonNetwork.LocalPlayer.ActorNumber ? MatchStatus.MatchStartedForPlayer1 : MatchStatus.MatchStartedForPlayer2,
            };

            string reqJson = JsonUtility.ToJson(req);

            if (ConfigurationManager.Instance.GameConfig.enableDevLogs)
            {
                Debug.Log(reqJson);
            }

            await NetworkManager.POSTRequest("/leaderboard/match", reqJson, (resp) =>
            {
                Debug.Log("[HTTP]Match start registered!");
            }, (err, code) =>
            {
                Debug.LogWarning($"[HTTP]Error registering the match {code}: {err}");
            }, true);
        }
    }
}