using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsBar : MonoBehaviour
{
    public static event Action OnShoot;
    public static event Action<bool> WeaponStateUpdated;

    public GameObject playerActionsWrapper;
    public GameObject weaponInBar;
    public GameObject weaponOutBar;

    private void OnEnable()
    {
        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDisable()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (state is MyTurnState)
        {
            playerActionsWrapper.SetActive(true);
            weaponInBar.SetActive(true);
            weaponOutBar.SetActive(false);
        }
        else
        {
            playerActionsWrapper.SetActive(false);
        }
    }

    public void Shoot()
    {
        OnShoot?.Invoke();
    }

    public void WeaponOut()
    {
        weaponOutBar.SetActive(true);
        weaponInBar.SetActive(false);
        WeaponStateUpdated?.Invoke(true);
    }

    public void WeaponIn()
    {
        weaponInBar.SetActive(true);
        weaponOutBar.SetActive(false);
        WeaponStateUpdated?.Invoke(false);
    }
}
