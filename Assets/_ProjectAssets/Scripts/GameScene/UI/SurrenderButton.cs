using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurrenderButton : MonoBehaviour
{
    public void OnClick()
    {
        ModalsManager.Instance.ShowModal("Are you sure you want to surrender?", () =>
        {
            RoomStateManager.Instance.Retreat();
        });
    }
}
