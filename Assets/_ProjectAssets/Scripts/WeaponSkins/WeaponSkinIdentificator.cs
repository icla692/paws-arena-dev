using UnityEngine;

public class WeaponSkinIdentificator : MonoBehaviour
{
    [field: SerializeField] public WeaponSkinType Type { get; private set; }
    [field: SerializeField] private SpriteRenderer[] sprites;

    private int skinId;

    public int SkinId
    {
        get
        {
            return skinId;
        }
        set
        {
            skinId = value;
        }
    }

    public void ApplySkin(Sprite[] _sprites, int _skinId)
    {
        skinId = _skinId;
        for (var _index = 0; _index < _sprites.Length; _index++)
        {
            if (sprites.Length<=_index)
            {
                break;
            }
            sprites[_index].sprite = _sprites[_index];
        }
    }
}
