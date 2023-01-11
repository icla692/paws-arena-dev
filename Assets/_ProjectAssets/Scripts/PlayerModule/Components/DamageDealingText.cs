using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealingText : MonoBehaviour
{
    public void Init(int damage)
    {
        var text = GetComponent<TMPro.TextMeshPro>();
        text.text = $"-{damage}p";
        text.color = Color.clear;

        var rect = GetComponent<RectTransform>();
        LeanTween.value(0, 1, 0.5f).setDelay(UnityEngine.Random.Range(0, 1f)).setOnComplete(() =>
        {
            text.color = Color.white;
            LeanTween.value(0, 2, 1.5f).setOnUpdate(val =>
            {
                rect.anchoredPosition = new Vector2(0, val);
            })
            .setOnComplete(() =>
            {
                LeanTween.scale(gameObject, Vector3.zero, .5f).setOnComplete(() =>
                {
                    Destroy(gameObject);
                });
            });
        });
    }
}
