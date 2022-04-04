using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;
using System;

/// <summary>
/// Simple script that paints with Clear color the primary layer and paints using Black the secondary layer.
/// Additionally, on right click paints primary layer with black.
/// Use with SampleScene1.
/// </summary>
public class ClickAndDestroy : MonoBehaviour
{
    [SerializeField]
    protected int circleSize = 16;
    [SerializeField]
    protected int outlineSize = 4;

    protected Shape destroyCircle;
    protected Shape outlineCircle;

    [SerializeField]
    protected BasicPaintableLayer primaryLayer;
    [SerializeField]
    protected BasicPaintableLayer secondaryLayer;

    private void Start()
    {
        destroyCircle = Shape.GenerateShapeCircle(circleSize);
        outlineCircle = Shape.GenerateShapeCircle(circleSize + outlineSize);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnLMBClick();
        }

        if (Input.GetMouseButton(1))
        {
            OnRMBClick();
        }


    }

    protected virtual void OnLMBClick()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition - primaryLayer.transform.position);
        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - circleSize, (int)(p.y * primaryLayer.PPU - circleSize)),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = new Color(0,0,0,0.75f),
            Position = new Vector2Int((int)(p.x * secondaryLayer.PPU) - circleSize - outlineSize, (int)(p.y * secondaryLayer.PPU - circleSize - outlineSize)),
            Shape = outlineCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.NONE
        });
    }

    protected virtual void OnRMBClick()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition - primaryLayer.transform.position);
        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - circleSize, (int)(p.y * primaryLayer.PPU - circleSize)),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.BUILD
        });
    }
}
