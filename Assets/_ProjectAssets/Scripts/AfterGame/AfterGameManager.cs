using Anura.Templates.MonoSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.colorfulcoding.AfterGame
{
    public class AfterGameManager : MonoSingleton<AfterGameManager>
    {
        private void Start()
        {
            Debug.Log("State: " + GameState.gameResolveState);
        }
    }
}