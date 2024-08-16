using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public class RPC_Coin_BSJ : NetworkBehaviour
{
    [SerializeField] private GameObject Coin_BSJ;
    //Client가 Server에 Connect 되었을 때 콜백함수
    public override void OnStartAuthority()
    {
        if (isLocalPlayer)
        {
            Coin_BSJ.SetActive(true);
        }
    }
}
