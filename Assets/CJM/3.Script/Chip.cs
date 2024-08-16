using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : NetworkBehaviour
{
    //Chip 자체는 동작하지 않고 반투명한 돌만 나오게끔 할 예정 

    private bool isPut = false;
    public bool IsPut { get => isPut; set => isPut = value; }
    [SerializeField] private Gomoku_Logic logic;
    [SerializeField] private Player player;

    //0 흑 1 백 
    [SerializeField] private Material[] checkchip_material;
    [SerializeField] private Mesh mesh;
    public Mesh ChipMesh { get => mesh; }

    public Material[] CheckChip_Mate { get => checkchip_material; }

    private int row;
    private int col;
    public int Row { get => row; set => row = value; }
    public int Col { get => col; set => col = value; }

    private void Awake()
    {
        mesh = Resources.Load("Gogame_chip") as Mesh;
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();

    }

    public override void OnStartClient()
    {
        player = FindObjectOfType<Player>();
        NetworkServer.Spawn(this.gameObject);
        this.gameObject.SetActive(true);
    }

    private void OnMouseOver()
    {

    }

    private void OnMouseExit()
    {
        if (!isPut && logic.result_Panel.activeSelf.Equals(false))
        {
            gameObject.GetComponent<MeshFilter>().mesh = null;
        }
    }

}
