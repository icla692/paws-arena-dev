using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeaderboardLineBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject principalIdTooltip;
    public TMPro.TextMeshProUGUI principalIdText;

    void Start()
    {
        principalIdTooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        principalIdTooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        principalIdTooltip.SetActive(false);
    }

    public void SetPrincipalId(string principalId)
    {
        principalIdText.text = "Principal ID: " + principalId;
    }
}
