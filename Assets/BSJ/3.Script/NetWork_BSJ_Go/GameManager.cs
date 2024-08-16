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
        //서버에서 오브젝트 생성
        GameObject Coin_obj_BSJ = Instantiate(coin_Spawn, Vector3.zero, Quaternion.identity);

        //네트워크에 오브젝트 추가
        NetworkServer.Spawn(Coin_obj_BSJ);

        //생성된 오브젝트의 Identity 가져오기
        NetworkIdentity identity = Coin_obj_BSJ.GetComponent<NetworkIdentity>();

        //클라에게 권한 부여
        if (identity != null)
        {
            identity.AssignClientAuthority(connectionToClient);
        }
    }

    //클라 권한 해제 메서드 (혹시 모르니까 미리 만들어두기)
    public void RemoveClientAuthority(NetworkIdentity identity)
    {
        if (identity != null)
        {
            identity.RemoveClientAuthority();
        }
    }
}
