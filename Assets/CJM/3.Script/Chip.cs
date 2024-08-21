using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : NetworkBehaviour
{
    //Chip 자체는 동작하지 않고 반투명한 돌만 나오게끔 할 예정 
    [SyncVar]
    private bool isPut = false;
    public bool IsPut { get => isPut; set => isPut = value; }
    [SerializeField] private Gomoku_Logic logic;
    [SerializeField] public Player player = null;

    //0 흑 1 백 
    [SerializeField] private Material[] checkchip_material;
    [SerializeField] private MeshRenderer meshrender;

    public MeshRenderer MeshrenderGet { get => meshrender; set => meshrender = value; }

    public Material[] CheckChip_Mate { get => checkchip_material; }

    [SyncVar]
    [SerializeField] private  int row;
    [SyncVar]
    [SerializeField] private int col;
    public int Row { get => row; set => row = value; }
    public int Col { get => col; set => col = value; }

    private void Awake()
    {
        meshrender = GetComponent<MeshRenderer>();
        meshrender.enabled = false;
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
    }


    private void OnMouseOver()
    {
        if(player == null)
        {
            Player[] obj = FindObjectsOfType<Player>();
            
            foreach(Player p in obj)
            {
                if(p.isLocalPlayer)
                    player = p;
            }
        }
            

        if (!isPut && logic.result_Panel.activeSelf.Equals(false))
        {
            MeshRenderer mate = transform.GetComponent<MeshRenderer>();
            mate.enabled = true;
            mate.material = ((player.connectcount % 2).Equals(1)) ? CheckChip_Mate[0] : CheckChip_Mate[1];
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
