using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using Photon.Pun;
using System;
using UnityEngine;

public class RoomStateManager : MonoSingleton<RoomStateManager>
{
    public static event Action<IRoomState> OnStateUpdated;

    public PUNGameRoomManager photonManager;
    public GameObject playerPrefab;
    public GameObject playerUIPrefab;
    public Transform playerUIParent;

    [HideInInspector]
    public GameSceneMasterInfo sceneInfo = new GameSceneMasterInfo();
    [HideInInspector]
    public int lastPlayerRound = -1;
    [HideInInspector]
    public PhotonView photonView;

    [HideInInspector]
    public IRoomState currentState;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Init();
    }

    private void OnEnable()
    {
        PUNRoomUtils.onPlayerLeft += OnPlayerLeft;
    }


    private void OnDisable()
    {
        PUNRoomUtils.onPlayerLeft -= OnPlayerLeft;
    }

    private void Init()
    {
        SetState(new WaitingForAllPlayersToJoinState()); ;
        photonView.RPC("OnPlayerSceneLoaded", RpcTarget.All);
    }


    public void SetState(IRoomState state)
    {
        if(currentState != null)
        {
            currentState.OnExit();
        }

        currentState = state;
        currentState.Init(this);

        OnStateUpdated?.Invoke(state);
    }


    public void SetFirstPlayerTurn()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            SetState(new MyTurnState());
        }
        else
        {
            SetState(new OtherPlayerTurnState());
        }
    }

    private void OnPlayerLeft()
    {
        SetState(new ResolvingGameState(PhotonNetwork.LocalPlayer.IsMasterClient ? GameResolveState.PLAYER_1_WIN : GameResolveState.PLAYER_2_WIN));
    }

    public Color GetMyColor()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient ? ConfigurationManager.Instance.Config.GetFirstTeamColor() : ConfigurationManager.Instance.Config.GetSecondTeamColor();
    }

    public Color GetOtherColor()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient ? ConfigurationManager.Instance.Config.GetSecondTeamColor() : ConfigurationManager.Instance.Config.GetFirstTeamColor();
    }


    [PunRPC]
    private void StartNextRound()
    {
        //Single Player mode
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if(sceneInfo.usersInScene == 1)
            {
                SetState(new MyTurnState());
                return;
            }
        }


        // 1v1 mode
        if (lastPlayerRound == 0)
        {
            SetState(PhotonNetwork.LocalPlayer.IsMasterClient ? new OtherPlayerTurnState() : new MyTurnState());
        }
        else
        {
            SetState(PhotonNetwork.LocalPlayer.IsMasterClient ? new MyTurnState() : new OtherPlayerTurnState());
        }
    }

    [PunRPC]
    private void OnPlayerSceneLoaded()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            sceneInfo.usersInScene++;

            Debug.Log($"Players in scene: {sceneInfo.usersInScene} / {PhotonNetwork.CurrentRoom.PlayerCount}");
            if (sceneInfo.usersInScene == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC("OnAllPlayersJoinedScene", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OnAllPlayersJoinedScene()
    {
        SetState(new StartingGameState());
    }

    [PunRPC]
    private void StartResolveGame(GameResolveState state)
    {
        SetState(new ResolvingGameState(state));
    }
}
