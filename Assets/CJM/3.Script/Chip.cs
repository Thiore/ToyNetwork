using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    //Chip 자체는 동작하지 않고 반투명한 돌만 나오게끔 할 예정 

    private bool isPut = false;
    public bool IsPut { get => isPut; set => isPut = value; }
    [SerializeField] private Player player;
    [SerializeField] private Gomoku_Logic logic;

    //0 흑 1 백 
    [SerializeField] private Material[] checkchip_material;
    [SerializeField] private Mesh mesh;
    public Mesh ChipMesh { get => mesh; }

    private int row;
    private int col;
    public int Row { get => row; set => row = value; }
    public int Col { get => col; set => col = value; }

    private void Awake()
    {
        mesh = Resources.Load("Gogame_chip") as Mesh;
        player = GameObject.FindObjectOfType<Player>();
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
    }

    private void OnMouseOver()
    {
        if (!isPut && logic.result_Panel.activeSelf.Equals(false))
        {
            transform.GetComponent<MeshFilter>().mesh = mesh;
            MeshRenderer mate = transform.GetComponent<MeshRenderer>();
            mate.material = player.Myturn ? checkchip_material[0] : checkchip_material[1];
        }
    }

    private void OnMouseExit()
    {
        if (!isPut && logic.result_Panel.activeSelf.Equals(false))
        {
            gameObject.GetComponent<MeshFilter>().mesh = null;
        }
    }

}
