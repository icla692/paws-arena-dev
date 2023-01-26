using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardData : MonoBehaviour
{
    [HideInInspector]
    public LeaderboardEntity leaderboard = null;
    public async UniTask<LeaderboardEntity> GetLeaderboard()
    {
        if (leaderboard != null && leaderboard.leaderboard.Count > 0)
        {
            return leaderboard;
        }

        Debug.Log("[HTTP] Grabbing leaderboard...");
        string resp = await NetworkManager.GETRequestCoroutine("/leaderboard",
            (code, err) =>
            {
                Debug.LogWarning("Couldn't retrieve Leaderboard!");
            });

        if (string.IsNullOrEmpty(resp)) return null;

        leaderboard = JsonUtility.FromJson<LeaderboardEntity>(resp);

        Debug.Log($"[HTTP] Grabbed {leaderboard.leaderboard.Count} players...");
        return leaderboard;
    }
}
