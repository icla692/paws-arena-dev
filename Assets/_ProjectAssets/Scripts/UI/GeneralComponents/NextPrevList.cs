using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPrevList : MonoBehaviour
{
    public List<GameObject> list;
    public int currentIdx = 0;

    public GameObject nextButton;
    public GameObject prevButton;

    private void OnEnable()
    {
        SetCurrentIdx(currentIdx);
    }

    public void Next()
    {
        if (currentIdx + 1 >= list.Count) return;
        SetCurrentIdx(currentIdx + 1);
    }

    public void Back()
    {
        if (currentIdx <= 0) return;
        SetCurrentIdx(currentIdx - 1);
    }

    private void SetCurrentIdx(int newIdx)
    {
        currentIdx = newIdx;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(i == currentIdx);
        }

        prevButton.SetActive(currentIdx != 0);
        nextButton.SetActive(currentIdx < (list.Count - 1));
    }
}
