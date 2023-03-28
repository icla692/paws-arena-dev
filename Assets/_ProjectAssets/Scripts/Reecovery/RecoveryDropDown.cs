using System;
using UnityEngine;

public class RecoveryDropDown : MonoBehaviour
{
    [SerializeField] GameObject healMessageHolder;

    float animationLength = 0.1f;
    bool isOpen;

    private void Start()
    {
        Close();
    }

    public void HandleClick()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Show();
        }
    }

    public void Close()
    {
        gameObject.LeanScale(Vector3.zero, animationLength);
        isOpen = false;
    }

    public void Show()
    {
        gameObject.LeanScale(Vector3.one, animationLength);
        isOpen = true;
    }

    public void Heal()
    {
        if (GameState.selectedNFT.CanFight)
        {
            return;
        }

        if (ValuablesManager.Instance.Milk > 0)
        {
            ValuablesManager.Instance.Milk--;
            GameState.selectedNFT.RecoveryEndDate = DateTime.UtcNow;
            //TODO tell server that player used milk to recover kittie
        }
        else
        {
            healMessageHolder.SetActive(true);
        }
        Close();
    }

    public void BuyMilk()
    {
        //TODO buy milk
        Close();
    }
}
