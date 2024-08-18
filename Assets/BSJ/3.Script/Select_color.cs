using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
    
public enum PlayerType { Black, White}

public class Select_color : NetworkBehaviour
{
    public PlayerType playerType;

    public Material chipBlack;
    public Material chipWhite;
    public GameObject chipPrefab;
    public Renderer chipRenderer;

    private void Awake()
    {
        chipRenderer = chipPrefab.GetComponent<Renderer>();
        if (chipRenderer == null)
        {
            Debug.LogError("Renderer component not found on chipPrefab");
        }
    }


    //private void Start()
    //{
    ////    if (isLocalPlayer) // 로컬 플레이어에서만 색상 설정
    ////    {
    ////        SetChipMaterial();
    ////        SetPlayerLayer();
    ////    }
    ////}

    public void SetPlayerLayer()
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

    public void SetChipMaterial()
    {
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
