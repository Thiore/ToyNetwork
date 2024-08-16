using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum Type
{
    Server_BSJ = 0,
    Client_BSJ
}

public class Server_Checker_BSJ : MonoBehaviour
{
    public Type type_BSJ;

    private NetworkManager manager_BSJ;

    private void Start()
    {
        manager_BSJ = GetComponent<NetworkManager>();
        if (type_BSJ.Equals(Type.Server_BSJ))
        {
            Start_Server_BSJ();
        }
        else
        {
            Start_Client_BSJ();
        }
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
            manager_BSJ.StartServer();
            // 서버 입장에서 어떤 클라이언트가(ip) 들어왔는지 확인
            Debug.Log($"{manager_BSJ.networkAddress} StartServer");
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
        manager_BSJ.StartClient();
        Debug.Log($"{manager_BSJ.networkAddress} StartClient");
    }
    private void OnApplicationQuit()
    {
        if (NetworkClient.isConnected)
        {
            manager_BSJ.StopClient();
        }
        if (NetworkServer.active)
        {
            manager_BSJ.StopServer();
        }
    }
}
