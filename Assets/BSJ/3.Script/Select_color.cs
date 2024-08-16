using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum PlayerType { Black, White}

public class Select_color : MonoBehaviour
{
    public PlayerType playerType;

    public Material chipBlack;
    public Material chipWhite;
    public GameObject chipPrefab;
    private Renderer chipRenderer;
    

   
    private void Start()
    {
        chipRenderer = chipPrefab.GetComponent<Renderer>();
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

    private void SetChipMaterial()
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
