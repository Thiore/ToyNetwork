using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum Type
{
    Server_BSJ = 0,
    Client_BSJ
}

public class Server_Checker_BSJ : NetworkBehaviour
{
    public Type type_BSJ;
    private NetworkManager manager_BSJ;

    private void Start()
    {
        manager_BSJ = GetComponent<NetworkManager>();
        if (type_BSJ == Type.Server_BSJ)
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
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL 서버 사용할 수 없음");
        }
        else
        {
            manager_BSJ.StartServer();
            NetworkServer.OnConnectedEvent += OnClientConnected;
        }
    }

    private void OnClientConnected(NetworkConnection conn)
    {
        Debug.Log($"Client connected: {conn.connectionId}");

        int clientIndex = NetworkServer.connections.Count - 1;
        PlayerType assignedColor = (clientIndex % 2 == 0) ? PlayerType.White : PlayerType.Black;

        StartCoroutine(AssignColorWithDelay(conn, assignedColor));
    }

    private IEnumerator AssignColorWithDelay(NetworkConnection conn, PlayerType assignedColor)
    {
        yield return new WaitForSeconds(1); // 잠시 대기
        if (conn.identity == null)
        {
            Debug.LogError($"conn.identity is still null for client {conn.connectionId} after delay. TargetAssignColor cannot be called.");
            yield break;
        }
        TargetAssignColor(conn, assignedColor);
    }

    [TargetRpc]
    private void TargetAssignColor(NetworkConnection conn, PlayerType color)
    {
        Debug.Log($"TargetAssignColor called on client {conn.connectionId}, assigned color: {color}");

        var selectColor = conn.identity.GetComponent<Select_color>();

        if (selectColor == null)
        {
            Debug.LogError("Select_color component not found on the client!");
            return;
        }

        // Assign the player type
        selectColor.playerType = color;

        // Log to confirm the player type assignment
        Debug.Log($"Client {conn.connectionId} - playerType set to {selectColor.playerType}");

        // Apply the material and layer based on the assigned color
        selectColor.SetChipMaterial();
        selectColor.SetPlayerLayer();

        // Additional Debugging
        Debug.Log($"Client {conn.connectionId} - Chip material set to {selectColor.chipRenderer.material.name}");
        Debug.Log($"Client {conn.connectionId} - Layer set to {LayerMask.LayerToName(selectColor.gameObject.layer)}");
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