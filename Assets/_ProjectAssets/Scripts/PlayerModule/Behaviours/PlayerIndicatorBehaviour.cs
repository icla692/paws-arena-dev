using Anura.ConfigurationModule.Managers;
using Anura.Extensions;
using UnityEngine;

public class PlayerIndicatorBehaviour : MonoBehaviour
{
    [SerializeField] private Transform indicator;

    private bool hasRotate;

    private float currentDirection;

    private void Update()
    {
        if (!hasRotate)
            return;

        indicator.Rotate(Vector3.zero.WithZ(currentDirection * GetIndicatorSpeed()) * Time.deltaTime);
    }

    public void RegisterDirectionCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Indicator.started += _ => hasRotate = true;
        playerActions.Indicator.performed += value => SetIndicatorDirection(value.ReadValue<float>());
        playerActions.Indicator.canceled += _ => { hasRotate = false; SetIndicatorDirection(0); };
    }

    public void SetIndicatorDirection(float direction)
    {
        currentDirection = direction;
    }

    private float GetIndicatorSpeed()
    {
        return ConfigurationManager.Instance.Config.GetIndicatorSpeed();
    }
}
