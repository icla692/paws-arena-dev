using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratesManager : MonoBehaviour
{
    [SerializeField]
    private float intervalInSeconds = 30f;
    [SerializeField]
    private Vector2 minPos;
    [SerializeField]
    private Vector2 maxPos;

    private CratesConfig cratesConfig;
    private float lastTime;

    // Start is called before the first frame update
    void Start()
    {
        cratesConfig = ConfigurationManager.Instance.Crates;
        lastTime = Time.time;    
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;
        if(Time.time - lastTime >= intervalInSeconds)
        {
            lastTime = Time.time;

            SpawnCrate(cratesConfig.GetCrate());
        }
    }

    private void SpawnCrate(CrateConfig crateConfig)
    {
        Vector2 randomPos = new Vector2(UnityEngine.Random.Range(minPos.x, maxPos.x), UnityEngine.Random.Range(minPos.y, maxPos.y));
        PhotonNetwork.Instantiate(crateConfig.prefab.name, randomPos, Quaternion.identity);
    }
}
