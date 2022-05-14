using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIBehaviour : MonoBehaviour
{
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

        Debug.Log("New HP: " + currentHealth);
        float startingX = healthBar.sizeDelta.x;
        LeanTween.value(startingX, healthBarTotalWidth * (currentHealth * 1.0f / totalhealth), 1f).setEaseInOutCirc().setOnUpdate(val =>
        {
            healthBar.sizeDelta = new Vector2(val, healthBar.sizeDelta.y);
        });
    }

    public void Init()
    {
        isInit = true;
        healthBarTotalWidth = healthBar.sizeDelta.x;
        totalhealth = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();

        OnHealthUpdated(currentHealth);
    }
}
