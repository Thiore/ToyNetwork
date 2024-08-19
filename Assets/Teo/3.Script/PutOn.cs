using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PutOn : NetworkBehaviour
{
    [SerializeField] private Transform LB;
    [SerializeField] private Transform RT;
    private float setTime = 10f;
    private float startTime;
    [SerializeField] private GameObject Chip_Prefabs;
    private int SetCount = 5;
    private Queue<GameObject> Chip_Queue = new Queue<GameObject>();
    public bool isGameStart { get; set; }

    private Select_color select_Color;
    private Camera cam;

    private void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        select_Color = GetComponent<Select_color>();

        for (int i = 0; i < SetCount; i++)
        {
            GameObject obj = Instantiate(select_Color.chipPrefab);
            obj.SetActive(false);
            obj.GetComponent<Kick_Chip>().SetPutOn(this);
            Chip_Queue.Enqueue(obj);
        }

        startTime = 0f;
    }

    private void Update()
    {
        if (isGameStart || !isLocalPlayer) // 권한이 있는 로컬 플레이어만 수행
        {
            //Debug.Log("마우스 권한 들어옴");
            return;
        }
        //Debug.Log("Put on Update 들어옴");
        if (startTime < setTime)
        {
            startTime += Time.deltaTime;
            if (Chip_Queue.Count > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("마우스 버튼 눌림");
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.point.x > LB.position.x && hit.point.x < RT.position.x && hit.point.z > LB.position.z && hit.point.z < RT.position.z && !hit.collider.CompareTag("Chip"))
                        {
                            // 서버에서 바둑알 생성 요청
                            CmdPlaceChip(hit.point);
                        }
                    }
                }
            }
        }
    }

    // 클라이언트에서 서버로 바둑알 위치 전송
    [Command]
    private void CmdPlaceChip(Vector3 position)
    {
        // 모든 클라이언트에서 바둑알 배치 동기화
        RpcPlaceChip(position);
    }

    // 모든 클라이언트에서 바둑알 배치
    [ClientRpc]
    private void RpcPlaceChip(Vector3 position)
    {
        if (Chip_Queue.Count > 0)
        {
            GameObject obj = Chip_Queue.Dequeue();
            obj.transform.position = position;
            obj.SetActive(true);
        }
    }

    public void Die(GameObject obj)
    {
        obj.SetActive(false);
        Chip_Queue.Enqueue(obj);
        Debug.Log(Chip_Queue.Count);
        if (Chip_Queue.Count.Equals(SetCount))
        {
            Debug.Log("Lose");
        }
    }
}
