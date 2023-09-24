using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineOnHover : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Outline[] outlines;
    
    [SerializeField] private bool enableImagesOnHover = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var _outline in outlines)
        {
            _outline.enabled = true;
            if (enableImagesOnHover)
            {
                _outline.GetComponent<Image>().enabled = true;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var _outline in outlines)
        {
            _outline.enabled = false;
            if (enableImagesOnHover)
            {
                _outline.GetComponent<Image>().enabled = false;
            }
        }
    }
}
