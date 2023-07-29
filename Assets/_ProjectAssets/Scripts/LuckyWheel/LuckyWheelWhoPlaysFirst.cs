using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class LuckyWheelWhoPlaysFirst : MonoBehaviour
{
    public static bool DoIPlayFirst;
    
    [SerializeField] AnimationCurve spinCurve;
    [SerializeField] AnimationCurve endSpinCurve;
    [SerializeField] float spinSpeed;
    [SerializeField] RectTransform pointerHolder;
    
    private LuckyWheelRewardSO choosenPlayer;
    private float speed;
    private PhotonView photonView;
    private List<SyncPlayerPlatformBehaviour> playerPlatforms;

    private void OnEnable()
    {
        ChooseStartingPlayer();
        playerPlatforms = FindObjectsOfType<SyncPlayerPlatformBehaviour>().ToList();
        foreach (var _playerPlatform in playerPlatforms)
        {
            _playerPlatform.gameObject.SetActive(false);
        }
    }

    private void ChooseStartingPlayer()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        DoIPlayFirst = Random.Range(0,2)==0;
        float _targetZ = DoIPlayFirst ? Random.Range(20, 170) : Random.Range(190, 350);
        
        Spin(_targetZ,DoIPlayFirst);

        if (!(PhotonNetwork.CurrentRoom==null|| PhotonNetwork.CurrentRoom.PlayerCount==1))
        {
            photonView.RPC(nameof(Spin),RpcTarget.Others,_targetZ,!DoIPlayFirst);
        }
    }

    IEnumerator SpinRoutine(float _targetRotationZ)
    {
        pointerHolder.eulerAngles = Vector3.zero;
        float _spinDuration = UnityEngine.Random.Range(2, 4f);
        float _timePassed = 0;

        while (_timePassed < _spinDuration)
        {
            _timePassed += Time.deltaTime;
            float _pointOnCurve = _timePassed / _spinDuration;
            float _zIncrement = Time.deltaTime * speed * spinCurve.Evaluate(_pointOnCurve);

            Vector3 _rotaiton = pointerHolder.eulerAngles;
            _rotaiton.z += _zIncrement;
            pointerHolder.eulerAngles = _rotaiton;

            yield return null;
        }

        // End spin
        float _targetedZ = _targetRotationZ;
        float _currentZRotation = pointerHolder.eulerAngles.z;
        int _additionalFullSPins = 1;
        _targetedZ -= (360 * _additionalFullSPins);
        float _distanceTraveled = 0;
        float _distanceToTravel = _targetedZ - _currentZRotation;
        float _pointAtCurve = 0;

        do
        {
            if (_distanceTraveled == 0)
            {
                _pointAtCurve = 0;
            }
            else
            {
                _pointAtCurve = _distanceTraveled / _distanceToTravel;
            }

            float _speedModifier = endSpinCurve.Evaluate(_pointAtCurve);
            float _movingDistanceThisFrame = Mathf.MoveTowards(_currentZRotation, _targetedZ, Time.deltaTime * _speedModifier * speed);
            _movingDistanceThisFrame = _currentZRotation - _movingDistanceThisFrame;
            _distanceTraveled += _movingDistanceThisFrame;
            _currentZRotation += _movingDistanceThisFrame;
            pointerHolder.eulerAngles = new Vector3(pointerHolder.eulerAngles.x,
                                                    pointerHolder.eulerAngles.y,
                                                    _currentZRotation);
            yield return null;

        } while (_pointAtCurve < 1);

        // Snap position just in case
        pointerHolder.eulerAngles = new Vector3(pointerHolder.eulerAngles.x, pointerHolder.eulerAngles.y, _targetedZ);

        StartCoroutine(EndSpin());
    }

    private IEnumerator EndSpin()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        foreach (var _playerPlatform in playerPlatforms)
        {
            _playerPlatform.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    private void Spin(float _targetZ, bool _doIPlayFirst)
    {
        DoIPlayFirst = _doIPlayFirst;
        speed = spinSpeed;
        StartCoroutine(SpinRoutine(_targetZ));
    }
}
