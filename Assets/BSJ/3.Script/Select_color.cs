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
            Debug.Log("Select_color OnStartAuthority ����");
            //CmdSpawnCoin();
        }
    }
    private void Start()
    {
        Debug.Log($"Start - PlayerType: {playerType}, isLocalPlayer: {hasAuthority}");
        // ������Ʈ�� ������ ������ �ִ��� Ȯ��
        if (hasAuthority)
        {
            // ���� �÷��̾��� ������ PlayerType�� ����
            CmdSetPlayerType(PlayerType.White); // ���÷� White�� ����
        }
        

        chipRenderer = chipPrefab.GetComponent<Renderer>();
        SetPlayerLayer();
        SetChipMaterial();
    }

    [Command]
    private void CmdSetPlayerType(PlayerType type)
    {
        Debug.Log("CmdSetPlayerType ȣ���. Ÿ��: " + type);
        playerType = type;
        RpcUpdatePlayerType(type);
    }

    [ClientRpc]
    void RpcUpdatePlayerType(PlayerType newType)
    {
        playerType = newType;
        Debug.Log($"RpcUpdatePlayerType ȣ���. Ÿ��: {playerType}");

        // PlayerType�� ������Ʈ�� �� ��� material�� ������Ʈ
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
        Debug.Log($"SetChipMaterial ȣ��� - PlayerType: {playerType}");
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