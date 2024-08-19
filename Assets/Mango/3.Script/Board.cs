using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : NetworkBehaviour
{
    public int Turn = 1;

    [SerializeField] private GameObject Chip_Pivot;
    [SerializeField] private GameObject chipPrefab;

    private void Awake()
    {
        Chip_Pivot = GameObject.Find("Chip_Pivot");
        chipPrefab = Resources.Load("Prefabs/GoGame_Chip") as GameObject;
    }


    public void InitBoard()
    {
        int index = 0;
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                Createchip(index, j, i);
                index++;
            }
        }

    }

    public void Createchip(int index, int j, int i)
    {
        GameObject chipobj = Instantiate(chipPrefab) as GameObject;
        chipobj.transform.SetParent(this.gameObject.transform);
        Chip_Pivot.transform.GetChild(index).gameObject.AddComponent<NetworkIdentity>();
        chipobj.transform.position = Chip_Pivot.transform.GetChild(index).position;
        NetworkIdentity net = chipobj.transform.GetComponent<NetworkIdentity>();
        NetworkServer.Spawn(chipobj, net.connectionToClient);
        RPCSetPre(chipobj, Chip_Pivot.transform.GetChild(index));
        if (chipobj.TryGetComponent(out Chip chip))
        {
            chip.Row = j;
            chip.Col = i;
        }
    }

    [ClientRpc]
    public void RPCSetPre(GameObject obj, Transform pre)
    {
        obj.transform.SetParent(pre);
    }

}
