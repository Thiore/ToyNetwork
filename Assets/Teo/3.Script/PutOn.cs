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
        if (isGameStart || !isLocalPlayer) // ������ �ִ� ���� �÷��̾ ����
        {
            //Debug.Log("���콺 ���� ����");
            return;
        }
        //Debug.Log("Put on Update ����");
        if (startTime < setTime)
        {
            startTime += Time.deltaTime;
            if (Chip_Queue.Count > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("���콺 ��ư ����");
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.point.x > LB.position.x && hit.point.x < RT.position.x && hit.point.z > LB.position.z && hit.point.z < RT.position.z && !hit.collider.CompareTag("Chip"))
                        {
                            // �������� �ٵϾ� ���� ��û
                            CmdPlaceChip(hit.point);
                        }
                    }
                }
            }
        }
    }

    // Ŭ���̾�Ʈ���� ������ �ٵϾ� ��ġ ����
    [Command]
    private void CmdPlaceChip(Vector3 position)
    {
        // ��� Ŭ���̾�Ʈ���� �ٵϾ� ��ġ ����ȭ
        RpcPlaceChip(position);
    }

    // ��� Ŭ���̾�Ʈ���� �ٵϾ� ��ġ
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
