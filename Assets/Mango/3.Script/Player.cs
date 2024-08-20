using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    public GoColor color;
    public enum GoColor
    {
        White = 0,
        Black
    }

    private int myColor = 0;
    public int MyColor { get { return myColor; } }
    //0Èæ 1¹é
    [SerializeField] private Material[] chip_material;
    // false Èæ true ¹é 
    [SyncVar]
    private bool myTurn = false;
    public bool Myturn { get => myTurn; }

    private bool thefirst = false;

    [SerializeField] private Gomoku_Logic logic;
    [SerializeField] private Board board;
    private GoGameManager myGameManager;

    [SyncVar]
    public int connectcount;


    private void Awake()
    {
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
        myGameManager = GameObject.FindObjectOfType<GoGameManager>();
        board = GameObject.FindObjectOfType<Board>();
    }

    private void Start()
    {
        if (!isLocalPlayer) return;
        //GetCount();
        Debug.Log(connectcount);
        if (1 == connectcount % 2)
            thefirst = true;

        if (thefirst)
        {
            color = GoColor.Black;
            myTurn = true;
        }
        else
        {
            color = GoColor.White;
            myTurn = false;
        }
    }


    private void Update()
    {
        if (!isLocalPlayer || !myTurn) return;

        if (Input.GetMouseButtonUp(0) && logic.result_Panel.activeSelf.Equals(false))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out Chip chip))
                    {
                        ClientChip(chip);
                        CmdTurn();
                    }
                }
            }
        }

    }


    [Client]
    public void ClientChip(Chip chip)
    {
        CmdPutChip(chip);
    }

    [Command]
    private void CmdPutChip(Chip chip)
    {
        RPCDrawBoard(chip);
    }

    [ClientRpc]
    private void RPCDrawBoard(Chip chip)
    {

        if (chip.IsPut == false)
        {
            chip.IsPut = true;
            chip.MeshrenderGet.enabled = true;
            chip.MeshrenderGet.material = (1 == connectcount % 2) ? chip_material[0] : chip_material[1];
            logic.AddChip(chip, this);
        }
    }


    [Command]
    public void CmdTurn()
    {
        TurnChange();
    }

    [ClientRpc]
    public void TurnChange()
    {
        foreach(var player in FindObjectsOfType<Player>())
        {
            player.myTurn = !player.myTurn;
        }
    }


    public void SetConnectionOrder(int order)
    {
        connectcount = order;
    }


}
