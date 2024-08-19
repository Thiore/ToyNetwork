using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public class RPC_Coin_BSJ : NetworkBehaviour
{
    [SerializeField] private GameObject coinPrefab; // 코인 프리팹 변수명 수정
    
    // Client가 Server에 Connect 되었을 때 콜백 함수
    public override void OnStartAuthority()
    {
        if (isLocalPlayer)
        {
            Debug.Log("OnStartAuthority 들어옴");
            //CmdSpawnCoin();
        }
    }

    private void Update()
    {
        // 스페이스바를 눌렀을 때 코인 생성
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            CmdSpawnCoin();
        }
    }

    [Command]
    private void CmdSpawnCoin()
    {
        GameObject coin = Instantiate(coinPrefab);
        NetworkServer.Spawn(coin, connectionToClient);  // 생성한 클라이언트에게 권한 부여
    }

    


}
