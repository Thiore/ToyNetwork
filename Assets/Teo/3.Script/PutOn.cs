using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum PlayerType { Black, White }

public class PutOn : NetworkBehaviour
{
    [SerializeField] private Transform LB;
    [SerializeField] private Transform RT;
    
    private float setTime = 6f;
    private float startTime;
    [SerializeField] private GameObject Chip_Prefabs;
   
    private int SetCount = 5;
    [SyncVar]
    private bool isMyTurn;
    [SyncVar]
    public PlayerType playerType;
    private Queue<GameObject> Chip_Queue = new Queue<GameObject>();
   
    //public GameObject[] Chip_Array;
    private int Chip_Num = 0;
    public bool isGameStart { get; set; }

    //private Select_color select_Color;
    private Camera cam;
   // public bool isthis;

    //private void Awake()
    //{
    //    Chip_Array = new GameObject[SetCount];
    //}

    private void Start()
    {
        
        //select_Color = GetComponent<Select_color>();

        //for (int i = 0; i < SetCount; i++)
        //{
        //    GameObject obj = Instantiate(select_Color.chipPrefab);
        //    obj.SetActive(false);
        //    obj.GetComponent<Kick_Chip>().SetPutOn(this);
        //    Chip_Array[i] = obj;
        //    //Chip_Queue.Enqueue(obj);
        //}
        startTime = 0f;
        if (!isLocalPlayer) return;

        cam = GetComponent<Camera>();


        for (int i = 0; i < SetCount; i++)
        {
            CmdChipSet();
        }


    }

    private void Update()
    {
        //if (isGameStart || isLocalPlayer) // 권한이 있는 로컬 플레이어만 수행
        //{
        //    //Debug.Log("마우스 권한 들어옴");
        //    return;
        //}
        if (!isLocalPlayer||isServer) return;
        //isthis = isLocalPlayer;
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
                            Debug.Log("생성요청하기");
                            CmdChipPos(hit.point);
                            transform.GetChild(Chip_Num).position = hit.point;
                            transform.GetChild(Chip_Num).gameObject.SetActive(true);
                            Chip_Num++;
                            //            obj.SetActive(true);
                            // 서버에서 바둑알 생성 요청
                            //CmdPlaceChip(hit.point);
                            //hit.transform.GetComponent<Kick_Chip>().ismine = true;
                        }
                    }
                }
            }
        }
    }
    [Command]
    public void CmdChipSet()
    {
        
            GameObject _Chip = Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
            NetworkServer.Spawn(_Chip, gameObject);
            _Chip.TryGetComponent(out Kick_Chip chip);
            chip.SetPutOn(this);
            Chip_Queue.Enqueue(_Chip);
            _Chip.SetActive(false);
        
    }
    [ClientRpc]
    
    
    [Command]
    public void CmdChipPos(Vector3 pos)
    {
        if(Chip_Queue.Count>0)
        {
            GameObject obj = Chip_Queue.Dequeue();
            obj.transform.position = pos;
            obj.SetActive(true);
        }
       
    }

    public void ChangeType(PlayerType type)
    {
        playerType = type;
    }

    #region 주석시작
    // 클라이언트에서 서버로 바둑알 위치 전송
    //[Command]
    //private void CmdPlaceChip(Vector3 position)
    //{
    //    RpcPlaceChip(position);
    //    //if (netId.Equals(localID))
    //    //{
            
    //        if (!Chip_Num.Equals(SetCount))
    //        {
    //            Debug.Log("들어왔니?");
    //            //GameObject obj = Chip_Queue.Dequeue();
    //            GameObject obj = Chip_Array[Chip_Num];
    //            Chip_Num++;
    //            obj.transform.position = position;
    //            obj.SetActive(true);
    //            if (isLocalPlayer)
    //            {

    //                obj.GetComponent<Kick_Chip>().ismine = true;

    //            }

    //        }
    //    //}
    //    // 모든 클라이언트에서 바둑알 배치 동기화
        
    //}

    //// 모든 클라이언트에서 바둑알 배치
    //[ClientRpc]
    //private void RpcPlaceChip(Vector3 position)
    //{
    //    Debug.Log("여기까진 들어옴");
    //    //if (Chip_Queue.Count > 0)
    //        if (!Chip_Num.Equals(SetCount))
    //        {
    //            Debug.Log("들어왔니?");
    //            //GameObject obj = Chip_Queue.Dequeue();
    //            GameObject obj = Chip_Array[Chip_Num];
    //            Chip_Num++;
    //            obj.transform.position = position;
    //            obj.SetActive(true);
    //            if (isLocalPlayer)
    //            {

    //                obj.GetComponent<Kick_Chip>().ismine = true;

    //            }

    //        }
    //}
    ////[Server]
    ////public void Send_RpcKickEgg(int num, Vector3 force, Vector3 position, Quaternion rotation)
    ////{
    ////    CmdKickEgg(num, force, position, rotation);
    ////}
    //[Command]
    //public void CmdKickEgg(int num, Vector3 force/*, Vector3 position, Quaternion rotation*/)
    //{
    //    RpcKickEgg(num, force);
    //    Chip_Array[num].GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

    //}
    //[ClientRpc]
    //public void RpcKickEgg(int num, Vector3 force/*, Vector3 position, Quaternion rotation*/)
    //{
    //    Chip_Array[num].GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    //    //// `Chip_Prefabs`의 위치와 회전을 동기화
    //    //Chip_Prefabs.transform.position = position;
    //    //Chip_Prefabs.transform.rotation = rotation;
    //    // 클라이언트에 동기화된 위치와 회전 적용
    //     //Chip_Array[num].transform.position = position;
    //    //Chip_Array[num].transform.rotation = rotation;
    //    Debug.Log("RpcKickEgg() 호출됨");
        
    //}

    public void KickForce(Vector3 force,GameObject chip)
    {
        chip.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
    #endregion
    //[ClientRpc]
    //private void RPCEgg()
    //{
    //    Debug.Log("RPCEgg() 호출됨");

    //}
    public void Die(GameObject obj)
    {
        obj.SetActive(false);
        Chip_Queue.Enqueue(obj);
        //Debug.Log(Chip_Queue.Count);
        if (Chip_Queue.Count.Equals(0))
        {
            Debug.Log("Lose");
        }
    }
}
