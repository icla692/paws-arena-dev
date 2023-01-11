using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite defaultSprite;

    public Color colorOverlay;
    public TMPro.TextMeshProUGUI text;

    public Sprite onClickImage;

    private Button button;
    private Color initColor;

    private void Start()
    {
        button = GetComponent<Button>();
        initColor = button.image.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.image.color = colorOverlay;
        if(text!= null)
        {
            text.color = colorOverlay;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.color = initColor;
        if (text != null)
        {
            text.color = initColor;
        }
    }
}
