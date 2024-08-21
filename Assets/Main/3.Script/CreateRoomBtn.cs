using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomBtn : MonoBehaviour
{
    [SerializeField] private Text GameType;
    [SerializeField] private InputField RoomName;
    [SerializeField] private InputField Password;


    //방만들기
    public void Hosting_Room()
    {
        if (RoomName.text.Equals(string.Empty))
        {
            RoomName.text = SQL_Manager.instance.info.User_Name + "님의 방입니다.";
        }
        if (SQL_Manager.instance.CreateRoom(RoomName.text, GameType.text, Password.text != "" ? Password.text : null))
        {
            //RoomManager.instance.networkAddress = SQL_Manager.instance.SelectRoomID().ToString();
            //Debug.Log($"{RoomManager.instance.networkAddress}번 방에 입장합니다.");
            //RoomManager.instance.SetGame(SQL_Manager.instance.SelectRoomID().ToString());

        }
    }
}
