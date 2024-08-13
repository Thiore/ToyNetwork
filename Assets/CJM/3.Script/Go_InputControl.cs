using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum PlayerColor
{
    Black = 0,
    White
}

public class Go_InputControl : MonoBehaviour
{
    int myColor = 0;
    [SerializeField] private Transform[] chipPivots;
    [SerializeField] private GameObject chip_pivotParent;

    // 0번 검은색 / 1번 흰색 
    [SerializeField] private Material[] chip_material;
    [SerializeField] private Material[] checkchip_material;

    [SerializeField] private Mesh mesh;

    private GameObject currentPosChip;
    private Queue<GameObject> currentPosChips;



    private void Awake()
    {
        mesh = Resources.Load("Gogame_chip") as Mesh;
        chip_pivotParent = GameObject.Find("Chip_Pivot");
        chipPivots = new Transform[chip_pivotParent.transform.childCount];
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
        //내가 0이거나 0이 아니거나로 두면 될듯 

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                hit.collider.gameObject.GetComponent<MeshFilter>().mesh = mesh;
                MeshRenderer mate = hit.collider.gameObject.GetComponent<MeshRenderer>();
                mate.material = myColor.Equals(0) ? chip_material[0] : chip_material[1];
            }
        }

    }

    private void CheckPutChip()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                currentPosChip = hit.collider.gameObject;
                currentPosChip.GetComponent<MeshFilter>().mesh = mesh;
                MeshRenderer mate = currentPosChip.GetComponent<MeshRenderer>();
                mate.material = myColor.Equals(0) ? checkchip_material[0] : checkchip_material[1];
            }
        }


    }


}
