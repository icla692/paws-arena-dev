using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratesManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 minPos;
    [SerializeField]
    private Vector2 maxPos;

    private CratesConfig cratesConfig;

    // Start is called before the first frame update
    void Start()
    {
        cratesConfig = ConfigurationManager.Instance.Crates;

        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDestroy()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;

        int roundNumber = RoomStateManager.Instance.roundNumber;
        if (roundNumber == 6)
        {
            if (state is MyTurnState)
            {
                SpawnCrate(cratesConfig.GetCrate());
            }
        }else if(roundNumber == 12)
        {
            if(state is OtherPlayerTurnState)
            {
                SpawnCrate(cratesConfig.GetCrate());
            }
        }                
    }

    private void SpawnCrate(CrateConfig crateConfig)
    {
        Vector2 randomPos = new Vector2(UnityEngine.Random.Range(minPos.x, maxPos.x), UnityEngine.Random.Range(minPos.y, maxPos.y));
        PhotonNetwork.Instantiate(crateConfig.prefab.name, randomPos, Quaternion.identity);
    }
}
