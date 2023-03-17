using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEmoji", menuName = "ScriptableObjects/Emojis")]
public class EmojiSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public Sprite Preview { get; private set; }
    [field: SerializeField] public GameObject Visual { get; private set; }
    [SerializeField] EmojiAnimationType animationType;
    static List<EmojiSO> allEmojis;

    public string AnimationNime => animationType.ToString();

    public static EmojiSO GetEmoji(int _emojiId)
    {
        LoadAllEmojis();
        return allEmojis.First(element => element.Id == _emojiId);
    }

    public static List<EmojiSO> GetEmojis()
    {
        LoadAllEmojis();
        return allEmojis;
    }


    static void LoadAllEmojis()
    {
        if (allEmojis != null)
        {
            return;
        }

        allEmojis = Resources.LoadAll<EmojiSO>("Emojis/").ToList();
    }

}
