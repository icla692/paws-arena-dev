using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.colorfulcoding.AfterGame
{
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

        [Header("Cat Stand")]
        public SpriteRenderer standGlow;
        public Color standWinColor;
        public Color standLoseColor;
        public Color standDrawColor;
        void Start()
        {
            int checkIfIWon = GameResolveStateUtils.CheckIfIWon(GameState.gameResolveState);

            if (checkIfIWon > 0)
            {
                winTitle.SetActive(true);
                bg.GetComponent<Image>().color = winColor;
                standGlow.color = winColor;
            }
            else if (checkIfIWon < 0)
            {
                loseTitle.SetActive(true);
                bg.GetComponent<Image>().color = loseColor;
                standGlow.color = loseColor;
            }
            else
            {
                drawTitle.SetActive(true);
                bg.GetComponent<Image>().color = drawColor;
                standGlow.color = drawColor;
            }

            totalCoinsValue.text = "" + GameState.pointsChange.oldPoints;

            if (GameState.pointsChange.points != 0)
            {
                LeanTween.value(gameObject, GameState.pointsChange.oldPoints, GameState.pointsChange.oldPoints + GameState.pointsChange.points, 2f).setOnUpdate((float val) =>
                {
                    totalCoinsValue.text = "" + Math.Floor(val);
                });
            }
        }
    }
}