using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NFTImageButton : MonoBehaviour
{
    public RawImage rawImage;
    public GameObject selectedGraphics;

    public Texture2D emptyTex;
    public Texture2D loadingTex;

    void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        selectedGraphics.SetActive(false);
        rawImage.texture = emptyTex;
        GetComponent<Button>().interactable = false;
    }

    public void SetLoadingState()
    {
        rawImage.texture = loadingTex;
    }

    public void SetSelectedState()
    {
        selectedGraphics.SetActive(true);
    }

    public void SetTexture(Texture2D tex)
    {
        rawImage.texture = tex;
        GetComponent<Button>().interactable = true;
    }

    public void Select()
    {
        selectedGraphics.SetActive(true);
    }
}
