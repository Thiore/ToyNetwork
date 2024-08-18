using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum eType
{
    Server = 0,
    Client
}

public class gotestServer : MonoBehaviour
{
    public eType type;

    private NetworkManager manager;

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

        }
    }


    public void BoardGO()
    {
        var boardfab = Resources.Load("Prefabs/Gogame_board") as GameObject;
        var board = Instantiate(boardfab).transform.GetComponent<Board>();
        var iden = board.GetComponent<NetworkIdentity>();
        board.InitBoard();
        NetworkServer.Spawn(board.gameObject, iden.connectionToClient);
    }

    public void Start_Client()
    {
        manager.StartClient();
        Debug.Log(NetworkManager.singleton.isNetworkActive);
        Debug.Log(NetworkServer.active);
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
