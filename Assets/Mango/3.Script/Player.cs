using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Player : MonoBehaviour
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
    private bool myTurn = false;

    [SerializeField] Gomoku_Logic logic;

    private Action action;

    private float currentTime = 0f;
    private float limitTime = 10f;

    private void Awake()
    {
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
    }

    private void Update()
    {
        //GoGame();
        if (Input.GetMouseButtonUp(0) && logic.result_Panel.activeSelf.Equals(false))
        {
            PutChip();
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
                        if(myColor.Equals(0))
                        {
                            if (!logic.Check_SamSam(chip))
                            {
                                return;
                            }
                        }
                        chip.IsPut = true;
                        hit.collider.gameObject.GetComponent<MeshFilter>().mesh = chip.ChipMesh;
                        MeshRenderer mate = chip.GetComponent<MeshRenderer>();
                        mate.material = myColor.Equals(0) ? chip_material[0] : chip_material[1];
                        logic.AddChip(chip, this);
                        TurnChange();
                    }
                }
            }
        }
    }


    public void TurnChange()
    {
        if (myColor.Equals(0))
        {
            myColor = 1;
        }
        else if (myColor.Equals(1))
        {
            myColor = 0;
        }
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


}
