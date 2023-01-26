using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardUIManager : MonoBehaviour
{
    public LeaderboardData leaderboardData;
    [Header("UI Components")]
    public Transform leaderboardContent;
    public GameObject leaderboardLinePrefab;

    public TMPro.TextMeshProUGUI firstPlacePoints;
    public TMPro.TextMeshProUGUI secondPlacePoints;
    public TMPro.TextMeshProUGUI thirdPlacePoints;

    public PlayerPlatformBehaviour firstPlayer;
    public PlayerPlatformBehaviour secondPlayer;
    public PlayerPlatformBehaviour thirdPlayer;

    // Start is called before the first frame update
    void Start()
    {
        PopulateLeaderboard();
    }

    private async void PopulateLeaderboard()
    {
        LeaderboardEntity data = await leaderboardData.GetLeaderboard();

        foreach(PlayerStatsEntity playerStats in data.leaderboard)
        {
            GameObject go = Instantiate(leaderboardLinePrefab, leaderboardContent);
            go.transform.Find("Nickname").GetComponent<TMPro.TextMeshProUGUI>().text = playerStats.nickname;
            go.transform.Find("Points").GetComponent<TMPro.TextMeshProUGUI>().text = "" + playerStats.points;
        }

        firstPlacePoints.text = "" + data.leaderboard[0]?.points;
        secondPlacePoints.text = "" + data.leaderboard[1]?.points;
        thirdPlacePoints.text = "" + data.leaderboard[2]?.points;

        firstPlayer.SetCat(data.first);
        secondPlayer.SetCat(data.second);
        thirdPlayer.SetCat(data.third);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}
