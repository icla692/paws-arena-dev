using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterGameMainTitle : MonoBehaviour
{
    public GameObject winTitle;
    public GameObject loseTitle;
    public GameObject drawTitle;
    public Image bg;

    public TMPro.TextMeshProUGUI totalCoinsValue;
    public Color winColor;
    public Color loseColor;
    public Color drawColor;
    void Start()
    {
        int checkIfIWon = GameResolveStateUtils.CheckIfIWon(GameState.gameResolveState);

        if (checkIfIWon > 0)
        {
            winTitle.SetActive(true);
            bg.GetComponent<Image>().color = winColor;
            totalCoinsValue.color = winColor;
        }
        else if (checkIfIWon < 0)
        {
            loseTitle.SetActive(true);
            bg.GetComponent<Image>().color = loseColor;
            totalCoinsValue.color = loseColor;
        }
        else
        {
            drawTitle.SetActive(true);
            bg.GetComponent<Image>().color = drawColor;
            totalCoinsValue.color = drawColor;
        }
    }
}
