using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalMovement : MonoBehaviour
{
    public float frequency;
    public Vector3 amplitude;

    void Update()
    {
        transform.localPosition = Mathf.Sin(Time.time * frequency) * amplitude;
    }
}
