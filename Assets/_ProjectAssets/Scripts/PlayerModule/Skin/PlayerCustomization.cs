using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    [SerializeField]
    private bool inGame = false;

    [SerializeField]
    private GameObject backpack;
    [SerializeField]
    private GameObject rEar;
    [SerializeField]
    private GameObject lEar;

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

    [Header("Back")]
    [SerializeField]
    private GameObject backParent;
    [SerializeField]
    private SpriteRenderer backSpriteRenderer;

    [SerializeField]
    private List<string> backKeysMapping;
    [SerializeField]
    private List<Sprite> backSprites;


    [Header("Body")]
    [SerializeField]
    private SpriteRenderer bodySpriteRenderer;

    [SerializeField]
    private List<string> bodyKeysMapping;
    [SerializeField]
    private List<Sprite> bodySprites;


    [Header("Hats")]
    [SerializeField]
    private SpriteRenderer hatsSpriteRenderer;
    [SerializeField]
    private SpriteRenderer hatsNoEarsRenderer;
    [SerializeField]
    private SpriteRenderer hatsBetweenEarsRenderer;

    [SerializeField]
    private List<string> hatsKeysMapping;
    [SerializeField]
    private List<string> hatsNoEarsKeysMapping;
    [SerializeField]
    private List<string> hatsBetweenEarsMapping;

    [SerializeField]
    private List<Sprite> hatsSprites;
    [SerializeField]
    private List<Sprite> hatsNoEarsSprites;
    [SerializeField]
    private List<Sprite> hatsBetweenEarsSprites;

    [Header("Eyewear")]
    [SerializeField]
    private SpriteRenderer eyewearSpriteRenderer;

    [SerializeField]
    private List<string> eyewearKeysMapping;
    [SerializeField]
    private List<Sprite> eyewearSprites;

    [Header("Mouth")]
    [SerializeField]
    private SpriteRenderer mouthSpriteRenderer;

    [SerializeField]
    private List<string> mouthKeysMapping;
    [SerializeField]
    private List<Sprite> mouthSprites;

    [Header("GroundFront")]
    [SerializeField]
    private Transform groundFrontTransform;
    [SerializeField]
    private SpriteRenderer groundFrontSpriteRenderer;
    [SerializeField]
    private List<string> groundFrontKeysMapping;
    [SerializeField]
    private List<Sprite> groundFrontSprites;

    [Header("GroundBack")]
    [SerializeField]
    private Transform groundBackTransform;
    [SerializeField]
    private SpriteRenderer groundBackSpriteRenderer;
    [SerializeField]
    private List<string> groundBackKeysMapping;
    [SerializeField]
    private List<Sprite> groundBackSprites;

    [Header("Ground")]
    [SerializeField]
    private Transform groundTransform;
    [SerializeField]
    private SpriteRenderer groundSpriteRenderer;
    [SerializeField]
    private List<string> groundKeysMapping;
    [SerializeField]
    private List<Sprite> groundSprites;

    private void OnEnable()
    {
        if (!inGame)
        {
            backpack.SetActive(false);
        }
        else
        {
            backParent.SetActive(false);
            groundFrontTransform.gameObject.SetActive(false);
            groundBackTransform.gameObject.SetActive(false);
            groundTransform.gameObject.SetActive(false);
        }
    }

    public void SetKittyColor(string colorId)
    {
        SetColor(colorId, kittyColorMapping, colorMultiplyElements);
    }

    public void SetEyes(string eyesId)
    {
        SetSingleActiveElement(eyesId, eyesKeyIdxMapping, eyesGameObjects);
    }

    public void SetBack(string backId)
    {
        if (inGame) return;
        SetSingleSpriteElement(backId, backKeysMapping, backSprites, backSpriteRenderer);
    }

    public void SetBody(string bodyId)
    {
        SetSingleSpriteElement(bodyId, bodyKeysMapping, bodySprites, bodySpriteRenderer);
    }

    public void SetHat(string hatId)
    {
        if (hatsBetweenEarsMapping.Contains(hatId)){
            lEar.SetActive(true);
            rEar.SetActive(true);

            hatsBetweenEarsRenderer.gameObject.SetActive(true);
            hatsNoEarsRenderer.gameObject.SetActive(false);
            hatsSpriteRenderer.gameObject.SetActive(false);

            SetSingleSpriteElement(hatId, hatsBetweenEarsMapping, hatsBetweenEarsSprites, hatsBetweenEarsRenderer);
        }
        else if (hatsKeysMapping.Contains(hatId))
        {
            lEar.SetActive(true);
            rEar.SetActive(true);

            hatsBetweenEarsRenderer.gameObject.SetActive(false);
            hatsNoEarsRenderer.gameObject.SetActive(false);
            hatsSpriteRenderer.gameObject.SetActive(true);

            SetSingleSpriteElement(hatId, hatsKeysMapping, hatsSprites, hatsSpriteRenderer);
        }
        else if (hatsNoEarsKeysMapping.Contains(hatId))
        {
            lEar.SetActive(false);
            rEar.SetActive(false);

            hatsBetweenEarsRenderer.gameObject.SetActive(false);
            hatsNoEarsRenderer.gameObject.SetActive(true);
            hatsSpriteRenderer.gameObject.SetActive(false);

            SetSingleSpriteElement(hatId, hatsNoEarsKeysMapping, hatsNoEarsSprites, hatsNoEarsRenderer);
        }
    }

    public void SetEyewear(string eyewearId)
    {
        SetSingleSpriteElement(eyewearId, eyewearKeysMapping, eyewearSprites, eyewearSpriteRenderer);
    }

    public void SetMouth(string mouthId)
    {
        SetSingleSpriteElement(mouthId, mouthKeysMapping, mouthSprites, mouthSpriteRenderer);
    }

    public void SetGroundFront(string groundId)
    {
        if (inGame) return;
        SetSingleSpriteElement(groundId, groundFrontKeysMapping, groundFrontSprites, groundFrontSpriteRenderer);
    }

    public void SetGroundBack(string groundId)
    {
        if (inGame) return;
        SetSingleSpriteElement(groundId, groundBackKeysMapping, groundBackSprites, groundBackSpriteRenderer);
    }

    public void SetGround(string groundId)
    {
        if (inGame) return;
        SetSingleSpriteElement(groundId, groundKeysMapping, groundSprites, groundSpriteRenderer);
    }

    private void SetSingleSpriteElement(string key, List<string> elements, List<Sprite> sprites, SpriteRenderer spriteRenderer)
    {
        Debug.Log($"Trying to set {key}");
        if (!elements.Contains(key))
        {
            Debug.LogWarning($"No {key} in our list");
            return;
        }

        int idx = elements.IndexOf(key);
        spriteRenderer.sprite = sprites[idx];
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
