using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public class RPC_Coin_BSJ : NetworkBehaviour
{
    [SerializeField] private GameObject Coin_BSJ;
    //Client�� Server�� Connect �Ǿ��� �� �ݹ��Լ�
    public override void OnStartAuthority()
    {
        if (isLocalPlayer)
        {
            Coin_BSJ.SetActive(true);
        }
    }
}
