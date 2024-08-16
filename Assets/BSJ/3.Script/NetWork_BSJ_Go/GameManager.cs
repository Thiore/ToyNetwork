using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public GameObject coinPrefab;

    [Command]
    public void CmdSpawnCoin(Vector3 position)
    {
        // �������� ������Ʈ ����
        GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

        // ��Ʈ��ũ�� ������Ʈ �߰�
        NetworkServer.Spawn(coin);

        // ������ ������Ʈ�� NetworkIdentity ��������
        NetworkIdentity identity = coin.GetComponent<NetworkIdentity>();

        // Ŭ���̾�Ʈ���� ���� �ο�
        if (identity != null)
        {
            RpcAssignClientAuthority(identity.netId);
        }
    }

    // Ŭ���̾�Ʈ���� ���� �ο� ��û
    [ClientRpc]
    private void RpcAssignClientAuthority(uint netId)
    {
        NetworkIdentity identity = NetworkServer.spawned[netId];
        if (identity != null)
        {
            identity.AssignClientAuthority(connectionToClient);
        }
    }

    // Ŭ���̾�Ʈ ���� ���� �޼��� (Ȥ�� �ʿ��ϴٸ� ���)
    public void ReleaseClientAuthority(NetworkIdentity identity)
    {
        if (identity != null)
        {
            identity.RemoveClientAuthority();
        }
    }
}
