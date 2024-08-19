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
    public GameObject[] Chip_Array;
    private int Chip_Num = 0;
    public bool isGameStart { get; set; }

    private Select_color select_Color;
    private Camera cam;
    public bool isthis;

    private void Start()
    {
        Chip_Array = new GameObject[SetCount];
        select_Color = GetComponent<Select_color>();

        for (int i = 0; i < SetCount; i++)
        {
            GameObject obj = Instantiate(select_Color.chipPrefab);
            obj.SetActive(false);
            obj.GetComponent<Kick_Chip>().SetPutOn(this);
            Chip_Array[i] = obj;
            //Chip_Queue.Enqueue(obj);
        }
        startTime = 0f;
        if (!isLocalPlayer) return;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        //if (isGameStart || isLocalPlayer) // 권한이 있는 로컬 플레이어만 수행
        //{
        //    //Debug.Log("마우스 권한 들어옴");
        //    return;
        //}
        if (!isLocalPlayer) return;
        isthis = isLocalPlayer;
        //Debug.Log("Put on Update 들어옴");
        if (startTime < setTime)
        {
            startTime += Time.deltaTime;
            //if (Chip_Queue.Count > 0)
            //{
            if(!Chip_Num.Equals(SetCount))
            { 
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("마우스 버튼 눌림");
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.point.x > LB.position.x && hit.point.x < RT.position.x && hit.point.z > LB.position.z && hit.point.z < RT.position.z && !hit.collider.CompareTag("Chip"))
                        {
                            Debug.Log("생성요청하기");
                            // 서버에서 바둑알 생성 요청
                            CmdPlaceChip(hit.point);
                            //hit.transform.GetComponent<Kick_Chip>().ismine = true;
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
        Debug.Log("여기까진 들어옴");
        //if (Chip_Queue.Count > 0)
        if (!Chip_Num.Equals(SetCount))
        {
            Debug.Log("들어왔니?");
            //GameObject obj = Chip_Queue.Dequeue();
            GameObject obj = Chip_Array[Chip_Num];
            Chip_Num++;
            obj.transform.position = position;
            obj.SetActive(true);
            if (isLocalPlayer)
            {
                
                obj.GetComponent<Kick_Chip>().ismine = true;
                
            }
            
        }
    }
    [Command]
    public void CmdKickEgg(int num, Vector3 force)
    {
        RpcKickEgg(num, force);


    }
    [ClientRpc]
    public void RpcKickEgg(int num, Vector3 force)
    {
        Chip_Array[num].GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        //// `Chip_Prefabs`의 위치와 회전을 동기화
        //Chip_Prefabs.transform.position = position;
        //Chip_Prefabs.transform.rotation = rotation;
        Debug.Log("RpcKickEgg() 호출됨");
        
    }

    //[ClientRpc]
    //private void RPCEgg()
    //{
    //    Debug.Log("RPCEgg() 호출됨");

    //}
    public void Die(GameObject obj)
    {
        obj.SetActive(false);
        Chip_Num--;
        //Chip_Queue.Enqueue(obj);
        //Debug.Log(Chip_Queue.Count);
        if (Chip_Num.Equals(0))
        {
            Debug.Log("Lose");
        }
    }
}
