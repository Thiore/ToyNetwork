using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RPC_GO_BSJ : NetworkBehaviour
{
    [SerializeField] private GameObject goPrefab; // 코인 프리팹 변수명 수정

    // Client가 Server에 Connect 되었을 때 콜백 함수
    public override void OnStartAuthority()
    {
        Debug.Log("OnStartClient 메서드 호출됨");
        // 권한이 있는 로컬 플레이어인 경우
        if (isLocalPlayer)
        {
            Debug.Log("RPC_GO_BSJ 들어옴");

            // 현재 위치와 회전을 서버에 전송
            PositionAndRotation(transform.position, transform.rotation);
        }
        else
        {
            Debug.Log("isLocalPlayer가 false임");
        }
    }
    private void Update()
    {
        
    }
    //[Command]
    //public void CmdAssignAuthority(NetworkIdentity obj, NetworkConnectionToClient conn)
    //{
    //    obj.AssignClientAuthority(conn);
    //    Debug.Log("클라 권한 들어왔음");
    //}

    [Command]
    private void PositionAndRotation(Vector3 position, Quaternion rotation)
    {
        UpdatePositionAndRotation(position, rotation);
    }

    [ClientRpc]
    private void UpdatePositionAndRotation(Vector3 position, Quaternion rotation)
    {
        if (goPrefab != null)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
        else
        {
            Debug.LogError("goPrefab이 할당되지 않았습니다.");
        }
    }
    public void SyncPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        if (!isLocalPlayer)
        {
            PositionAndRotation(position, rotation);
        }
    }
}
