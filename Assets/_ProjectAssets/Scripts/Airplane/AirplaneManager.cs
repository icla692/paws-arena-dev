using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneManager : MonoBehaviour
{
    [Header("Internals")]
    public GameObject airplaneParent;
    public Animator airplaneAnimator;

    [Header("Params")]
    public float travelTime = 2f;
    public float distanceToTargetThreshold = 5f;
    public Transform startPos;
    public Transform endPos;
    public GameObject bombPrefab;
    public Transform bombStartPos;

    [Header("TEST")]
    public Transform targetToHit;
    
    private float _speed;
    private Vector3 _direction;
    private bool routineActive = false;
    private bool shouldDropBomb = false;

    private void FixedUpdate()
    {
        if (!routineActive) return;

        airplaneParent.transform.position += _direction.normalized * _speed * Time.deltaTime;


        Debug.Log(Math.Abs(bombStartPos.transform.position.x - targetToHit.transform.position.x));
        if (shouldDropBomb && Math.Abs(bombStartPos.transform.position.x - targetToHit.transform.position.x) <= distanceToTargetThreshold)
        {
            StartDropBombAnimation();
            shouldDropBomb = false;
        }

        if(Vector3.Distance(airplaneParent.transform.position, endPos.transform.position) <= 5)
        {
            EndRoutine();
        }
    }

    private void StartRoutine()
    {
        routineActive = shouldDropBomb = true;
        airplaneParent.transform.position = startPos.transform.position;
        _speed = Vector3.Distance(startPos.transform.position, endPos.transform.position) / travelTime;
        _direction = endPos.transform.position - startPos.transform.position;
        Debug.Log($"{_direction.x} {_direction.y} {_direction.z}");
    }

    private void EndRoutine()
    {
        routineActive = false;
    }

    private void StartDropBombAnimation()
    {
        airplaneAnimator.SetTrigger("Drop");
    }

    public void DropBomb()
    {
        StartCoroutine(DropBombCoroutine());
    }
    private IEnumerator DropBombCoroutine()
    {
        var go = PhotonNetwork.Instantiate("Bullets/" + bombPrefab.name, bombStartPos.transform.position, Quaternion.identity);
        yield return new WaitForEndOfFrame();
        go.GetComponent<BulletComponent>().Launch(Vector3.zero, 0);

    }

    [ContextMenu("Test Routine")]
    public void TestRoutine()
    {
        StartRoutine();
    }
}
