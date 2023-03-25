using System.Collections;
using UnityEngine;

public class LuckyWheelRewardDisplay : MonoBehaviour
{
    const string IDLE_ANIMATION_KEY = "Idle";
    const string SHAKING_ANIMATION_KEY = "Shaking";

    [SerializeField] float scaleFactor;
    [SerializeField] GameObject shadowHolder;
    [SerializeField] Transform cristalHolder;
    [SerializeField] LuckyWheelRewardType rewardType;

    public LuckyWheelRewardType RewardType => rewardType;

    IEnumerator shakingRoutine;

    public void ResetDisplay()
    {
        if (shakingRoutine != null)
        {
            StopCoroutine(shakingRoutine);
            shakingRoutine = null;
        }
        cristalHolder.localScale = new Vector3(1, 1, 1);
        shadowHolder.SetActive(false);
    }

    public void ShowShadow()
    {
        shadowHolder.SetActive(true);
    }

    public void Shake()
    {
        cristalHolder.localScale *= scaleFactor;
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
        Vector3 originalPos = cristalHolder.position;

        float shakeMagnitude = 10f;

        while (true)
        {
            float x = originalPos.x + Mathf.Sin(Time.time * 50) * shakeMagnitude * Time.deltaTime;
            float y = originalPos.y + Mathf.Cos(Time.time * 50) * shakeMagnitude * Time.deltaTime;
            float z = originalPos.z;

            cristalHolder.position = new Vector3(x, y, z);
            yield return null;
        }
    }
}
