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
        //    Debug.LogError("chipPrefab�� �Ҵ���� �ʾҽ��ϴ�!");
        //    return;
        //}

        //if (chipRenderer == null)
        //{
        //    Debug.LogError("chipPrefab���� Renderer ������Ʈ�� ã�� �� �����ϴ�!");
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
        // ������Ʈ�� ������ ������ �ִ��� Ȯ��
        //if (hasAuthority)
        //{
        //    // ���� �÷��̾��� ������ PlayerType�� ����
        //    Debug.Log("CmdSetPlayerType ȣ�� ��"); //ȣ�� �� ���� Ȯ��
        //    CmdSetPlayerType(PlayerType.White); // ���÷� White�� ����
        //    Debug.Log("CmdSetPlayerType ȣ�� ��");
        //    Debug.Log($"Start - PlayerType: {playerType}, isLocalPlayer: {hasAuthority}");
        //}


        //SetPlayerLayer();
        //SetChipMaterial();


    }

    [Server]
    private void AssignPlayerType()
    {
        
        //ù ��° �÷��̾�� ���, �� ��° �÷��̾�� ������
        if (playerCount % 2 == 0)
        {
            playerType = PlayerType.White;
        }
        else
        {
            playerType = PlayerType.Black;
        }
        playerCount++;
        //��� Ŭ�󿡰� ������Ʈ�� �÷��̾� Ÿ�� �����ֱ�
        RpcUpdatePlayerType(playerType);
    }


    [Command]
    private void CmdSetPlayerType(PlayerType type)
    {
        Debug.Log($"���� Ȯ�� - hasAuthority: {isLocalPlayer}");
        Debug.Log("CmdSetPlayerType ȣ���. Ÿ��: " + type);
        playerType = type;
        RpcUpdatePlayerType(type);
    }
    

    [ClientRpc]
   public void RpcUpdatePlayerType(PlayerType newType)
    {
        //// �������� ������ �÷��̾� Ÿ���� Ŭ���̾�Ʈ�� �ݿ�
        //playerType = newType;
        //Debug.Log($"RpcUpdatePlayerType ȣ���. Ÿ��: {playerType}");

        //// PlayerType�� ������Ʈ�� �� material�� ������Ʈ
        //SetPlayerLayer();
        //SetChipMaterial();

        // �������� ������ �÷��̾� Ÿ���� Ŭ���̾�Ʈ�� �ݿ��մϴ�.
        OnPlayerTypeChanged(newType);

    }

    public void OnPlayerTypeChanged(PlayerType newType)
    {
        playerType = newType;
        Debug.Log($"OnPlayerTypeChanged ȣ���. Ÿ��: {playerType}");

        // PlayerType�� ������Ʈ�� �� material�� ������Ʈ
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