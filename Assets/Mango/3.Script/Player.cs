using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    private int myColor = 0;
    public int MyColor { get { return myColor; } }
    //0�� 1��
    [SerializeField] private Material[] chip_material;

    //private LayerMask empty = LayerMask.NameToLayer("Empty");

    // false �� true �� 
    private bool myTurn = true;
    public bool Myturn { get => myTurn; }

    [SerializeField] private Gomoku_Logic logic;
    private GoGameManager myGameManager;

    private float currentTime = 0f;
    private float limitTime = 10f;



    private void Awake()
    {
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
        myGameManager = GameObject.FindObjectOfType<GoGameManager>();
    }



    private void Update()
    {
        if (!isLocalPlayer) return;

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
                        CmdPutChip(chip);
                        Debug.Log("������Ʈ");
                    }
                }
            }
        }
    }

    public int GetColor()
    {
        return myColor;
    }

    [ClientRpc]
    private void CmdPutChip(Chip chip)
    {

        Debug.Log("cmd command");
        RPCDrawBoard(chip);
    }

    [Command]
    private void RPCDrawBoard(Chip chip)
    {
        Debug.Log(":rpc");
        chip.MeshrenderGet.enabled = true;
        chip.MeshrenderGet.material = myTurn ? chip_material[0] : chip_material[1];
        logic.AddChip(chip, this);
        Debug.Log(NetworkServer.active);

        TurnChange();

    }


    public void TurnChange()
    {
        myTurn = !myTurn;
    }




    public void TranslucentChip()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent<Chip>(out Chip chip))
                {
                    if (!chip.IsPut && logic.result_Panel.activeSelf.Equals(false))
                    {
                        MeshRenderer mate = transform.GetComponent<MeshRenderer>();
                        mate.enabled = true;
                        mate.material = ((myGameManager.myTurn % 2).Equals(1)) ? chip.CheckChip_Mate[0] : chip.CheckChip_Mate[1];
                    }
                }
            }
        }

    }




}
