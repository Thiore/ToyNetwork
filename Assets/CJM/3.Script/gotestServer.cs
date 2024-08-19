using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mono.CecilX;

public enum eType
{
    Server = 0,
    Client
}

public class gotestServer : MonoBehaviour
{
    public eType type;

    private NetworkManager manager;
    private GameObject board;

    private void Start()
    {
        manager = GetComponent<NetworkManager>();
        if (type.Equals(eType.Server))
        {
            Start_Server();
        }
        else
        {
            Start_Client();
        }
    }

    public void Start_Server()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WEBgl server ¾ÈµÊ");
        }
        else
        {
            NetworkManager.singleton.StartServer();
            Debug.Log($"{manager.networkAddress} Start Server");
            NetworkServer.OnConnectedEvent += (NetworkConnectionToClient) =>
            {
                Debug.Log($"New Client Connect : {NetworkConnectionToClient.address}");

            };
            NetworkServer.OnDisconnectedEvent += (NetworkConnectionToClient) => { Debug.Log($"Client DisConnect : {NetworkConnectionToClient.address}"); };

            BoardGO();
            NetworkServer.Spawn(this.board);
        }
    }

    public void NetworkTest(NetworkConnection conn)
    {
        NetworkServer.Spawn(board.gameObject, conn);
    }



    public void BoardGO()
    {
        var boardfab = Resources.Load("Prefabs/Gogame_board") as GameObject;
        var board = Instantiate(boardfab).transform.GetComponent<Board>();
        var iden = board.GetComponent<NetworkIdentity>();
        NetworkServer.Spawn(board.gameObject, iden.connectionToClient);
        board.InitBoard();
        this.board = board.gameObject;
       
    }

    public void Start_Client()
    {
        manager.StartClient();
        Debug.Log($"{manager.networkAddress} StartClient");

        //if(SQL_Manager.instance.SetIP())
        //{
        //    manager.networkAddress = SQL_Manager.instance.ip;
        //    manager.StartClient();
        //    Debug.Log($"{manager.networkAddress} StartClient");
        //}
        //else
        //{
        //    Debug.Log("½ÇÆÐ");
        //}
    }

    

    private void OnApplicationQuit()
    {
        if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
        if (NetworkServer.active)
        {
            manager.StopServer();
        }
    }



}
