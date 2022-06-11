using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public AudioClip sfx;

    private TextMeshProUGUI _text;

    public void StartCountDown(Action callback)
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(CountdownAnimation(3, callback));   
    }

    private IEnumerator CountdownAnimation(int seconds, Action callback)
    {
        while(seconds > 0)
        {
            _text.text = "" + seconds;
            SFXManager.Instance.PlayOneShot(sfx);
            yield return new WaitForSeconds(1f);
            seconds--;
        }

        callback?.Invoke();
    }
}
