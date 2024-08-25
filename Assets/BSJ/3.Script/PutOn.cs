using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public enum PlayerType { Black, White }

public class PutOn : NetworkBehaviour
{
    [SerializeField] private Transform LB;
    [SerializeField] private Transform RT;
    
    public float setTime = 6f;
    public float startTime;
    [SerializeField] private GameObject Chip_Prefabs;
   
    [SyncVar]
    public bool isMyTurn;
    [SyncVar]
    public PlayerType playerType;

    private Text Log1;


    private List<GameObject> Chip_List = new List<GameObject>();
   
    //public GameObject[] Chip_Array;
    //private int Chip_Num = 0;

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
        

        Log1 = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        
        Log1.text = string.Empty;
        if (!isLocalPlayer) return;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (GameManager.instance == null) return;
        if (GameManager.instance.isSetStart)
        {
            if (startTime < setTime)
            {
                startTime += Time.deltaTime;
                
                if (Chip_List.Count < 5)
                {
                    Log1.text = $"당신은 {playerType.ToString()}색입니다.\n원하는 위치에 알을 놓아주세요!\n남은 알 수 : {5-Chip_List.Count} / 남은 시간 : {Mathf.CeilToInt(setTime-startTime)}";
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("마우스 버튼 눌림");
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            if (hit.point.x > LB.position.x && hit.point.x < RT.position.x && hit.point.z > LB.position.z && hit.point.z < RT.position.z && !hit.collider.CompareTag("Chip"))
                            {
                                Debug.Log("생성요청하기");
                                CmdPlayerType(hit.point, playerType, netId);
                                //Chip_Num++;
                                //            obj.SetActive(true);
                                // 서버에서 바둑알 생성 요청
                                //CmdPlaceChip(hit.point);
                                //hit.transform.GetComponent<Kick_Chip>().ismine = true;
                            }
                        }
                    }
                }
                else
                {
                    Log1.text = "더 이상 알을 놓을 수 없습니다.";
                }
            }
            else
            {
                GameManager.instance.isSetStart = false;
                GameManager.instance.isGameStart = true;
            }
        }
        if(GameManager.instance.isGameStart)
        {
            
            if(GameManager.instance.isTurn.Equals(isMyTurn))
            {
                Log1.text = "내 차례";
            }
            else
            {
                Log1.text = "상대 차례";

            }
            Log1.text += $"\nMyColor : {playerType.ToString()}";
        }
        
    }
    
    [Command]
    private void CmdPlayerType(Vector3 pos, PlayerType type, uint playerid)
    {
        GameObject _Chip = Instantiate(NetworkManager.singleton.spawnPrefabs[0], pos, Quaternion.identity);

        _Chip.TryGetComponent(out Kick_Chip chip);

        NetworkServer.Spawn(_Chip);
        chip.TryGetComponent(out MeshRenderer chipRen);
        if (type.Equals(PlayerType.Black))
            chipRen.material.color = Color.black;
        else
            chipRen.material.color = Color.white;
        

        RpcChipSet(playerid, chip.netId);
    }
    [ClientRpc]
    private void RpcChipSet(uint playerid, uint chipid)
    {
        if(netId.Equals(playerid))
        {
            Chip_List.Add(NetworkClient.spawned[chipid].gameObject);
            GameManager.instance.SetCount(playerType, Chip_List.Count);
        }
        PutOn put = NetworkClient.spawned[playerid].GetComponent<PutOn>();

        Kick_Chip chip = NetworkClient.spawned[chipid].GetComponent<Kick_Chip>();
        MeshRenderer chipRen = NetworkClient.spawned[chipid].GetComponent<MeshRenderer>();
        if (put.playerType.Equals(PlayerType.Black))
        {
            chipRen.material.color = Color.black;
        }
        else
        {
            chipRen.material.color = Color.white;
        }
        chip.player = put;
        chip.type = put.playerType;

    }

    [Command]
    public void CmdKickEgg(Vector3 force, uint netid)
    {
        NetworkServer.spawned[netid].GetComponent<Kick_Chip>().rb.AddForce(force, ForceMode.Impulse);
        GameManager.instance.CmdChangeTurn();
    }
    



    public void HitChipStart(uint netid)
    {
        if (netId.Equals(netid))
        {
            playerType = PlayerType.Black;
        }
        else
        {
            playerType = PlayerType.White;
        }
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

    //public void KickForce(Vector3 force,GameObject chip)
    //{
    //    chip.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    //}
    #endregion
  
    
    public void Die(GameObject obj)
    {

        Chip_List.Remove(obj);
        GameManager.instance.SetCount(playerType, Chip_List.Count);


    }

}
