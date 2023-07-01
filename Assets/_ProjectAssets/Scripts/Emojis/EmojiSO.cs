using UnityEngine;

[CreateAssetMenu(fileName = "NewEmoji", menuName = "ScriptableObjects/Emoji")]
public class EmojiSO : ScriptableObject
{
    [field: SerializeField] public int Id;
    [field: SerializeField] public string Name;
    [field: SerializeField] public Sprite Sprite;
}
