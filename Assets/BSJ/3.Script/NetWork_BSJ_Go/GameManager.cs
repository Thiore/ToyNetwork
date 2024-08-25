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
            Log.text = "무승부!";
            return;
        }
        else
        {
            PutOn player = NetworkClient.localPlayer.GetComponent<PutOn>();
            if(player.playerType.Equals(PlayerType.Black))
            {
                if(Black.Equals(0))
                {
                    Log.text = "패배!";
                }
                else
                {
                    Log.text = "승리";
                }
            }
            else
            {
                if (White.Equals(0))
                {
                    Log.text = "패배!";
                }
                else
                {
                    Log.text = "승리";
                }
            }
        }
        
    }
    

    //[Command]
    //public void CmdSpawnCoin(Vector3 position)
    //{
    //    // 서버에서 오브젝트 생성
    //    GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

    //    // 네트워크에 오브젝트 추가
    //    NetworkServer.Spawn(coin);

    //    // 생성된 오브젝트의 NetworkIdentity 가져오기
    //    NetworkIdentity identity = coin.GetComponent<NetworkIdentity>();

    //    // 클라이언트에게 권한 부여
    //    if (identity != null)
    //    {
    //        RpcAssignClientAuthority(identity.netId);
    //    }
    //}

    //// 클라이언트에게 권한 부여 요청
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
    // 클라이언트 권한 해제 메서드 (혹시 필요하다면 사용)
    //public void ReleaseClientAuthority(NetworkIdentity identity)
    //{
    //    if (identity != null)
    //    {
    //        identity.RemoveClientAuthority();
    //    }
    //}
}
