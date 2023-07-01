using UnityEngine;
using UnityEngine.UI;

public class EmojiInGame : MonoBehaviour
{
    [SerializeField] private Image display;
    private readonly float upWordsMovementAmount = 0.8f;
    
    public void Setup(EmojiSO _emoji)
    {
        transform.localScale = Vector3.zero;
        transform.localPosition = Vector3.zero;
        display.sprite = _emoji.Sprite;

        LeanTween.scale(gameObject, Vector3.one, 0.2f)
            .setEase(LeanTweenType.easeOutCubic) // Use a desired easing type
            .setOnComplete(() =>
            {
                Vector3 _newPosition = transform.position;
                _newPosition.y += upWordsMovementAmount;
                LeanTween.move(gameObject, _newPosition, 0.5f)
                    .setEase(LeanTweenType.easeOutCubic)
                    .setOnComplete(() =>
                    {
                        LeanTween.value(gameObject, 1f, 0f, 0.15f)
                            .setEase(LeanTweenType.easeOutQuad) // Use a desired easing type
                            .setOnUpdate((float _alpha) =>
                            {
                                Color _color = display.color;
                                _color.a = _alpha;
                                display.color = _color;
                            })
                            .setOnComplete(() =>
                            {
                                Destroy(gameObject);
                            });
                        _newPosition.y += upWordsMovementAmount / 2;
                        LeanTween.move(gameObject, _newPosition, 0.1f)
                            .setEase(LeanTweenType.easeOutCubic);
                    });
            });
    }
}