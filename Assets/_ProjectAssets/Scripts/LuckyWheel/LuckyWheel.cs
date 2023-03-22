using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyWheel : MonoBehaviour
{
    [SerializeField] AnimationCurve spinCurve;

    [SerializeField] float spinSpeed;
    [SerializeField] RectTransform pointerHolder;

    Action<LuckyWheelRewardSO> callback;

    public void Spin(Action<LuckyWheelRewardSO> _callback)
    {
        callback = _callback;
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        float _spinDuration = UnityEngine.Random.Range(6, 8f); // to get more randomnes
        float _timePassed = 0;

        while (_timePassed < _spinDuration)
        {
            _timePassed += Time.deltaTime;
            float _pointOnCurve = _timePassed / _spinDuration;
            float _zIncrement = Time.deltaTime * spinSpeed * spinCurve.Evaluate(_pointOnCurve);

            Vector3 _rotaiton = pointerHolder.eulerAngles;
            _rotaiton.z += _zIncrement;
            pointerHolder.eulerAngles = _rotaiton;

            yield return null;
        }

        LuckyWheelRewardSO _choosenReward = LuckyWheelRewardSO.GetReward();
        float _targetedZ = UnityEngine.Random.Range(_choosenReward.MinRotation, _choosenReward.MaxRotation);
        Vector3 _targetedRotation = pointerHolder.eulerAngles;
        if (_targetedZ < 0)
        {
            _targetedZ += 360;
        }
        _targetedRotation.z = _targetedZ;

        float _speedModifier = 8;
        float threshold = 0.01f;
        while (Vector3.Distance(pointerHolder.eulerAngles, _targetedRotation) > threshold)
        {
            if (pointerHolder.eulerAngles.z < _targetedRotation.z)
            {
                pointerHolder.eulerAngles = Vector3.MoveTowards(pointerHolder.eulerAngles, _targetedRotation, Time.deltaTime * spinSpeed / _speedModifier);
            }
            else
            {
                pointerHolder.eulerAngles = Vector3.MoveTowards(pointerHolder.eulerAngles, _targetedRotation, -Time.deltaTime * spinSpeed / _speedModifier);
            }
            yield return null;
        }

        callback?.Invoke(_choosenReward);
    }
}