using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance = null;

    public GameObject coinPrefab;

    private Text Log;
    private GameObject Panel;

    [SyncVar]
    public bool isTurn;
    [SyncVar]
    public bool isGameStart;
    [SyncVar]
    public bool isSetStart;

    private int Black;
    private int White;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        isTurn = true;
        isGameStart = false;
        isSetStart = false;
        Black = 0;
        White = 0;
        Panel = transform.GetChild(0).GetChild(0).gameObject;
        Log = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        Panel.SetActive(false);

    }

    public void SetCount(PlayerType type, int Count)
    {
        if(type.Equals(PlayerType.Black))
        {
            Black = Count;
        }
        else
        {
            White = Count;
        }
        if(isGameStart)
        {
            if(Black.Equals(0)||White.Equals(0))
            {
                isGameStart = false;
                Invoke("GameSet", 1f);
            }
            
        }
    }
    public void GameSet()
    {
        Panel.SetActive(true);
        if (Black.Equals(0) && White.Equals(0))
        {
            Log.text = "���º�!";
            return;
        }
        else
        {
            PutOn player = NetworkClient.localPlayer.GetComponent<PutOn>();
            if(player.playerType.Equals(PlayerType.Black))
            {
                if(Black.Equals(0))
                {
                    Log.text = "�й�!";
                }
                else
                {
                    Log.text = "�¸�";
                }
            }
            else
            {
                if (White.Equals(0))
                {
                    Log.text = "�й�!";
                }
                else
                {
                    Log.text = "�¸�";
                }
            }
        }
        
    }
    

    //[Command]
    //public void CmdSpawnCoin(Vector3 position)
    //{
    //    // �������� ������Ʈ ����
    //    GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

    //    // ��Ʈ��ũ�� ������Ʈ �߰�
    //    NetworkServer.Spawn(coin);

    //    // ������ ������Ʈ�� NetworkIdentity ��������
    //    NetworkIdentity identity = coin.GetComponent<NetworkIdentity>();

    //    // Ŭ���̾�Ʈ���� ���� �ο�
    //    if (identity != null)
    //    {
    //        RpcAssignClientAuthority(identity.netId);
    //    }
    //}

    //// Ŭ���̾�Ʈ���� ���� �ο� ��û
    //[ClientRpc]
    //private void RpcAssignClientAuthority(uint netId)
    //{
    //    NetworkIdentity identity = NetworkServer.spawned[netId];
    //    if (identity != null)
    //    {
    //        identity.AssignClientAuthority(connectionToClient);
    //    }
    //}

    [ClientRpc]
    public void CmdChangeTurn()
    {
        isTurn = !isTurn;
    }
    // Ŭ���̾�Ʈ ���� ���� �޼��� (Ȥ�� �ʿ��ϴٸ� ���)
    //public void ReleaseClientAuthority(NetworkIdentity identity)
    //{
    //    if (identity != null)
    //    {
    //        identity.RemoveClientAuthority();
    //    }
    //}
}
