using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RecoveryHandler : MonoBehaviour
{
    [SerializeField] GameObject recoveryHolder; //used for kitties in selection menu
    [SerializeField] Image recoveryFillAmount;

    IEnumerator recoveryRoutine;
    public static int RecoveryInMinutes = 30;
    string kittyImageUrl;

    public void ShowRecovery(DateTime _endDate,string _kittyImageUrl)
    {
        kittyImageUrl = _kittyImageUrl;
        if (_endDate < DateTime.UtcNow)
        {
            DataManager.Instance.PlayerData.RemoveRecoveringKittie(_kittyImageUrl);
            return;
        }
        if (recoveryHolder != null)
        {
            recoveryHolder.gameObject.SetActive(true);
        }
        recoveryRoutine = RecoveryRoutine(_endDate);
        StartCoroutine(recoveryRoutine);
    }

    public void StopRecovery()
    {
        if (recoveryRoutine == null)
        {
            return;
        }

        StopCoroutine(recoveryRoutine);
        recoveryRoutine = null;
        recoveryFillAmount.fillAmount = 1;
        DataManager.Instance.PlayerData.RemoveRecoveringKittie(kittyImageUrl);
    }

    IEnumerator RecoveryRoutine(DateTime _endDate)
    {
        double _totalSecoundsForRecovery = 60 * RecoveryInMinutes;
        double _secoundsPassed;
        float _fillAmount = 0;
        DateTime _startDate = _endDate.AddSeconds(-_totalSecoundsForRecovery);
        while (_fillAmount < 1)
        {
            _secoundsPassed = (DateTime.UtcNow - _startDate).TotalSeconds;
            _fillAmount = (float)(_secoundsPassed / _totalSecoundsForRecovery);
            recoveryFillAmount.fillAmount = _fillAmount;
            yield return new WaitForSeconds(1);
        }

        HideRecovery();
    }

    void HideRecovery()
    {
        if (recoveryHolder != null)
        {
            DataManager.Instance.PlayerData.RemoveRecoveringKittie(kittyImageUrl);
            recoveryHolder.SetActive(false);
        }
    }
}
