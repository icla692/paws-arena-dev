using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Extensions;
using Photon.Pun;
using System;
using UnityEngine;

public class PlayerIndicatorBehaviour : MonoBehaviour
{
    [SerializeField] private Transform indicator;
    [SerializeField] private IndicatorInputCircleBehaviour indicatorCircle;
    [SerializeField] private LineRenderer lineDirectionIndicator;
    [SerializeField] private LineRenderer lineIndicatorSpeed;
    [HideInInspector] public float currentPower;

    private PhotonView photonView;
    private Config config => ConfigurationManager.Instance.Config;
    private Vector2 lastMousePosition;
    private float maxRadius;

    private bool isHoldingSelect = false;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        maxRadius = config.GetCircleShootRadius();

        lineDirectionIndicator.SetPosition(1, new Vector3(maxRadius, 0, 1));
        SetPowerLineLength(1.0f);
    }

    private void Update()
    {
        if (isHoldingSelect)
        {
            indicatorCircle.CheckPointerClick(lastMousePosition);
        }
    }

    private void OnEnable()
    {
        indicatorCircle.onIndicatorPlaced += OnIndicatorPlaced;
    }

    private void OnDisable()
    {
        indicatorCircle.onIndicatorPlaced -= OnIndicatorPlaced;
    }

    public void RegisterDirectionCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.ScreenPosition.performed += value => lastMousePosition = value.ReadValue<Vector2>();
        playerActions.Select.started += _ => isHoldingSelect = true;
        playerActions.Select.canceled += _ => isHoldingSelect = false;
    }

    private void OnIndicatorPlaced(float angle, float power)
    {
        indicator.rotation = Quaternion.Euler(Vector3.zero.WithZ(angle));
        photonView.RPC("SetPowerLineLength", RpcTarget.All, power);
    }

    [PunRPC]
    private void SetPowerLineLength(float power)
    {
        lineIndicatorSpeed.SetPosition(1, new Vector3(0.2f + power * maxRadius, 0, 1));
        currentPower = power;
    }
}
