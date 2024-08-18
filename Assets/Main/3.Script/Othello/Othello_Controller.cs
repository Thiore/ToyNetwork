using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    private PlayerType playerType;
    private int playerLayer;

    private int empty;
    private int black;
    private int white;

    private GameObject emptyObj = null;

    private Dictionary<int, List<Vector3Int>> hitDisFilter = new Dictionary<int, List<Vector3Int>>();
    private Dictionary<Vector3Int, GameObject> FindChip = new Dictionary<Vector3Int, GameObject>();

    private Animator ChipAnim = null;
    private MeshRenderer ChipRenderer = null;

    private Color ReadyColor = new Color(1f, 1f, 1f, 0.5f);
    private Color SetColor = new Color(1f, 1f, 1f, 1f);

    private Dictionary<int,Coroutine> Chip_Co = new Dictionary<int, Coroutine>();


    private GameObject[] AllChip;
    private int BlackChip = 0;
    private int WhiteChip = 0;

    private bool isPlayerTurn = false;
    private bool isChipTurn = false;
    private bool isFinish = false;

    private bool isHost = false;

    public override void OnStartLocalPlayer()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            isHost = true;
        else if (NetworkClient.isConnected && !NetworkServer.active)
            isHost = false;
    }

    private void Awake()
    {
        //playerType = PlayerType.Black;
        //룸매니저가 host면 playerType Black, client면 White
        empty = LayerMask.NameToLayer("Empty");
        black = LayerMask.NameToLayer("Black");
        white = LayerMask.NameToLayer("White");

        if (isHost)
        {
            playerLayer = black;
            isPlayerTurn = true;
        }
        else
        {
            playerLayer = white;
            isPlayerTurn = false;
        }

        AllChip = GameObject.FindGameObjectsWithTag("Chip");
        foreach (GameObject chip in AllChip)
        {
            Vector3 pos = chip.transform.position;
            Vector3Int posInt = Vector3Int.RoundToInt(pos);
            
            FindChip.Add(posInt, chip);
        }
    }

    private void Update()
    {
        if(isPlayerTurn && !isChipTurn && !isFinish)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.CompareTag("Chip"))
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
                                ChipAnim.SetTrigger($"Ready{playerType}");
                            }
                        }
                    }
                }
            }
        }
    }
    [Client]
    public void ClickChip()
    {
        if (Input.GetMouseButtonDown(0) && hitDisFilter.Count > 0)
        {
            isChipTurn = true;
            if (ChipRenderer != null)
            {
                ChipRenderer.material.color = SetColor;
            }
            if (ChipAnim != null)
            {
                ChipAnim.SetTrigger($"Cr{playerType}");
            }
            TurnChip(playerType);
            //List<KeyValuePair<int, List<Vector3Int>>> turnChipToList = hitDisFilter.ToList();
            List<int> turnChipKey = new List<int>();
            List<List<Vector3Int>> turnChipValue = new List<List<Vector3Int>>();
            foreach(int key in hitDisFilter.Keys)
            {
                turnChipKey.Add(key);
                turnChipValue.Add(hitDisFilter[key]);
            }

            cmdSendMethod(Vector3Int.RoundToInt(emptyObj.transform.position), playerType, turnChipKey, turnChipValue, isChipTurn);
            emptyObj = null;
        }
    }

    [Command]
    private void cmdSendMethod(Vector3Int position, PlayerType player, List<int> turnChipKey, List<List<Vector3Int>> turnChipValue, bool chipturn)
    {
        
        RpcClickChip(position, player, turnChipKey, turnChipValue, chipturn);
    }

    [ClientRpc]
    private void RpcClickChip(Vector3Int position, PlayerType player, List<int> turnChipKey, List<List<Vector3Int>> turnChipValue, bool chipturn)
    {
        isChipTurn = chipturn;

        emptyObj = FindChip[position];
        //emptyObj.Chip.TryGetComponent(out ChipRenderer);
        //emptyObj.Chip.TryGetComponent(out ChipAnim);
        ChipRenderer.material.color = SetColor;
        ChipAnim.SetTrigger($"Cr{playerType}");

        hitDisFilter.Clear();
        foreach(int key in turnChipKey)
        {
            hitDisFilter.Add(turnChipKey[key], turnChipValue[key]);
        }
        //foreach (int key in turnChip.Keys)
        //{
        //    hitDisFilter.Add(key, new List<ChipData>());
        //    foreach (ChipData chipData in turnChip[key])
        //    {

        //        hitDisFilter[key].Add(FindChip[Vector3Int.RoundToInt(chipData.Position)]);

        //    }
        //}
        TurnChip(player);
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

    private void TurnChip(PlayerType player)
    {
        foreach(int key in hitDisFilter.Keys)
            Chip_Co.Add(key,StartCoroutine(TurnChip_co(key,player)));
        
    }

    private IEnumerator TurnChip_co(int index, PlayerType player)
    {
       
        for(int i = 0; i < hitDisFilter[index].Count;i++)
        {
            
            Animator turnChip;
            FindChip[hitDisFilter[index][i]].TryGetComponent(out turnChip);
            turnChip.SetTrigger($"Turn{player}");
            bool isturn = false;
            while(!isturn)
            {
                //if(hitDisFilter[index][i].Chip.layer.Equals(player))
                   //isturn = true;
                
                    yield return null;
            }

        }
        
        Chip_Co.Remove(index);
        
       
        if (Chip_Co.Count.Equals(0))
        {
            isChipTurn = false;
            BlackChip = 0;
            WhiteChip = 0;
            for(int i = 0; i < AllChip.Length;i++)
            {
                if (AllChip[i].layer.Equals(white))
                    WhiteChip += 1;
                else if (AllChip[i].layer.Equals(black))
                    BlackChip += 1;
                else
                    break;
            }
            if((WhiteChip + BlackChip).Equals(64))
            {
                isFinish = true;
                if(playerType.Equals(PlayerType.Black))
                {
                    if (BlackChip > WhiteChip)
                    {
                        Debug.Log("승리");
                    }
                    else
                        Debug.Log("패배");
                }
                else
                {
                    if (BlackChip > WhiteChip)
                    {
                        Debug.Log("패배");
                    }
                    else
                        Debug.Log("승리");
                }
            }
            hitDisFilter.Clear();
        }
        yield break;
    }

}
