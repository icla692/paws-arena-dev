using Anura.Templates.MonoSingleton;
using DTerrain;
using UnityEngine;

public class PaintingManager : MonoSingleton<PaintingManager>
{
    [SerializeField] private int defaultCircleSize = 16;
    [SerializeField] private int defaultOutlineSize = 4;

    [SerializeField] private BasicPaintableLayer primaryLayer;
    [SerializeField] private BasicPaintableLayer secondaryLayer;

    private Shape destroyCircle;
    private Shape outlineCircle;

    private void Start()
    {
        destroyCircle = Shape.GenerateShapeCircle(defaultCircleSize);
        outlineCircle = Shape.GenerateShapeCircle(defaultOutlineSize);
    }

    public void Destroy(Vector3 hitPoint)
    {
        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(hitPoint.x * primaryLayer.PPU) - defaultCircleSize, (int)(hitPoint.y * primaryLayer.PPU) - defaultCircleSize),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(hitPoint.x * secondaryLayer.PPU) - defaultCircleSize, (int)(hitPoint.y * secondaryLayer.PPU) - defaultCircleSize),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.NONE
        });

    }

    public void Build(Vector3 hitPoint)
    {
        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(hitPoint.x * primaryLayer.PPU) - defaultCircleSize, (int)(hitPoint.y * primaryLayer.PPU) - defaultCircleSize),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.NONE,
            DestructionMode = DestructionMode.BUILD
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(hitPoint.x * secondaryLayer.PPU) - defaultCircleSize, (int)(hitPoint.y * secondaryLayer.PPU) - defaultCircleSize),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.BUILD
        });
    }
}
