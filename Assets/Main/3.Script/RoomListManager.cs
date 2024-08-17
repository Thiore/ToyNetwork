using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class RoomListManager : MonoBehaviour
{
    //public static RoomListManager Instance { get; private set; } // 싱글톤 인스턴스

    [SerializeField] private Transform contentPanel; // Scroll View의 Content에 연결
    [SerializeField] private Room_Btn_Control roomButtonPrefab; // 방 버튼 프리팹
    [SerializeField] private string lobbySceneName = "Lobby"; // 로비 씬 이름

    //private List<GameObject> roomButtons = new List<GameObject>(); // 방 버튼 리스트
    private Dictionary<int, Room_Btn_Control> roomButtonDic = new Dictionary<int, Room_Btn_Control>();

    private SQL_Manager sqlManager; // SQL_Manager 인스턴스 참조

    private void Awake()
    {
        //// 싱글톤 인스턴스 설정
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject); // 씬 전환 시 객체를 유지
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        sqlManager = SQL_Manager.instance; // SQL_Manager 인스턴스 가져오기
    }

    private void Start()
    {
        FetchRoomList();
    }



    /// <summary>
    /// DB에서 방 목록을 가져와 UI에 업데이트하는 메서드
    /// Start에 넣어두었고 추후 새로고침 버튼을 만들어 버튼이 눌릴때마다 호출되게 할 수 있음
    /// </summary>
    public void FetchRoomList()
    {
        // DB에서 방 목록을 가져옴
        sqlManager.FetchRoomList();

        // 방 목록 UI에 추가
        foreach(int room in sqlManager.roomDic.Keys)
        {
            if (!roomButtonDic.ContainsKey(room))
                AddRoomToUI(sqlManager.roomDic[room]);
        }
        foreach(int room in roomButtonDic.Keys)
        {
            if (!sqlManager.roomDic.ContainsKey(room))
            {
                Destroy(roomButtonDic[room].gameObject);
                roomButtonDic.Remove(room);
            }               
        }

    }

    /// <summary>
    /// 방 정보를 UI에 추가하는 메서드
    /// </summary>
    /// <param name="roomInfo">방 정보 객체</param>
    private void AddRoomToUI(Room_info roomInfo)
    {
        Room_Btn_Control newButton = Instantiate(roomButtonPrefab, contentPanel);

        // UI 요소 업데이트
        newButton.SetRoomBtn(roomInfo);

        // 방에 입장하는 버튼 클릭 이벤트 설정
        newButton.GetComponent<Button>().onClick.AddListener(() => newButton.Join_Room());
        roomButtonDic.Add(roomInfo.Room_ID,newButton);
    }

    /// <summary>
    /// 방을 삭제하는 메서드
    /// </summary>
    /// <param="roomID">삭제할 방 ID</param>
    public void DeleteRoom(int roomID)
    {
        bool isDeleted = sqlManager.DeleteRoom(roomID);

        if (isDeleted)
        {
            // UI 업데이트
            FetchRoomList();
        }
        else
        {
            Debug.LogError("Failed to delete room in DB");
        }
    }

    /// <summary>
    /// 방의 현재 플레이어 수를 업데이트하는 메서드
    /// </summary>
    /// <param="roomID">방 ID</param>
    /// <param="currentPlayers">현재 플레이어 수</param>
    public void UpdateRoomPlayerCount(int roomID, int currentPlayers)
    {
        bool isUpdated = sqlManager.UpdateRoomPlayerCount(roomID, currentPlayers);

        if (!isUpdated)
        {
            Debug.LogError("Failed to update room player count in DB");
        }
    }
}
