using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGameNetworkManager : NetworkManager
{
    public static GoGameNetworkManager instance = null;

    public int ConnectCount = 0;
    private Dictionary<NetworkConnection, int> clientConnection = new Dictionary<NetworkConnection, int>();

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        ConnectCount++;
        Debug.Log(ConnectCount);
        clientConnection[conn] = ConnectCount;
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        clientConnection.Remove(conn);
    }

    private Dictionary<NetworkConnection, Player> players = new Dictionary<NetworkConnection, Player>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        GameObject player = conn.identity.gameObject;
        Player goplayer = player.GetComponent<Player>();
        players[conn] = goplayer;
        if (goplayer != null)
        {
            goplayer.SetConnectionOrder(ConnectCount);
        }

    }

}
