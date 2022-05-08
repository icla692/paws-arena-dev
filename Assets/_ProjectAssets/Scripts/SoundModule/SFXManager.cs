using Anura.Templates.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoSingleton<SFXManager>
{
    public AudioSource oneShotAudioSource;

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
