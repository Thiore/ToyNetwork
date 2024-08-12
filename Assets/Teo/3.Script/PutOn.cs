using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutOn : MonoBehaviour
{
    [SerializeField] private Transform LB;
    [SerializeField] private Transform RT;
    private float setTime = 15f;
    private float startTime;
    [SerializeField] private GameObject Chip_Prefabs;
    private int SetCount = 5;
    private Queue<GameObject> Chip_Queue = new Queue<GameObject>();

    private void Start()
    {
        startTime = 0f;
        for(int i = 0; i < SetCount;i++)
        {
            GameObject obj = Instantiate(Chip_Prefabs);
            obj.SetActive(false);
            Chip_Queue.Enqueue(obj);
        }
    }

    private void Update()
    {
        if(startTime<setTime)
        {
            startTime += Time.deltaTime;
            if(Chip_Queue.Count>0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.point.x > LB.position.x && hit.point.x < RT.position.x && hit.point.z > LB.position.z && hit.point.z < RT.position.z && !hit.collider.CompareTag("Chip"))
                        {
                            GameObject obj = Chip_Queue.Dequeue();
                            obj.transform.position = hit.point;
                            obj.SetActive(true);
                        }
                    }
                }
            }
            
        }
    }

}
