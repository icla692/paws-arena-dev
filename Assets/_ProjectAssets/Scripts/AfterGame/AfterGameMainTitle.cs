using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGameMainTitle : MonoBehaviour
{
    public GameObject winTitle;
    public GameObject loseTitle;
    public GameObject drawTitle;
    void Start()
    {
        int checkIfIWon = GameResolveStateUtils.CheckIfIWon(GameState.gameResolveState);

        if (checkIfIWon > 0)
        {
            winTitle.SetActive(true);
        }
        else if (checkIfIWon < 0)
        {
            loseTitle.SetActive(true);
        }
        else
        {
            drawTitle.SetActive(true);
        }
    }
}
