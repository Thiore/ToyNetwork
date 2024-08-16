using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager_BSJ : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        // Ŭ���̾�Ʈ���� ������ ���� �ο� ��û
        if (isLocalPlayer)
        {
            CmdRequestClientAuthority();
        }
    }

    // ������ Ŭ���̾�Ʈ ���� �ο� ��û
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
        // ������ Ŭ���̾�Ʈ ���� ���� ��û
        if (isLocalPlayer)
        {
            CmdReleaseClientAuthority();
        }
    }

    // ������ Ŭ���̾�Ʈ ���� ���� ��û
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
