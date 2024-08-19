using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RPC_GO_BSJ : NetworkBehaviour
{
    [SerializeField] private GameObject goPrefab; // ���� ������ ������ ����

    // Client�� Server�� Connect �Ǿ��� �� �ݹ� �Լ�
    public override void OnStartAuthority()
    {
        Debug.Log("OnStartClient �޼��� ȣ���");
        // ������ �ִ� ���� �÷��̾��� ���
        if (isLocalPlayer)
        {
            Debug.Log("RPC_GO_BSJ ����");

            // ���� ��ġ�� ȸ���� ������ ����
            PositionAndRotation(transform.position, transform.rotation);
        }
        else
        {
            Debug.Log("isLocalPlayer�� false��");
        }
    }
    private void Update()
    {
        
    }
    //[Command]
    //public void CmdAssignAuthority(NetworkIdentity obj, NetworkConnectionToClient conn)
    //{
    //    obj.AssignClientAuthority(conn);
    //    Debug.Log("Ŭ�� ���� ������");
    //}

    [Command]
    private void PositionAndRotation(Vector3 position, Quaternion rotation)
    {
        UpdatePositionAndRotation(position, rotation);
    }

    [ClientRpc]
    private void UpdatePositionAndRotation(Vector3 position, Quaternion rotation)
    {
        if (goPrefab != null)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
        else
        {
            Debug.LogError("goPrefab�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
    public void SyncPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        if (!isLocalPlayer)
        {
            PositionAndRotation(position, rotation);
        }
    }
}
