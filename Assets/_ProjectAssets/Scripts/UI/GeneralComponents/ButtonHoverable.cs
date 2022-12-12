using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color colorOverlay;

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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.color = initColor;
    }
}
