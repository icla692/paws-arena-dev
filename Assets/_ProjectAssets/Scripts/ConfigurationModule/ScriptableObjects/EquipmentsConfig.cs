using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentsConfig", menuName = "Configurations/EquipmentsConfig", order = 4)]
public class EquipmentsConfig : ScriptableObject
{
    public List<Sprite> eyes;
    public List<Sprite> head;
    public List<Sprite> mouth;
    public List<Sprite> body;
    public List<Sprite> tail;
    public List<Sprite> legs;
}
