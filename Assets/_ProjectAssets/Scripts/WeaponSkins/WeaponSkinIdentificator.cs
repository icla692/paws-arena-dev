using UnityEngine;

public class WeaponSkinIdentificator : MonoBehaviour
{
    [field: SerializeField] public WeaponSkinType Type { get; private set; }
    [field: SerializeField] private SpriteRenderer[] sprites;

    public void ApplySkin(Sprite[] _sprites)
    {
        for (var _index = 0; _index < _sprites.Length; _index++)
        {
            sprites[_index].sprite = _sprites[_index];
        }
    }
}
