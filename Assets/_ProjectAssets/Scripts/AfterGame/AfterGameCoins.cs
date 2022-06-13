using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGameCoins : MonoBehaviour
{
    public TMPro.TextMeshProUGUI totalCoinsValue;

    void Start()
    {
        int checkIfIWon = GameResolveStateUtils.CheckIfIWon(GameState.gameResolveState);

        if (checkIfIWon > 0)
        {
            totalCoinsValue.color = Color.green;
        }
        else if (checkIfIWon < 0)
        {
            totalCoinsValue.color = Color.red;
        }
        else
        {
            totalCoinsValue.color = Color.yellow;
        }
    }
}
