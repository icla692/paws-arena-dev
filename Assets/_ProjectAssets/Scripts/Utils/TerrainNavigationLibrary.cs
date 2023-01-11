using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class TerrainNavigationLibrary
{
    public enum Direction
    {
        Left,
        Right
    }

    private const int LAYER_TERRAIN = 6;
    private static readonly LayerMask LAYERMASK_TERRAIN = 1 << LAYER_TERRAIN;

    private const float TRACING_STEP = 0.2f;
    //private const float KITTY_HEIGHT = 1.13833f;

    public static Vector3 GetPositionAtXDisplacement(Bounds obj, Direction direction, float xDisplacement)
    {
        Vector3 xDir = DirectionToVec(direction);
        float xLengthTraversed = TRACING_STEP;

        Vector3 sourcePos = obj.center;
        Vector3 probingPos = sourcePos;
        Vector3 lastGoodProbingPos = sourcePos;

        while (xLengthTraversed <= xDisplacement)
        {
            float y = probingPos.y;
            probingPos = sourcePos + xLengthTraversed * xDir;
            probingPos.y = y;

            if (!PositionIsBetweenHorizontalBounds(probingPos, obj))
            {
                break;
            }

            RaycastHit2D hit = RaycastDownAtYIntervals(probingPos, obj);
            if (hit)
            {
                probingPos.y = hit.point.y + (obj.size.y / 2);
            }
            else
            {
                break;
            }

            lastGoodProbingPos = probingPos;
            xLengthTraversed += TRACING_STEP;
        }

        return lastGoodProbingPos;
    }

    public static bool PositionIsBetweenHorizontalBounds(Vector3 pos, Bounds obj)
    {
        float objWidth = obj.size.x;

        Bounds left = BotManager.Instance.leftMapBound.bounds;
        if (pos.x < left.max.x + objWidth) return false;

        Bounds right = BotManager.Instance.rightMapBound.bounds;
        if (pos.x > right.min.x - objWidth) return false;

        return true;
    }

    private static RaycastHit2D RaycastDown(Vector3 origin)
    {
        return Physics2D.Raycast(origin, Vector3.down, Mathf.Infinity, LAYERMASK_TERRAIN);
    }

    private static RaycastHit2D RaycastDownAtYIntervals(Vector3 origin, Bounds obj, int attempts = 100)
    {
        float y = 0;

        for (int i=0; i<attempts; i++)
        {
            RaycastHit2D hit = RaycastDown(origin + Vector3.up * y);
            if (hit) return hit;
            y += obj.size.y / 2;
        }

        return new RaycastHit2D();
    }

    private static Vector3 DirectionToVec(Direction dir)
    {
        return dir == Direction.Left ? Vector3.left : Vector3.right;
    }
}
