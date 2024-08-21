using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ing static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player : MonoBehaviour
{
    public Color color;
    public enum Color
    {
        White = 0,
        Black
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

    private void Awake()
    {
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
    }

    private void Update()
    {
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
                DrawBoard(hit.collider.gameObject);
            }
        }
    }


    private void DrawBoard(GameObject obj)
    {
        if (obj.TryGetComponent(out Chip chip) && !chip.IsPut)
        {
            obj.GetComponent<MeshFilter>().mesh = chip.ChipMesh;
            MeshRenderer mate = chip.GetComponent<MeshRenderer>();
            mate.material = myTurn ? chip_material[0] : chip_material[1];
            logic.AddChip(chip, this);
            chip.IsPut = true;
            TurnChange();
        }
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
                if(hit.collider.gameObject.TryGetComponent<Chip>(out Chip chip))
                {
                    if (!chip.IsPut && logic.result_Panel.activeSelf.Equals(false))
                    {
                        transform.GetComponent<MeshFilter>().mesh = chip.ChipMesh;
                        MeshRenderer mate = transform.GetComponent<MeshRenderer>();

                        mate.material = myTurn ? chip.CheckChip_Mate[0] : chip.CheckChip_Mate[1];
                    }
                }
            }
        }

    }




}
