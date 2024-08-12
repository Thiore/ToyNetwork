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

    // 0�� ������ / 1�� ��� 
    [SerializeField] private Material[] chip_material;
    [SerializeField] private Material[] checkchip_material;

    [SerializeField] Mesh mesh;

    private void Awake()
    {
        mesh = Resources.Load("Gogame_chip") as Mesh;
        chip_pivotParent = GameObject.Find("Chip_Pivot");
        chipPivots = new Transform[chip_pivotParent.transform.childCount];
        Debug.Log(chip_pivotParent.transform.childCount);

    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        CheckPutChip(ray);
        PutChip(ray);
    }

    private void PutChip(Ray ray)
    {
        //���� 0�̰ų� 0�� �ƴϰų��� �θ� �ɵ� 

        if (Input.GetMouseButtonUp(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {

                    Debug.Log(hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<MeshFilter>().mesh = mesh;
                    MeshRenderer mate = hit.collider.gameObject.GetComponent<MeshRenderer>();
                    mate.material = myColor.Equals(0) ? chip_material[0] : chip_material[1];
                }
            }
        }
    }

    private void CheckPutChip(Ray ray)
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    hit.collider.gameObject.GetComponent<MeshFilter>().mesh = mesh;
                    MeshRenderer mate = hit.collider.gameObject.GetComponent<MeshRenderer>();
                    mate.material = myColor.Equals(0) ? checkchip_material[0] : checkchip_material[1];

                }
            }
        }
    }


}
