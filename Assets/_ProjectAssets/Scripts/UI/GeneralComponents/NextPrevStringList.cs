using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPrevStringList : NextPrevList
{
    public GameObject value;
    public List<string> list;

    protected override int GetNumberOfElements()
    {
        return list.Count;
    }

    protected override void SetCurrentIdx(int newIdx)
    {
        base.SetCurrentIdx(newIdx);
        value.GetComponent<TMPro.TextMeshProUGUI>().text = list[newIdx];
    }
}
