using Anura.Templates.MonoSingleton;
using DTerrain;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapsManager : MonoSingleton<MapsManager> 
{
    public PUNRoomUtils punRoomUtils;
    public List<GameObject> mapPrefabs;

    public void CreateMap()
    {
        int mapIdx = (int)punRoomUtils.GetRoomCustomProperty("mapIdx");
        var go = GameObject.Instantiate(mapPrefabs[mapIdx], Vector3.zero, Quaternion.identity, transform);
        List<BasicPaintableLayer> paintables =  go.GetComponentsInChildren<BasicPaintableLayer>().ToList();
        if(paintables.Count != 2)
        {
            Debug.LogWarning("There are not just 2 BasicPaintableLayer's in the map!");
        }
        PaintingManager.Instance.primaryLayer = paintables[0];
        PaintingManager.Instance.secondaryLayer = paintables[1];
    }
}
