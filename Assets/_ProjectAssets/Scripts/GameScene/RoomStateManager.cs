using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using Photon.Pun;
using System;
using UnityEngine;

public class RoomStateManager : MonoSingleton<RoomStateManager>
{
    public static event Action<IRoomState> OnStateUpdated;

    public PUNGameRoomManager photonManager;
    [Header("Player")]
    public GameObject playerPrefab;
    public GameObject playerUIPrefab;

    public Transform playerUIParent;
    [Header("Others")]
    public TrajectoryBehaviour trajectory;

    [HideInInspector]
    public GameSceneMasterInfo sceneInfo = new GameSceneMasterInfo();

    [HideInInspector]
    public PhotonView photonView;
    [HideInInspector]
    public int lastPlayerRound = -1;
    [HideInInspector]
    public int roundNumber = 1;

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
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = state;
        currentState.Init(this);

        OnStateUpdated?.Invoke(state);
    }

    public void SetFirstPlayerTurn()
    {
        roundNumber++;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            SetState(new MyTurnState());
        }
        else
        {
            SetState(new OtherPlayerTurnState());
        }
    }

    public bool WasMyRound()
    {
        return (lastPlayerRound == 0 && PhotonNetwork.LocalPlayer.IsMasterClient) ||
            (lastPlayerRound == 1 && !PhotonNetwork.LocalPlayer.IsMasterClient);
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


    public void TryStartNextRound()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameResolveState resolveState = PlayerManager.Instance.GetWinnerByDeath();
            if (resolveState != GameResolveState.NO_WIN)
            {
                photonView.RPC("StartResolveGame", RpcTarget.All, resolveState);
            }
            else
            {
                int nextRound = roundNumber;
                if(lastPlayerRound == 1)
                {
                    nextRound += 1;
                }
                if (nextRound > ConfigurationManager.Instance.Config.GetMaxNumberOfRounds())
                {
                    resolveState = PlayerManager.Instance.GetWinnerByHealth();
                    photonView.RPC("StartResolveGame", RpcTarget.All, resolveState);
                    return;
                }

                if (sceneInfo.usersInScene == 1)
                {
                    photonView.RPC("StartNextRound", RpcTarget.All, 0, nextRound);
                }
                else
                {
                    photonView.RPC("StartNextRound", RpcTarget.All, (lastPlayerRound + 1) % 2, nextRound);
                }
            }
        }
    }

    public void SetProjectileLaunchedState()
    {
        photonView.RPC("StartProjectileLaunchedState", RpcTarget.All);
    }

    public void Retreat()
    {
        photonView.RPC("Retreat", RpcTarget.All, PhotonNetwork.LocalPlayer.IsMasterClient ? 0 : 1);
    }

    [PunRPC]
    private void StartProjectileLaunchedState()
    {
        SetState(new ProjectileLaunchedState());
    }

    [PunRPC]
    private void StartNextRound(int playerNumber, int roundNumber)
    {
        this.roundNumber = roundNumber;
        if (playerNumber == 0)
        {
            SetState(PhotonNetwork.LocalPlayer.IsMasterClient ? new MyTurnState() : new OtherPlayerTurnState());
        }
        else
        {
            SetState(PhotonNetwork.LocalPlayer.IsMasterClient ? new OtherPlayerTurnState() : new MyTurnState());
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

    [PunRPC]
    private void Retreat(int playerIdx)
    {
        GameResolveState resolveState = PlayerManager.Instance.GetWinnerByLoserIndex(playerIdx);
        photonView.RPC("StartResolveGame", RpcTarget.All, resolveState);
    }
}
