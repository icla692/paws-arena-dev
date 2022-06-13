using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMobileLauncherBehaviour : MonoBehaviour
{
    private Animator _animator;

    [HideInInspector]
    public PlayerThrowBehaviour throwBehaviour;

    void Start()
    {
        _animator = GetComponent<Animator>();
        PlayerThrowBehaviour.onLaunchPreparing += PrepareLaunch;
    }

    private void OnDestroy()
    {
        PlayerThrowBehaviour.onLaunchPreparing -= PrepareLaunch;
    }

    private void PrepareLaunch()
    {
        _animator.SetTrigger("Shoot");
    }
    
    public void Launch()
    {
        throwBehaviour.Launch();
    }
}
