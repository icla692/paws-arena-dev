using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    [Header("KittyColor")]
    [SerializeField]
    private List<SpriteRenderer> colorMultiplyElements;

    [SerializeField]
    private SerializableStringColorDictionary kittyColorMapping = new SerializableStringColorDictionary();


    [Header("Eyes")]
    [SerializeField]
    private List<GameObject> eyesGameObjects;

    [SerializeField]
    private List<string> eyesKeyIdxMapping;


    public void SetKittyColor(string colorId)
    {
        SetColor(colorId, kittyColorMapping, colorMultiplyElements);
    }

    public void SetEyes(string eyesId)
    {
        SetSingleActiveElement(eyesId, eyesKeyIdxMapping, eyesGameObjects);
    }


    private void SetColor(string key, SerializableStringColorDictionary elements, List<SpriteRenderer> targets)
    {
        Debug.Log($"Trying to set {key}");
        if (!elements.ContainsKey(key))
        {
            Debug.LogWarning($"No {key} in our list");
            return;
        }

        Color col = elements[key];
        foreach (SpriteRenderer sprite in targets)
        {
            sprite.color = col;
        }
    }
    private void SetSingleActiveElement(string key, List<string> ids, List<GameObject> targets)
    {
        Debug.Log($"Trying to set {key}");
        if (!ids.Contains(key))
        {
            Debug.LogWarning($"No {key} in our list");
            return;
        }

        int idx = ids.IndexOf(key);
        for(int i=0; i<targets.Count; i++)
        {
            targets[i].SetActive(i == idx);
        }
    }
}
