using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 

public class GameManager : NetworkBehaviour
{
    public GameObject coin_Spawn;

    [Command]
    public void CoinSpawn()
    {
        //�������� ������Ʈ ����
        GameObject Coin_obj_BSJ = Instantiate(coin_Spawn, Vector3.zero, Quaternion.identity);

        //��Ʈ��ũ�� ������Ʈ �߰�
        NetworkServer.Spawn(Coin_obj_BSJ);

        //������ ������Ʈ�� Identity ��������
        NetworkIdentity identity = Coin_obj_BSJ.GetComponent<NetworkIdentity>();

        //Ŭ�󿡰� ���� �ο�
        if (identity != null)
        {
            identity.AssignClientAuthority(connectionToClient);
        }
    }

    //Ŭ�� ���� ���� �޼��� (Ȥ�� �𸣴ϱ� �̸� �����α�)
    public void RemoveClientAuthority(NetworkIdentity identity)
    {
        if (identity != null)
        {
            identity.RemoveClientAuthority();
        }
    }
}
