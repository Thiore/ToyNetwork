using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_Btn_Control : MonoBehaviour
{
    public Room_info roomInfo;
    [SerializeField] private Text RoomID;
    [SerializeField] private Text GameType;
    [SerializeField] private Text RoomName;
    [SerializeField] private Text HostName;
    [SerializeField] private Text CurrentPlayer;
    [SerializeField] private Image Img_Lock;
    [SerializeField] private Sprite[] spriteLock;

    public void SetRoomBtn(Room_info roominfo)
    {
        roomInfo = roominfo;
        RoomID.text = roominfo.Room_ID.ToString();
        GameType.text = roominfo.Game_Type;
        RoomName.text = roominfo.Room_Name;
        HostName.text = roominfo.Host_Name;
        CurrentPlayer.text = roominfo.Current_Players.ToString() + "/2";
        if (roominfo.Password != null)
            Img_Lock.sprite = spriteLock[0];
        else
            Img_Lock.sprite = spriteLock[1];
    }
    

    //roomID를 매개변수로 받아옴 받아올 방법 생각 필요
    //public void Join_Room()
    //{
    //    //RoomManager.instance.networkAddress = roomInfo.Room_ID.ToString();
    //    RoomManager.instance.SetGame(roomInfo.Room_ID.ToString());

    //    SQL_Manager.instance.UpdateRoomPlayerCount(roomInfo.Room_ID, 2);

    //    RoomManager.instance.StartClient();
    //    // 네트워크 연결 설정
    //}
}
