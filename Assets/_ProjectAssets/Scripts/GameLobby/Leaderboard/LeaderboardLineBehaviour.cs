using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeaderboardLineBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject principalIdTooltip;
    public TMPro.TextMeshProUGUI principalIdText;
    public Image principalIdIcon;

    [Header("Icons")]
    public Sprite copySprite;
    public Sprite copiedSprite;

    void Start()
    {
        principalIdTooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        principalIdTooltip.SetActive(true);
        principalIdIcon.sprite = copySprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        principalIdTooltip.SetActive(false);
    }

    public void SetPrincipalId(string principalId)
    {
        principalIdText.text = "Principal ID: " + principalId;
    }

    public void SavePrincipalIdToClipboard()
    {
        UniClipboard.SetText(principalIdText.text);
        principalIdIcon.sprite = copiedSprite;
    }
}
