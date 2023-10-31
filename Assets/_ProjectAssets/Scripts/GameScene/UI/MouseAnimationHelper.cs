using UnityEngine;

public class MouseAnimationHelper : MonoBehaviour
{
    [SerializeField] private SpriteRenderer keyRenderer;
    private Sprite[] sprites;

    public void ApplySkin(int _index)
    {
        if (sprites == null)
        {
            WeaponSkinIdentificator _skinIdentifier = GetComponentInParent<WeaponSkinIdentificator>();
            sprites = WeaponSkinSO.Get(_skinIdentifier.SkinId).ProjectileSprite;
        }
        keyRenderer.sprite = sprites[11 + _index];
    }
}
