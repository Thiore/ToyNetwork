using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Linq;

public enum OthelloType { Black = 0 , White, Empty }
//[System.Serializable]
//public class ChipData
//{
//    public Vector3 Position;
//    public int ChipID; // GameObject의 인스턴스 ID로 대체

//    public ChipData()
//    {
//        Position = Vector3.zero;
//        ChipID = -1; // 기본값
//    }
//    public ChipData(Vector3 position, GameObject chip)
//    {
//        Position = position;
//        ChipID = chip.GetInstanceID();
//    }

//}

public class Othello_Controller : NetworkBehaviour
{
    //[SerializeField] private Material mat;
    [SerializeField] private Text Log;
    private PlayerType playerType;
    private int playerLayer;

    private int empty;
    private int black;
    private int white;

    private GameObject emptyObj = null;

    private Dictionary<int, List<Vector3Int>> hitDisFilter = new Dictionary<int, List<Vector3Int>>();
    private Dictionary<Vector3Int, GameObject> FindChip = new Dictionary<Vector3Int, GameObject>();
    //private Dictionary<int, List<Vector3Int>> TurnChipDic = new Dictionary<int, List<Vector3Int>>();

    private Animator ChipAnim = null;
    private MeshRenderer ChipRenderer = null;

    private Color ReadyColor = new Color(1f, 1f, 1f, 0.5f);
    private Color SetColor = new Color(1f, 1f, 1f, 1f);

    private Dictionary<int,Coroutine> Chip_Co = new Dictionary<int, Coroutine>();
    //private List<Coroutine> Chip_Co = new List<Coroutine>();

    private GameObject[] AllChip;
    private int BlackChip = 0;
    private int WhiteChip = 0;

    private bool isPlayerTurn = false;
    private bool isChipTurn = false;
    private bool isFinish = false;

    private bool isHost = false;

    private static event Action<Vector3Int, PlayerType,int, List<int>, List<List<Vector3Int>>> onMethod;
    [SyncVar]
    public int connectcount;
    [SyncVar]
    public bool isStart;
    [SyncVar]
    public bool isReady;

    private Vector3Int[] WhiteReset = new Vector3Int[2];
    private Vector3Int[] BlackReset = new Vector3Int[2];

    private void Awake()
    {
        //playerType = PlayerType.Black;
        //룸매니저가 host면 playerType Black, client면 White
        empty = LayerMask.NameToLayer("Empty");
        black = LayerMask.NameToLayer("Black");
        white = LayerMask.NameToLayer("White");

        Vector3 resetChip;
        resetChip = new Vector3(-1f, 0.45f, -1f);
        BlackReset[0] = Vector3Int.RoundToInt(resetChip);
        resetChip = new Vector3(1.04f, 0.45f, 1.04f);
        BlackReset[1] = Vector3Int.RoundToInt(resetChip);
        resetChip = new Vector3(1.04f, 0.45f, -1f);
        WhiteReset[0] = Vector3Int.RoundToInt(resetChip);
        resetChip = new Vector3(-1f, 0.45f, 1.04f);
        WhiteReset[1] = Vector3Int.RoundToInt(resetChip);

        AllChip = GameObject.FindGameObjectsWithTag("Chip");
        foreach (GameObject chip in AllChip)
        {
            Vector3 pos = chip.transform.position;
            Vector3Int posInt = Vector3Int.RoundToInt(pos);
            
            FindChip.Add(posInt, chip);
        }
        isStart = false;
        isReady = false;
        isChipTurn = false;
    }
    private void Start()
    {
        if (!isLocalPlayer) return;
        //GetCount();
        Debug.Log(connectcount);
        if (1 == connectcount%2)
            isHost = true;

        if (isHost)
        {
            playerType = PlayerType.Black;
            playerLayer = black;
            isPlayerTurn = true;
        }
        else
        {
            playerType = PlayerType.White;
            playerLayer = white;
            isPlayerTurn = false;
        }

        
    }

    public override void OnStartAuthority()
    {
        if (isLocalPlayer)
            transform.GetChild(0).gameObject.SetActive(true);
        onMethod += ClickChip;
        
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!isLocalPlayer) return;

        onMethod -= ClickChip;
        
    }

    public void SetConnectionOrder(int order)
    {
        
        connectcount = order;
    }
    [Command]
    public void CmdSetReady(bool ready)
    {
        isReady = ready;
        RoomManager.instance.CheckOthello_AllPlayersReady();
    }
    public void SetReady()
    {
        //if (!isLocalPlayer) return;
        isReady = !isReady;
        CmdSetReady(isReady);
    }
    private void Update()
    {
        
        if (!isLocalPlayer || !isStart|| Chip_Co.Count > 0) return;

        if (Input.GetMouseButtonDown(0) && ChipAnim != null && !isChipTurn)
        {
            
            Send();
        }
        //Debug.Log(isPlayerTurn);
        ////Debug.Log(isChipTurn);
        //Debug.Log(isFinish);
        //Debug.Log(Chip_Co.Count);
        //Debug.Log("___________________");

        //Debug.Log("playerLayer " + playerLayer);
        //Debug.Log("isPlayerTurn " + isPlayerTurn);
        if (isPlayerTurn && !isFinish)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Chip"))
                {
                    if (emptyObj != hit.collider.gameObject)
                    {
                        if (emptyObj != null)
                        {
                            if (emptyObj.layer.Equals(empty))
                            {
                                if (ChipAnim != null)
                                {
                                    ChipAnim.SetTrigger("None");
                                    ChipRenderer.material.color = SetColor;
                                    ChipAnim = null;
                                    ChipRenderer = null;
                                }
                                emptyObj = null;
                            }
                        }

                        if (hit.collider.gameObject.layer.Equals(empty))
                        {
                            emptyObj = hit.collider.gameObject;
                            hitDisFilter.Clear();
                            if (RayToEmptyObj(emptyObj))
                            {
                                emptyObj.TryGetComponent(out ChipRenderer);
                                ChipRenderer.material.color = ReadyColor;
                                emptyObj.TryGetComponent(out ChipAnim);
                                string animType = $"Ready{playerType.ToString()}";
                                ChipAnim.SetTrigger(animType);
                            }
                        }



                    }
                    
                }
            }
        }
        //if (TurnChipDic.Count > 0)
        //    Debug.Log(TurnChipDic.Count);
        //if (isChipTurn)
        //    Debug.Log("나왜 안바뀜?");

        if (Chip_Co.Count.Equals(0)&&isChipTurn)
        {
            Debug.Log("여기는?");
            isChipTurn = false;
            isPlayerTurn = !isPlayerTurn;
            BlackChip = 0;
            WhiteChip = 0;
            for (int i = 0; i < AllChip.Length; i++)
            {
                if (AllChip[i].layer.Equals(white))
                    WhiteChip += 1;
                else if (AllChip[i].layer.Equals(black))
                    BlackChip += 1;
                else
                    break;
            }
            if ((WhiteChip + BlackChip).Equals(64))
            {
                isFinish = true;
                isStart = false;
                isReady = false;
                transform.GetChild(0).gameObject.SetActive(true);
                //Log.gameObject.SetActive(true);
                if (playerType.Equals(PlayerType.Black))
                {
                    if (BlackChip > WhiteChip)
                    {
                        Log.text = "승리";
                    }
                    else
                        Log.text = "패배";
                }
                else
                {
                    if (BlackChip > WhiteChip)
                    {
                        Log.text = "패배";
                    }
                    else
                        Log.text = "승리";
                }
                Invoke("Reset",3f);
            }
            //hitDisFilter.Clear();

        }

    }

    private void Reset()
    {
        foreach (Vector3Int vec3 in FindChip.Keys)
        {
            for(int i = 0; i < BlackReset.Length;i++)
            {
                if (FindChip[vec3].Equals(BlackReset[i]))
                {
                    FindChip[vec3].GetComponent<Animator>().SetTrigger("TurnBlack");
                    continue;
                }
                else if (FindChip[vec3].Equals(WhiteReset[i]))
                {
                    FindChip[vec3].GetComponent<Animator>().SetTrigger("TurnWhite");
                    continue;
                }
            }
            FindChip[vec3].GetComponent<Animator>().SetTrigger("None");

        }



    }
        [Client]
    public void Send()
    {
        if (!isLocalPlayer) return;
        List<int> turnChipKey = new List<int>();
        List<List<Vector3Int>> turnChipValue = new List<List<Vector3Int>>();
        foreach (int key in hitDisFilter.Keys)
        {
            turnChipKey.Add(key);
            turnChipValue.Add(hitDisFilter[key]);
        }
        cmdSendMethod(Vector3Int.RoundToInt(emptyObj.transform.position), playerType,playerLayer, turnChipKey, turnChipValue);
        

    }

    [Command]
    private void cmdSendMethod(Vector3Int position, PlayerType player, int Layer, List<int> turnChipKey, List<List<Vector3Int>> turnChipValue)
    {
        RpcClickChip(position, player, Layer, turnChipKey, turnChipValue);
    }

    [ClientRpc]
    private void RpcClickChip(Vector3Int position, PlayerType player, int Layer, List<int> turnChipKey, List<List<Vector3Int>> turnChipValue)
    {
        onMethod?.Invoke(position, player, Layer, turnChipKey, turnChipValue);
        
    }

    private void ClickChip(Vector3Int position, PlayerType player, int Layer, List<int> turnChipKey, List<List<Vector3Int>> turnChipValue)
    {
        isChipTurn = true;
        emptyObj = null;
        emptyObj = FindChip[position];
        emptyObj.TryGetComponent(out ChipRenderer);
        emptyObj.TryGetComponent(out ChipAnim);
        ChipRenderer.material.color = SetColor;
        string animType = $"Cr{player.ToString()}";
        ChipAnim.SetTrigger(animType);

        
        for(int i = 0; i < turnChipKey.Count;i++)
        {
            Chip_Co.Add(i, StartCoroutine(TurnChip_co(i, turnChipValue[i], player, Layer)));
        }


    }


    private bool RayToEmptyObj(GameObject obj)
    {
            if (playerType.Equals(PlayerType.Black))
            {
                SearchChip(obj);
            }
            else
            {
                SearchChip(obj);
            }


        return hitDisFilter.Count > 0;
    }

    private void SearchChip(GameObject obj)
    {
        RaycastHit[] hits;
        float angle = 0;
        for (int i = 0; i < 8; i++)
        {
            Vector3 Dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            hits = Physics.RaycastAll(obj.transform.position, Dir, 17f, (1 << white) | (1 << black) | (1 << empty),QueryTriggerInteraction.Ignore);
            //hits = Physics.RaycastAll(obj.transform.position, Dir, 15f, white | black | empty);
            
            if (hits.Length > 0)
            {
               
                Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));//거리순서대로 정리
                //첫번째부터 같거나 빈거면 다음각도 검사
                if (hits[0].collider.gameObject.layer.Equals(playerLayer) || hits[0].collider.gameObject.layer.Equals(empty))
                {
                    angle += 45f;
                    continue;
                }                   
                else
                    hitDisFilter.Add(i, new List<Vector3Int>());


                for (int j = 0; j < hits.Length; j++)
                {
                    if (hits[j].collider.gameObject.layer.Equals(empty)) // List를 플레이어타입과 다른색으로 채우다 다른게 나오게되면 다시 다음 각도로 이동
                    {
                        hitDisFilter.Remove(i);
                        break;
                    }
                    if (((i%2).Equals(0))?hits[j].distance < 2.05f * (j + 1): hits[j].distance < 2.9f*(j+1))
                    {
                        if (hits[j].collider.gameObject.layer.Equals(playerLayer))
                        {
                            break;
                        }
                        hitDisFilter[i].Add(Vector3Int.RoundToInt(hits[j].collider.transform.position));

                    }
                }
                
            }
            angle += 45f;
        }
    }



    private IEnumerator TurnChip_co(int key, List<Vector3Int> values, PlayerType player, int layer)
    {
        
        foreach (Vector3Int vec3 in values)
        {
           
            Animator turnChip;
            FindChip[vec3].TryGetComponent(out turnChip);
            string animType = $"Turn{player.ToString()}";
            turnChip.SetTrigger(animType);
            bool isturn = false;
            while (!isturn)
            {
                if (turnChip.gameObject.layer.Equals(layer))
                    isturn = true;

                yield return null;
            }
        }

        Chip_Co.Remove(key);



    }

}
