using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public class RPC_Coin_BSJ : NetworkBehaviour
{
    [SerializeField] private GameObject coinPrefab; // ���� ������ ������ ����
    
    // Client�� Server�� Connect �Ǿ��� �� �ݹ� �Լ�
    public override void OnStartAuthority()
    {
        if (isLocalPlayer)
        {
            Debug.Log("OnStartAuthority ����");
            //CmdSpawnCoin();
        }
    }

    private void Update()
    {
        // �����̽��ٸ� ������ �� ���� ����
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            CmdSpawnCoin();
        }
    }

    [Command]
    private void CmdSpawnCoin()
    {
        GameObject coin = Instantiate(coinPrefab);
        NetworkServer.Spawn(coin, connectionToClient);  // ������ Ŭ���̾�Ʈ���� ���� �ο�
    }

    


}
