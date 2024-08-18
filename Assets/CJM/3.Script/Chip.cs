using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Chip : NetworkBehaviour
{
    //Chip 자체는 동작하지 않고 반투명한 돌만 나오게끔 할 예정 
    private bool isPut = false;
    public bool IsPut { get => isPut; set => isPut = value; }
    [SerializeField] private Gomoku_Logic logic;
    [SerializeField] private Player player;
    private GoGameManager gameManager;

    //0 흑 1 백 
    [SerializeField] private Material[] checkchip_material;
    [SerializeField] private MeshRenderer meshrender;

    public MeshRenderer MeshrenderGet { get => meshrender; set => meshrender = value; }

    public Material[] CheckChip_Mate { get => checkchip_material; }

    private int row;
    private int col;
    public int Row { get => row; set => row = value; }
    public int Col { get => col; set => col = value; }

    private void Awake()
    {
        meshrender = GetComponent<MeshRenderer>();
        meshrender.enabled = false;
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
        gameManager = FindObjectOfType<GoGameManager>();
    }


    private void OnMouseOver()
    {
        if (!isPut && logic.result_Panel.activeSelf.Equals(false))
        {
            MeshRenderer mate = transform.GetComponent<MeshRenderer>();
            mate.enabled = true;
            mate.material = ((gameManager.myTurn % 2).Equals(1)) ? CheckChip_Mate[0] : CheckChip_Mate[1];
        }
    }

    private void OnMouseExit()
    {
        if (!isPut && logic.result_Panel.activeSelf.Equals(false))
        {
            meshrender.enabled = false;
        }
    }

}
