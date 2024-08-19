using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum PlayerType { Black, White }

public class Select_color : NetworkBehaviour
{
    
    public PlayerType playerType;

    public Material chipBlack;
    public Material chipWhite;
    public GameObject chipPrefab;
    public Renderer chipRenderer;
    
    private static int playerCount = 0;
    

    private void Start()
    {
        
        //if (chipPrefab == null)
        //{
        //    Debug.LogError("chipPrefab이 할당되지 않았습니다!");
        //    return;
        //}

        //if (chipRenderer == null)
        //{
        //    Debug.LogError("chipPrefab에서 Renderer 컴포넌트를 찾을 수 없습니다!");
        //    return;
        //}
        chipRenderer = chipPrefab.GetComponent<Renderer>();
        if (isServer)
        {
            
            AssignPlayerType();

        }
        else
        {
            OnPlayerTypeChanged(playerType);
        }

        //Debug.Log($"Start - PlayerType: {playerType}, isLocalPlayer: {hasAuthority}");
        // 오브젝트가 권한을 가지고 있는지 확인
        //if (hasAuthority)
        //{
        //    // 로컬 플레이어의 정보나 PlayerType을 설정
        //    Debug.Log("CmdSetPlayerType 호출 전"); //호출 전 상태 확인
        //    CmdSetPlayerType(PlayerType.White); // 예시로 White로 설정
        //    Debug.Log("CmdSetPlayerType 호출 후");
        //    Debug.Log($"Start - PlayerType: {playerType}, isLocalPlayer: {hasAuthority}");
        //}


        //SetPlayerLayer();
        //SetChipMaterial();


    }

    [Server]
    private void AssignPlayerType()
    {
        
        //첫 번째 플레이어는 흰색, 두 번째 플레이어는 검은색
        if (playerCount % 2 == 0)
        {
            playerType = PlayerType.White;
        }
        else
        {
            playerType = PlayerType.Black;
        }
        playerCount++;
        //모든 클라에게 업데이트된 플레이어 타입 보내주기
        RpcUpdatePlayerType(playerType);
    }


    [Command]
    private void CmdSetPlayerType(PlayerType type)
    {
        Debug.Log($"권한 확인 - hasAuthority: {isLocalPlayer}");
        Debug.Log("CmdSetPlayerType 호출됨. 타입: " + type);
        playerType = type;
        RpcUpdatePlayerType(type);
    }
    

    [ClientRpc]
   public void RpcUpdatePlayerType(PlayerType newType)
    {
        //// 서버에서 결정된 플레이어 타입을 클라이언트에 반영
        //playerType = newType;
        //Debug.Log($"RpcUpdatePlayerType 호출됨. 타입: {playerType}");

        //// PlayerType이 업데이트된 후 material을 업데이트
        //SetPlayerLayer();
        //SetChipMaterial();

        // 서버에서 결정된 플레이어 타입을 클라이언트에 반영합니다.
        OnPlayerTypeChanged(newType);

    }

    public void OnPlayerTypeChanged(PlayerType newType)
    {
        playerType = newType;
        Debug.Log($"OnPlayerTypeChanged 호출됨. 타입: {playerType}");

        // PlayerType이 업데이트된 후 material을 업데이트
        SetPlayerLayer();
        SetChipMaterial();
    }
    private void SetPlayerLayer()
    {
        if (playerType == PlayerType.Black)
        {
            gameObject.layer = LayerMask.NameToLayer("Black");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("White");
        }
    }

    private  void SetChipMaterial()
    {
        Debug.Log($"SetChipMaterial 호출됨 - PlayerType: {playerType}");
        if (playerType == PlayerType.Black)
        {
            chipRenderer.material = chipBlack;
        }
        else
        {
            chipRenderer.material = chipWhite;
        }
    }
}