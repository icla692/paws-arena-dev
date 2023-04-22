using System.Collections;
using UnityEngine;

public class LuckyWheelRewardDisplay : MonoBehaviour
{
    const string IDLE_ANIMATION_KEY = "Idle";
    const string SHAKING_ANIMATION_KEY = "Shaking";

    [SerializeField] Vector3 shakingScale;
    [SerializeField] GameObject shadowHolder;
    [SerializeField] Transform cristalHolder;
    [SerializeField] LuckyWheelRewardType rewardType;

    public LuckyWheelRewardType RewardType => rewardType;

    IEnumerator shakingRoutine;

    Vector3 defaultScale;
    Vector3 defaultPostion;

    private void OnEnable()
    {
        defaultScale = cristalHolder.localScale;
        defaultPostion = cristalHolder.position;
    }

    public void ResetDisplay()
    {
        if (shakingRoutine != null)
        {
            StopCoroutine(shakingRoutine);
            shakingRoutine = null;
        }
        cristalHolder.localScale = defaultScale;
        cristalHolder.position = defaultPostion;
        shadowHolder.SetActive(false);
    }

    public void ShowShadow()
    {
        shadowHolder.SetActive(true);
    }

    public void Shake()
    {
        cristalHolder.localScale = shakingScale;
        shakingRoutine = ShakeInCircularShapeRoutine();
        StartCoroutine(shakingRoutine);
    }

    IEnumerator ShakingRoutine()
    {
        Vector3 _originalPos = cristalHolder.position;
        float _shakeMagnitude = 10f;
        while (true)
        {
            cristalHolder.position = _originalPos + Random.insideUnitSphere * _shakeMagnitude * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ShakeInCircularShapeRoutine()
    {
        Vector3 _center = cristalHolder.position;
        float _radius = 0.1f;
        float _angle = 0.0f;
        float _moveSpeed = 0.1f;
        float _timeToWait = 0.01f;

        while (true)
        {
            _angle += Time.deltaTime * 50.0f;
            Vector3 offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * _radius;
            Vector3 _newPos = _center + offset;

            while (Vector3.Distance(cristalHolder.position, _newPos) > 0.01f)
            {
                cristalHolder.position = Vector3.MoveTowards(cristalHolder.position, _newPos, _moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(_timeToWait);
        }
    }
}
