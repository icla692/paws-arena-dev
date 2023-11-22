using System.Collections;
using Anura.ConfigurationModule.Managers;
using UnityEngine;

public class StaminaDisplay : MonoBehaviour
{
    [SerializeField] private Transform foreground;
    [SerializeField] private Transform border;

    private Vector3 originalForegroundPosition; // To store the original position
    private IEnumerator displayRoutine;

    private void Start()
    {
        // Store the original position of the foreground
        originalForegroundPosition = foreground.localPosition;
        border.localScale = Vector3.zero;
    }
    
    public void UpdateMovementBar(float _movementLeftThisTurn)
    {
        border.localScale = new Vector3(2, 0.45f, 1);
        float _maxWidth = 0.95f; // Width scale of the Background
        float _maxHeight = 0.8f; // Height scale of the Background
        float _maxMovement = ConfigurationManager.Instance.MovingDistance;
        float _normalizedMovementLeft = _movementLeftThisTurn / _maxMovement;
        float _currentWidth = _maxWidth * _normalizedMovementLeft;

        float widthChange = _maxWidth - _currentWidth;

        foreground.localScale = new Vector3(_currentWidth, _maxHeight, 1);

        foreground.localPosition = originalForegroundPosition + new Vector3(-widthChange * 0.5f, 0, 0);
        
        if (displayRoutine == null)
        {
            displayRoutine = DisplayRoutine();
            StartCoroutine(displayRoutine);
        }
        else
        {
            // Restart the counter if already running
            StopCoroutine(displayRoutine);
            displayRoutine = DisplayRoutine();
            StartCoroutine(displayRoutine);
        }
    }

    private IEnumerator DisplayRoutine()
    {
        float _counter = 1;
        while (_counter > 0)
        {
            _counter -= Time.deltaTime;
            yield return null;
        }
        
        LeanTween.scale(border.gameObject, Vector3.zero, 0.2f); // Smoothly scale to Vector3.zero
        displayRoutine = null; // Reset the routine
    }
}
