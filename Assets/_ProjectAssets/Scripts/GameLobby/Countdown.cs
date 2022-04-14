using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public void StartCountDown(Action callback)
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(CountdownAnimation(5, callback));   
    }

    private IEnumerator CountdownAnimation(int seconds, Action callback)
    {
        while(seconds > 0)
        {
            _text.text = "" + seconds;
            yield return new WaitForSeconds(1f);
            seconds--;
        }

        callback?.Invoke();
    }
}
