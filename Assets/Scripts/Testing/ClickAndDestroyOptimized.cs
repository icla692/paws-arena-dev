using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

public class ClickAndDestroyOptimized : ClickAndDestroy
{
    protected override void OnLMBClick()
    {

        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - primaryLayer.transform.position;

        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - circleSize, (int)(p.y * primaryLayer.PPU) - circleSize),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(p.x * secondaryLayer.PPU) - circleSize, (int)(p.y * secondaryLayer.PPU) - circleSize),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.NONE
        });

    }

    protected override void OnRMBClick()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - primaryLayer.transform.position;
        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - circleSize, (int)(p.y * primaryLayer.PPU) - circleSize),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.NONE,
            DestructionMode = DestructionMode.BUILD
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(p.x * secondaryLayer.PPU) - circleSize, (int)(p.y * secondaryLayer.PPU) - circleSize),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.BUILD
        });

    }
}
