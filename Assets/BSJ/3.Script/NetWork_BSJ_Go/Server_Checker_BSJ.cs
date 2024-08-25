using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum Type
{
    Server_BSJ = 0,
    Client_BSJ
}

public class Server_Checker_BSJ : NetworkManager
{
    public Type type_BSJ;
    private int addPlayerCount;
    private List<PutOn> players = new List<PutOn>();

    
    public List<GameObject> spawnedObj = new List<GameObject>();

    public override void Start()
    {
        
        if (type_BSJ.Equals(Type.Server_BSJ))
        {
            Start_Server_BSJ();
            NetworkServer.maxConnections = 2;
        }
        else
        {
            Start_Client_BSJ();
        }
        addPlayerCount = 0;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        addPlayerCount++;
        GameObject player = conn.identity.gameObject;
        PutOn ChipPlayer = player.GetComponent<PutOn>();
        players.Add(ChipPlayer);
        int chipType = addPlayerCount % 2;
            ChipPlayer.playerType = (PlayerType)chipType;

        GameObject coin = Instantiate(spawnPrefabs[1]);
        NetworkServer.Spawn(coin);
        coin.GetComponent<Coin>().players = players;

        if (NetworkServer.connections.Count.Equals(2))
        {
            GameObject manager = Instantiate(spawnPrefabs[2]);
            NetworkServer.Spawn(manager);

            for (int i = 0; i < players.Count; i++)
            {
                players[i].HitChipStart(players[0].netId);

            }
            
            //Coin.coin.CmdBounceCoin();
        }
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        players.Remove(conn.identity.GetComponent<PutOn>());
        base.OnServerDisconnect(conn);
        
    }
   

    public void Start_Server_BSJ()
    {
        //webGL은 사용할 수 없기 때문에
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL 서버 사용할 수 없음");
        }
        else
        {
            //네트워크 메서드 사용
            StartServer();
            // 서버 입장에서 어떤 클라이언트가(ip) 들어왔는지 확인
            Debug.Log($"{networkAddress} StartServer");
            NetworkServer.OnConnectedEvent +=
                (NetworkConnectionToClient) =>
                {
                    Debug.Log($"New Client Connect : {NetworkConnectionToClient.address}");
                };

            NetworkServer.OnDisconnectedEvent +=
                NetworkServer.OnConnectedEvent +=
                (NetworkConnectionToClient) =>
                {
                    Debug.Log($"New Client DisConnect : {NetworkConnectionToClient.address}");
                };
        }
    }
    public void Start_Client_BSJ()
    {
        StartClient();
        Debug.Log($"{networkAddress} StartClient");
    }
    public override void OnApplicationQuit()
    {
        if (NetworkClient.isConnected)
        {
            StopClient();
        }
        if (NetworkServer.active)
        {
            StopServer();
        }
    }
}
