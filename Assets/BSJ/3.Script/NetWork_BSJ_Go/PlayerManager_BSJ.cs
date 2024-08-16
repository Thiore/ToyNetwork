using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
// 서버에서 클라에게 권한을 주기위한

public class PlayerManager_BSJ : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //클라에게 권한 부여
        if (isLocalPlayer)
        {
            //NetworkIdentity identity = GetComponent<NetworkIdentity>();
            //if (identity != null)
            //{
            //    identity.AssignClientAuthority(connectionToClient);
            //}
            CmdAssignClientAuthority();
        }
    }
    [Command]
    private void CmdAssignClientAuthority()
    {
        if (connectionToClient != null)
        {
            GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        }
    }
    public override void OnStopLocalPlayer()
    {
        base.OnStopLocalPlayer();

        //권한 해제
        if (isLocalPlayer)
        {
            NetworkIdentity identity = GetComponent<NetworkIdentity>();
            if (identity != null)
            {
                identity.RemoveClientAuthority();
            }
        }
    }
}
