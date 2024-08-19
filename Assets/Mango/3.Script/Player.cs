using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    public Color color;
    public enum Color
    {
        White = 0,
        Black
    }

    [SyncVar]
    public int MyNumber = 0;
    public void MyNumberGet(int num)
    {
        MyNumber = num;
    }

    private int myColor = 0;
    public int MyColor { get { return myColor; } }
    //0Èæ 1¹é
    [SerializeField] private Material[] chip_material;
    // false Èæ true ¹é 
    private bool myTurn = false;
    public bool Myturn { get => myTurn; }

    [SerializeField] private Gomoku_Logic logic;
    [SerializeField] private Board board;
    private GoGameManager myGameManager;


    private void Awake()
    {
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
        myGameManager = GameObject.FindObjectOfType<GoGameManager>();
        board = GameObject.FindObjectOfType<Board>();
    }


    private void Update()
    {
        if (!isLocalPlayer) return;

        Debug.Log(MyNumber);

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

    public int GetColor()
    {
        return myColor;
    }

    [Command]
    private void CmdPutChip(Chip chip)
    {
        RPCDrawBoard(chip);
    }

    [ClientRpc]
    private void RPCDrawBoard(Chip chip)
    {
        if(chip.IsPut == false)
        {
            chip.IsPut = true;
            chip.MeshrenderGet.enabled = true;
            chip.MeshrenderGet.material = myTurn ? chip_material[0] : chip_material[1];
            logic.AddChip(chip, this);
            TurnChange();
        }
    }


    public void TurnChange()
    {
        myTurn = !myTurn;
    }





}
