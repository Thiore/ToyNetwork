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
        // 서버에서 오브젝트 생성
        GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);

        // 네트워크에 오브젝트 추가
        NetworkServer.Spawn(coin);

        // 생성된 오브젝트의 NetworkIdentity 가져오기
        NetworkIdentity identity = coin.GetComponent<NetworkIdentity>();

        // 클라이언트에게 권한 부여
        if (identity != null)
        {
            RpcAssignClientAuthority(identity.netId);
        }
    }

    // 클라이언트에게 권한 부여 요청
    [ClientRpc]
    private void RpcAssignClientAuthority(uint netId)
    {
        NetworkIdentity identity = NetworkServer.spawned[netId];
        if (identity != null)
        {
            identity.AssignClientAuthority(connectionToClient);
        }
    }

    // 클라이언트 권한 해제 메서드 (혹시 필요하다면 사용)
    public void ReleaseClientAuthority(NetworkIdentity identity)
    {
        if (identity != null)
        {
            identity.RemoveClientAuthority();
        }
    }
}
