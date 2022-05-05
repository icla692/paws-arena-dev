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
    private int currentHealth;

    private bool isInit = false;

    private void Start()
    {
        currentHealth = totalhealth;
    }
    public void OnHealthUpdated(int val)
    {
        currentHealth = val;

        if (!isInit) return;

        healthText.text = "" + currentHealth;
        healthBar.sizeDelta = new Vector2(healthBarTotalWidth * (currentHealth * 1.0f / totalhealth), healthBar.sizeDelta.y);
    }

    public void Init()
    {
        isInit = true;
        healthBarTotalWidth = healthBar.sizeDelta.x;
        totalhealth = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();

        OnHealthUpdated(currentHealth);
    }
}
