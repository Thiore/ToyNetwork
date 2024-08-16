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
        //webGL�� ����� �� ���� ������
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL ���� ����� �� ����");
        }
        else
        {
            //��Ʈ��ũ �޼��� ���
            manager_BSJ.StartServer();
            // ���� ���忡�� � Ŭ���̾�Ʈ��(ip) ���Դ��� Ȯ��
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
