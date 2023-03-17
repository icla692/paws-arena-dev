using System;
using UnityEngine;

public class EmojiVisual : MonoBehaviour
{
    Action finishedAnimation;
    Animator animationController;

    private void Awake()
    {
        animationController = GetComponent<Animator>();
    }

    public void Setup(EmojiSO _emoji, Action _callback = null)
    {
        animationController.Play(_emoji.AnimationNime);
        finishedAnimation = _callback;
    }

    //called from animation event
    void FinishAnimation()
    {
        finishedAnimation?.Invoke();
        Destroy(gameObject);
    }
}
