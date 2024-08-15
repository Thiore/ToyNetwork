using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Btn_Control : MonoBehaviour
{
    //방만들기
    public void Hosting_Room()
    {
        string roomname=SQL_Manager.instance.info.User_Name;
        string img=SQL_Manager.instance.info.User_Img;
        string gametype = this.gameObject.name;
       // RoomListManager.Instance.CreateRoom(roomname,2);

        /// <summary>
        /// 새로운 방을 생성하는 메서드
        /// </summary>
        /// <param name="roomName">방 이름</param>
        /// <param name="maxPlayers">최대 플레이어 수</param>
        //public void CreateRoom(string roomName, int maxPlayers)
        //{
        //    if (RoomManager.singleton == null)
        //    {
        //        Debug.LogError("RoomManager Singleton is null");
        //        return;
        //    }

        //    // DB에 방 정보 저장
        //    bool isCreated = sqlManager.CreateRoom(roomName, maxPlayers, "127.0.0.1");

        //    if (isCreated)
        //    {
        //        // 방 목록 UI 갱신
        //        FetchRoomList();

        //        // 호스트는 로비 씬으로 이동
        //        RoomManager.singleton.StartHost();
        //        SceneManager.LoadScene(lobbySceneName);
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to create room in DB");
        //    }
        //}
    }

    //roomID를 매개변수로 받아옴 받아올 방법 생각 필요
    public void Join_Room(int roomID)
    {
        // DB에서 해당 방 정보를 가져옴
        Room_info room = SQL_Manager.instance.roomList.Find(r => r.Room_ID == roomID);

        if (room != null)
        {
            // 네트워크 연결 설정
            RoomManager.singleton.networkAddress = "127.0.0.1";
            RoomManager.singleton.GetComponent<TelepathyTransport>().port = 7777;

            // 클라이언트로서 해당 방에 접속
            RoomManager.singleton.StartClient();
        }
        else
        {
            Debug.LogError("Room not found in DB");
        }
    }
}
