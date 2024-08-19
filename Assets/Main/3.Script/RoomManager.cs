using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class RoomManager : NetworkManager
{
    public static RoomManager instance = null;

    //private Dictionary<NetworkConnection, string> Room_Dic = new Dictionary<NetworkConnection, string>();

    [SerializeField] private GameObject LobbyPrefabs;
    [SerializeField] private GameObject OthelloPlayer;
    [SerializeField] private GameObject GomokuPlayer;
    [SerializeField] private GameObject HitChipsPlayer;

    //private readonly string Othello = "Othello";
    //private readonly string Gomoku = "Gomoku";
    //private readonly string HitChips = "HitChips";

    [SerializeField] private eType ServerType;

    private string roomID;
    
    public int ConnectCount = 0;
    private Dictionary<NetworkConnection, int> clientConnection = new Dictionary<NetworkConnection, int>();

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        ConnectCount++;
        clientConnection[conn] = ConnectCount;
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        clientConnection.Remove(conn);
        
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        GameObject player = conn.identity.gameObject;
        Othello_Controller othelloplayer = player.GetComponent<Othello_Controller>();
        players[conn] = othelloplayer;
        if(othelloplayer != null)
        {
            othelloplayer.SetConnectionOrder(ConnectCount);
        }
    }
    private Dictionary<NetworkConnection,Othello_Controller> players = new Dictionary<NetworkConnection, Othello_Controller>();
    public void CheckOthello_AllPlayersReady()
    {

        foreach (var player in players.Values)
        {
            if (!player.isReady)
            {
                return;
            }
        }
        foreach (var player in players.Values)
        {
            player.isStart = true;
        }
    }

    



    //[Command]
    //public void CmdJoinRoom(NetworkConnection conn, string RoomID)
    //{
    //    roomID = RoomID;
    //    Room_Dic[conn] = roomID;
    //}

    //public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    //{
    //    if(Room_Dic.TryGetValue(conn, out roomID))
    //    {
    //        SetGame(roomID);
    //    }
    //    base.OnServerAddPlayer(conn);
    //}

    public override void Awake()
    {
        base.Awake();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (ServerType.Equals(eType.Server))
            {
                StartServer();
            }
            else
            {
                StartClient();
            }
            //networkAddress = SQL_Manager.instance.ServerIP;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        
    }



    //public void SetGame(string roomID)
    //{
    //    string gameType = SQL_Manager.instance.FindRoomType(roomID);
    //    switch (gameType)
    //    {
    //        case "Othello":
    //            playerPrefab = OthelloPlayer;
    //            break;
    //        case "Gomoku":
    //            playerPrefab = GomokuPlayer;
    //            break;
    //        case "HitChips":
    //            playerPrefab = HitChipsPlayer;
    //            break;
    //        default:
    //            playerPrefab = LobbyPrefabs;
    //            break;
    //    }
    //}


    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        if (NetworkClient.isConnected)
        {
            SQL_Manager.instance.UdateLogout();
        }
    }
}
