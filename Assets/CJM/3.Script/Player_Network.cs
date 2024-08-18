using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Player_Network : NetworkBehaviour
{
    public Color color;
    public enum Color
    {
        Black = 0,
        White
    }

    private int myColor = 0;
    public int MyColor { get { return myColor; } }
    //0Èæ 1¹é
    [SerializeField] private Material[] chip_material;

    // false Èæ true ¹é 
    private bool myTurn = true;
    public bool Myturn { get => myTurn; }

    [SerializeField] Gomoku_Logic logic;

    private float currentTime = 0f;
    private float limitTime = 10f;

    private static event Action<Player_Network> onChip;

    public override void OnStartAuthority()
    {
        if(hasAuthority)
        {

        }

 
    }


    private void Awake()
    {
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
    }

    private void Update()
    {
        //GoGame();
        if (Input.GetMouseButtonUp(0) && logic.result_Panel.activeSelf.Equals(false))
        {
            Send();
        }
    }


    private void PutChip()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Chip chip))
                {
                    if (!chip.IsPut)
                    {
                        if (myColor.Equals(0))
                        {
                            if (!logic.Check_SamSam(chip))
                            {
                                Debug.Log("33");
                                return;
                            }
                        }
                        chip.IsPut = true;
                        MeshRenderer mate = chip.GetComponent<MeshRenderer>();
                        mate.material = myTurn ? chip_material[0] : chip_material[1];
                        //logic.AddChip(chip, this);
                        TurnChange();
                    }
                }
            }
        }
    }


    public void TurnChange()
    {
        myTurn = !myTurn;
    }


    public void GoGame()
    {
        while (currentTime >= 0f)
        {
            currentTime -= Time.deltaTime;
            Debug.Log(currentTime);
            if (currentTime <= 0f)
            {
                currentTime = limitTime;
                TurnChange();
            }
        }
        //StartCoroutine(GoGame_co());
    }

    private IEnumerator GoGame_co()
    {
        currentTime = limitTime;
        while (currentTime >= 0f)
        {
            currentTime -= Time.deltaTime;
            Debug.Log(currentTime);
            if (currentTime <= 0f)
            {
                currentTime = limitTime;
                TurnChange();
                yield return new WaitForSecondsRealtime(limitTime);
            }
        }
        Debug.Log("ÅÏÃ¾");
        GoGame();
    }




    [Client]
    private void Send()
    {
        CmdPut();
    }


    [Command]
    private void CmdPut()
    {
        PlayerPutonChip();
    }


    [ClientRpc]
    private void PlayerPutonChip()
    {
        PutChip();
    }

}
