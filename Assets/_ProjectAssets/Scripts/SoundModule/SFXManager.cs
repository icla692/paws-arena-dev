using Anura.Templates.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoSingleton<SFXManager>
{
    public AudioSource musicSource;
    public AudioSource oneShotAudioSource;

    public void Start()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        if (!GameState.gameSettings.hasMusic)
        {
            musicSource.volume = 0;
        }
        if (!GameState.gameSettings.hasSoundFX)
        {
            oneShotAudioSource.volume = 0;
        }
    }

    public void PlayOneShot(AudioClip clip, float volume = 1)
    {
        StopOneShot();
        oneShotAudioSource.volume = volume;
        oneShotAudioSource.PlayOneShot(clip);
    }

    public void StopOneShot()
    {
        oneShotAudioSource.Stop();
    }
}
