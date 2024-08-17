using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class RoomManager : NetworkRoomManager
{
    public static RoomManager instance = null;

    [SerializeField] private GameObject OthelloPlayer;
    [SerializeField] private GameObject GomokuPlayer;
    [SerializeField] private GameObject HitChipsPlayer;

    private readonly string Othello = "Othello";
    private readonly string Gomoku = "Gomoku";
    private readonly string HitChips = "HitChips";


    public override void Awake()
    {
        base.Awake();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //networkAddress = SQL_Manager.instance.ServerIP;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
    }

    public override void OnClientNotReady()
    {
        base.OnClientNotReady();
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
    }

    public override void OnClientTransportException(Exception exception)
    {
        base.OnClientTransportException(exception);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnGUI()
    {
        base.OnGUI();
    }

    public override void OnRoomClientConnect()
    {
        base.OnRoomClientConnect();
    }

    public override void OnRoomClientDisconnect()
    {
        base.OnRoomClientDisconnect();
    }

    public override void OnRoomClientEnter()
    {
        base.OnRoomClientEnter();
    }

    public override void OnRoomClientExit()
    {
        base.OnRoomClientExit();
    }

    public override void OnRoomClientSceneChanged()
    {
        base.OnRoomClientSceneChanged();
    }

    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnRoomServerAddPlayer(conn);
    }

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        return base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
    }

    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        return base.OnRoomServerCreateRoomPlayer(conn);
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerDisconnect(conn);
    }

    public override void OnRoomServerPlayersNotReady()
    {
        base.OnRoomServerPlayersNotReady();
    }

    public override void OnRoomServerPlayersReady()
    {
        base.OnRoomServerPlayersReady();
    }

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    public override void OnRoomStartClient()
    {
        base.OnRoomStartClient();
    }

    public override void OnRoomStartHost()
    {
        base.OnRoomStartHost();
    }

    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();
    }

    public override void OnRoomStopClient()
    {
        base.OnRoomStopClient();
    }

    public override void OnRoomStopHost()
    {
        base.OnRoomStopHost();
    }

    public override void OnRoomStopServer()
    {
        base.OnRoomStopServer();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
    }

    public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
    {
        base.OnServerError(conn, error, reason);
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnServerTransportException(NetworkConnectionToClient conn, Exception exception)
    {
        base.OnServerTransportException(conn, exception);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    

    public override void ReadyStatusChanged()
    {
        base.ReadyStatusChanged();
    }


    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }

    public void SetGame(string type)
    {
        switch (type)
        {
            case "Othello":
                playerPrefab = OthelloPlayer;
                GameplayScene = Othello;
                break;
            case "Gomoku":
                playerPrefab = GomokuPlayer;
                GameplayScene = Gomoku;
                break;
            case "HitChips":
                playerPrefab = HitChipsPlayer;
                GameplayScene = HitChips;
                break;
        }
    }
    

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        if (NetworkClient.isConnected)
        {
            SQL_Manager.instance.UdateLogout();
        }
    }
}
