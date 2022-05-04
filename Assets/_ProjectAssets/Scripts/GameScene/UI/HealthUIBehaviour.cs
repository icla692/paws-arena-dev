using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIBehaviour : MonoBehaviour
{
    public TMPro.TextMeshProUGUI healthText;
    public RectTransform healthBar;

    private float healthBarTotalWidth = -1;
    private int totalhealth;

    public void OnHealthUpdated(int val)
    {
        healthText.text = "" + val;
        healthBar.sizeDelta = new Vector2(healthBarTotalWidth * (val * 1.0f / totalhealth), healthBar.sizeDelta.y);
    }

    public void Init()
    {
        healthBarTotalWidth = healthBar.sizeDelta.x;
        totalhealth = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();
    }
}
