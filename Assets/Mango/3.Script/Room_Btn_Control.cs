using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Btn_Control : MonoBehaviour
{
    //�游���
    public void Hosting_Room()
    {
        string roomname=SQL_Manager.instance.info.User_Name;
        int img=SQL_Manager.instance.info.User_Img;
        string gametype = this.gameObject.name;
        RoomListManager.Instance.CreateRoom(roomname,2);
    }

    //roomID�� �Ű������� �޾ƿ� �޾ƿ� ��� ���� �ʿ�
    public void Join_Room(int roomID)
    {
        // DB���� �ش� �� ������ ������
        Room_info room = SQL_Manager.instance.roomList.Find(r => r.Room_ID == roomID);

        if (room != null)
        {
            // ��Ʈ��ũ ���� ����
            RoomManager.singleton.networkAddress = "127.0.0.1";
            RoomManager.singleton.GetComponent<TelepathyTransport>().port = 7777;

            // Ŭ���̾�Ʈ�μ� �ش� �濡 ����
            RoomManager.singleton.StartClient();
        }
        else
        {
            Debug.LogError("Room not found in DB");
        }
    }
}
