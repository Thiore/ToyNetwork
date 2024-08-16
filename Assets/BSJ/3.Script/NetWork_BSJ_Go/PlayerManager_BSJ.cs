using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
// �������� Ŭ�󿡰� ������ �ֱ�����

public class PlayerManager_BSJ : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //Ŭ�󿡰� ���� �ο�
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

        //���� ����
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
