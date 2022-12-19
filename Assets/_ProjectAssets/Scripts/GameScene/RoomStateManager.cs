using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomStateManager : MonoSingleton<RoomStateManager>
{
    public static event Action<IRoomState> OnStateUpdated;

    public PUNGameRoomManager photonManager;
    [Header("Player")]
    public GameObject playerPrefab;
    public GameObject playerUIPrefab;

    
    [Header("Bots")]
    public GameObject botPlayerPrefab;

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
    
    [HideInInspector]
    public bool isMultiplayer;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Init();
    }

    private void OnEnable()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();

        PUNRoomUtils.onPlayerLeft += OnPlayerLeft;
    }


    private void OnDisable()
    {
        PUNRoomUtils.onPlayerLeft -= OnPlayerLeft;
    }

    private void OnDestroy()
    {
        LeanTween.cancelAll();
    }
    private void Init()
    {
        MapsManager.Instance.CreateMap();
        if (isMultiplayer)
        {
            SetState(new WaitingForAllPlayersToJoinState()); ;
            photonView.RPC("OnPlayerSceneLoaded", RpcTarget.All);
        }
        else
        {
            OnPlayerSceneLoaded_SinglePlayer();
        }
    }

    public void SetState(IRoomState state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        Debug.Log("Setting state " + state);

        currentState = state;
        currentState.Init(this);

        OnStateUpdated?.Invoke(state);
    }

    public void SetFirstPlayerTurn()
    {
        roundNumber++;
        if (!isMultiplayer || PhotonNetwork.LocalPlayer.IsMasterClient)
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
        if (!isMultiplayer) return true;

        return (lastPlayerRound == 0 && PhotonNetwork.LocalPlayer.IsMasterClient) ||
            (lastPlayerRound == 1 && !PhotonNetwork.LocalPlayer.IsMasterClient);
    }

    private void OnPlayerLeft()
    {
        if(currentState is ResolvingGameState)
        {
            return;
        }
        SetState(new ResolvingGameState(PhotonNetwork.LocalPlayer.IsMasterClient ? GameResolveState.PLAYER_1_WIN : GameResolveState.PLAYER_2_WIN));
    }


    public void TryStartNextRound()
    {
        if (!isMultiplayer || PhotonNetwork.IsMasterClient)
        {
            GameResolveState resolveState = PlayerManager.Instance.GetWinnerByDeath();
            if (resolveState != GameResolveState.NO_WIN)
            {
                photonView.RPC("StartResolveGame", RpcTarget.All, resolveState);
            }
            else
            {
                int nextRound = roundNumber;
                if(lastPlayerRound == (sceneInfo.usersInScene - 1))
                {
                    nextRound += 1;
                }
                if (nextRound > ConfigurationManager.Instance.Config.GetMaxNumberOfRounds())
                {
                    resolveState = PlayerManager.Instance.GetWinnerByHealth();

                    SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "StartResolveGame", RpcTarget.All, resolveState);
                    return;
                }

                if (sceneInfo.usersInScene == 1)
                {
                    SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "StartNextRound", RpcTarget.All, 0, nextRound);
                }
                else
                {
                    SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "StartNextRound", RpcTarget.All, (lastPlayerRound + 1) % 2, nextRound);
                }
            }
        }
    }

    public void SetProjectileLaunchedState(float waitBeforeEndTurn)
    {
        SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "StartProjectileLaunchedState", RpcTarget.All, waitBeforeEndTurn);
    }

    public void SendRetreatRPC()
    {
        int isMaster = !isMultiplayer? 1 : (PhotonNetwork.LocalPlayer.IsMasterClient ? 0 : 1);
        SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "Retreat", RpcTarget.MasterClient, isMaster);
    }

    public void SinglePlayerReturnMainMenu()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void LoadAfterGameScene(GameResolveState state)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("AfterGame");
        }else if (!isMultiplayer)
        {
            SinglePlayerReturnMainMenu();
        }
    }

    [PunRPC]
    public void StartProjectileLaunchedState(float waitBeforeEndTurn)
    {
        if (ConfigurationManager.Instance.Config.GetGameType() == Anura.ConfigurationModule.ScriptableObjects.GameType.TUTORIAL && !TutorialManager.Instance.finished)
        {
            SetState(new GamePausedState());
        }
        else
        {
            SetState(new ProjectileLaunchedState(waitBeforeEndTurn));
        }
    }

    [PunRPC]
    public void StartNextRound(int playerNumber, int roundNumber)
    {
        this.roundNumber = roundNumber;

        if (ConfigurationManager.Instance.Config.GetGameType() == Anura.ConfigurationModule.ScriptableObjects.GameType.TUTORIAL)
        {
            SetState(new MyTurnState());
            return;
        }

        if (!isMultiplayer)
        {
            if (playerNumber == 0)
            {
                SetState(new MyTurnState());
            }
            else
            {
                Debug.Log("Bot turn!");
                SetState(new BotTurnState());
            }
            return;
        }

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
    public void OnPlayerSceneLoaded()
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

    private void OnPlayerSceneLoaded_SinglePlayer()
    {
        if (ConfigurationManager.Instance.Config.GetGameType() == Anura.ConfigurationModule.ScriptableObjects.GameType.TUTORIAL)
        {
            sceneInfo.usersInScene = 1;
        }
        else
        {
            //Me + bot;
            sceneInfo.usersInScene = 2;
        }
        OnAllPlayersJoinedScene();
    }

    [PunRPC]
    public void OnAllPlayersJoinedScene()
    {
        SetState(new StartingGameState());
    }

    [PunRPC]
    public void StartResolveGame(GameResolveState state)
    {
        if (currentState is ResolvingGameState)
        {
            return;
        }
        SetState(new ResolvingGameState(state));
    }

    [PunRPC]
    public void Retreat(int playerIdx)
    {
        GameResolveState resolveState = PlayerManager.Instance.GetWinnerByLoserIndex(playerIdx);
        SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "StartResolveGame", RpcTarget.All, resolveState);
    }
}
