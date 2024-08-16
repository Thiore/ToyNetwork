using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager_BSJ : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        // 클라이언트에서 서버로 권한 부여 요청
        if (isLocalPlayer)
        {
            CmdRequestClientAuthority();
        }
    }

    // 서버에 클라이언트 권한 부여 요청
    [Command]
    private void CmdRequestClientAuthority()
    {
        NetworkIdentity identity = GetComponent<NetworkIdentity>();
        if (identity != null)
        {
            identity.AssignClientAuthority(connectionToClient);
        }
    }

    public override void OnStopLocalPlayer()
    {
        base.OnStopLocalPlayer();
        // 서버에 클라이언트 권한 해제 요청
        if (isLocalPlayer)
        {
            CmdReleaseClientAuthority();
        }
    }

    // 서버에 클라이언트 권한 해제 요청
    [Command]
    private void CmdReleaseClientAuthority()
    {
        NetworkIdentity identity = GetComponent<NetworkIdentity>();
        if (identity != null)
        {
            identity.RemoveClientAuthority();
        }
    }
}
