using Anura.Templates.MonoSingleton;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlatformsBehaviour : MonoSingleton<SyncPlatformsBehaviour>
{
    public GameObject syncPlayerPlatformPrefab;
    public Vector3 myPos = new Vector3(-1.847f, -.116f, 0);
    public Vector3 theirPos = new Vector3(9.47f, -.116f, 0);
    // Start is called before the first frame update
    void Start()
    {
        var go = PhotonNetwork.Instantiate(syncPlayerPlatformPrefab.name, myPos, Quaternion.identity);
    }
}
