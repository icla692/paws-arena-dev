using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterGameCharacter : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        StartCoroutine(CharacterAnimationCoroutine());
        Debug.Log("State: " + GameState.gameResolveState);
    }

    private IEnumerator CharacterAnimationCoroutine()
    {
        int checkIfIWon = GameResolveStateUtils.CheckIfIWon(GameState.gameResolveState);
        yield return new WaitForSeconds(1.5f);
        if(checkIfIWon < 0)
        {
            animator.SetBool("isDead", true);
        }
    }
}
