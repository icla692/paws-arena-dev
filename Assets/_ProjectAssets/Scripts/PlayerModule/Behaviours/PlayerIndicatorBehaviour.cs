using Anura.ConfigurationModule.Managers;
using Anura.Extensions;
using Photon.Pun;
using UnityEngine;

public class PlayerIndicatorBehaviour : MonoBehaviour
{
    [SerializeField] private Transform indicator;

    //private bool hasRotate;

    //private float currentDirection;
    private Vector2 lastMouseDirection;
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Vector2 indicatorScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            indicator.rotation = Quaternion.Euler(new Vector3(0, 0, Vector2.SignedAngle(new Vector2(1, 0), lastMouseDirection - indicatorScreenPos)));
        }
    }

    public void RegisterDirectionCallbacks(GameInputActions.PlayerActions playerActions)
    {
        //playerActions.Indicator.started += _ => hasRotate = true;
        //playerActions.Indicator.performed += value => SetIndicatorDirection(value.ReadValue<float>());
        //playerActions.Indicator.canceled += _ => { hasRotate = false; SetIndicatorDirection(0); };

        playerActions.ScreenPosition.performed += value => lastMouseDirection = value.ReadValue<Vector2>();
    }

    //public void SetIndicatorDirection(float direction)
    //{
    //    currentDirection = direction;
    //}

    //private float GetIndicatorSpeed()
    //{
    //    return ConfigurationManager.Instance.Config.GetIndicatorSpeed();
    //}
}
