using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    //Chip 자체는 동작하지 않고 반투명한 돌만 나오게끔 할 예정 

    private bool isPut = false;
    public bool IsPut { get => isPut; set => isPut = value; }
    [SerializeField] private Player player;

    //0 흑 1 백 
    [SerializeField] private Material[] checkchip_material;
    [SerializeField] private Mesh mesh;
    public Mesh ChipMesh { get => mesh; }

    public int row { get; private set; }
    public int col { get; private set; }

    public Chip(int r, int c)
    {
        row = r;
        col = c;
    }

    private void Awake()
    {
        mesh = Resources.Load("Gogame_chip") as Mesh;
        player = GameObject.FindObjectOfType<Player>();
    }

    private void OnMouseOver()
    {
        if (!isPut)
        {
            transform.GetComponent<MeshFilter>().mesh = mesh;
            MeshRenderer mate = transform.GetComponent<MeshRenderer>();
            mate.material = player.MyColor.Equals(0) ? checkchip_material[0] : checkchip_material[1];
        }
    }

    private void OnMouseExit()
    {
        if (!isPut)
        {
            gameObject.GetComponent<MeshFilter>().mesh = null;
        }
    }

}
