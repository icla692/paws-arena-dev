using Anura.ConfigurationModule.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class IndicatorInputCircleBehaviour : MonoBehaviour, IPointerUpHandler
{
    public event Action<float, float> onIndicatorPlaced;
    
    private float radius;

    private void Start()
    {
        radius = ConfigurationManager.Instance.Config.GetCircleShootRadius();
        transform.localScale = Vector3.one * 2.0f * radius;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Click position: {eventData.position}; CirclePosition: {Camera.main.WorldToScreenPoint(transform.position)}");
    }

    public void CheckPointerClick(Vector2 pointerPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pointerPos), Vector2.zero);
        if(hit.collider != null && hit.collider.gameObject == gameObject)
        {
            Vector2 pos = transform.position;
            float angle = Vector2.SignedAngle(new Vector2(1, 0), hit.point - pos);
            float power = (hit.point - pos).magnitude / radius;
            onIndicatorPlaced?.Invoke(angle, power);
        }
    }
}
