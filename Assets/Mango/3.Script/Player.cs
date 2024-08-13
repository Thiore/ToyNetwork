using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color color;
    public enum Color
    {
        Black = 0,
        White
    }

    private int myColor = 1;
    public int MyColor { get { return myColor; } }
    //0Èæ 1¹é
    [SerializeField] private Material[] chip_material;
    private bool myTurn = false;

    [SerializeField] Gomoku_Logic logic;

    private void Awake()
    {
        logic = GetComponent<Gomoku_Logic>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
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
                if (TryGetComponent(out Chip chip))
                {
                    if (!chip.IsPut)
                    {
                        chip.IsPut = true;
                        MeshRenderer mate = hit.collider.gameObject.GetComponent<MeshRenderer>();
                        mate.material = myColor.Equals(0) ? chip_material[0] : chip_material[1];
                    }
                }
            }
        }
    }

}
