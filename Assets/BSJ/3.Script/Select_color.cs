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
    private Renderer chipRenderer;
    public override void OnStartAuthority()
    {
        if (hasAuthority)
        {
            Debug.Log("Select_color OnStartAuthority 들어옴");
            //CmdSpawnCoin();
        }
    }
    private void Start()
    {
        Debug.Log($"Start - PlayerType: {playerType}, isLocalPlayer: {hasAuthority}");
        // 오브젝트가 권한을 가지고 있는지 확인
        if (hasAuthority)
        {
            // 로컬 플레이어의 정보나 PlayerType을 설정
            CmdSetPlayerType(PlayerType.White); // 예시로 White로 설정
        }
        

        chipRenderer = chipPrefab.GetComponent<Renderer>();
        SetPlayerLayer();
        SetChipMaterial();
    }

    [Command]
    private void CmdSetPlayerType(PlayerType type)
    {
        Debug.Log("CmdSetPlayerType 호출됨. 타입: " + type);
        playerType = type;
        RpcUpdatePlayerType(type);
    }

    [ClientRpc]
    void RpcUpdatePlayerType(PlayerType newType)
    {
        playerType = newType;
        Debug.Log($"RpcUpdatePlayerType 호출됨. 타입: {playerType}");

        // PlayerType이 업데이트된 후 즉시 material을 업데이트
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

    private void SetChipMaterial()
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