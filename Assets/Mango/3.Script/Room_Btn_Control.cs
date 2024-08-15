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
        string img=SQL_Manager.instance.info.User_Img;
        string gametype = this.gameObject.name;
       // RoomListManager.Instance.CreateRoom(roomname,2);

        /// <summary>
        /// ���ο� ���� �����ϴ� �޼���
        /// </summary>
        /// <param name="roomName">�� �̸�</param>
        /// <param name="maxPlayers">�ִ� �÷��̾� ��</param>
        //public void CreateRoom(string roomName, int maxPlayers)
        //{
        //    if (RoomManager.singleton == null)
        //    {
        //        Debug.LogError("RoomManager Singleton is null");
        //        return;
        //    }

        //    // DB�� �� ���� ����
        //    bool isCreated = sqlManager.CreateRoom(roomName, maxPlayers, "127.0.0.1");

        //    if (isCreated)
        //    {
        //        // �� ��� UI ����
        //        FetchRoomList();

        //        // ȣ��Ʈ�� �κ� ������ �̵�
        //        RoomManager.singleton.StartHost();
        //        SceneManager.LoadScene(lobbySceneName);
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to create room in DB");
        //    }
        //}
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
