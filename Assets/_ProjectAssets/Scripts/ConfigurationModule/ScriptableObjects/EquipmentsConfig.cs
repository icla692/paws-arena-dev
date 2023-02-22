using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteIdPair
{
    [SerializeField]
    public string id;
    [SerializeField]
    public Sprite thumbnail;
}


[CreateAssetMenu(fileName = "EquipmentsConfig", menuName = "Configurations/EquipmentsConfig", order = 4)]
public class EquipmentsConfig : ScriptableObject
{
    public List<Sprite> eyes;
    public List<Sprite> head;
    public List<Sprite> mouth;
    public List<Sprite> body;

    public List<Sprite> tailsOverlay;
    public List<Sprite> tailsFloating;
    public List<SpriteIdPair> tailsAnimated;
}
