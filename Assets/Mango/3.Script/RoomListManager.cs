using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class RoomListManager : MonoBehaviour
{
    public static RoomListManager Instance { get; private set; } // 싱글톤 인스턴스

    [SerializeField] private Transform contentPanel; // Scroll View의 Content에 연결
    [SerializeField] private GameObject roomButtonPrefab; // 방 버튼 프리팹
    [SerializeField] private string lobbySceneName = "Lobby_Scene"; // 로비 씬 이름

    private List<GameObject> roomButtons = new List<GameObject>(); // 방 버튼 리스트

    private SQL_Manager sqlManager; // SQL_Manager 인스턴스 참조

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 객체를 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sqlManager = SQL_Manager.instance; // SQL_Manager 인스턴스 가져오기
    }

    private void Start()
    {
        FetchRoomList();
    }

    /// <summary>
    /// 새로운 방을 생성하는 메서드
    /// </summary>
    /// <param name="roomName">방 이름</param>
    /// <param name="maxPlayers">최대 플레이어 수</param>
    public void CreateRoom(string roomName, int maxPlayers)
    {
        if (RoomManager.singleton == null)
        {
            Debug.LogError("RoomManager Singleton is null");
            return;
        }

        // DB에 방 정보 저장
        bool isCreated = sqlManager.CreateRoom(roomName, maxPlayers, "127.0.0.1");

        if (isCreated)
        {
            // 방 목록 UI 갱신
            FetchRoomList();

            // 호스트는 로비 씬으로 이동
            RoomManager.singleton.StartHost();
            SceneManager.LoadScene(lobbySceneName);
        }
        else
        {
            Debug.LogError("Failed to create room in DB");
        }
    }

    /// <summary>
    /// DB에서 방 목록을 가져와 UI에 업데이트하는 메서드
    /// Start에 넣어두었고 추후 새로고침 버튼을 만들어 버튼이 눌릴때마다 호출되게 할 수 있음
    /// </summary>
    public void FetchRoomList()
    {
        // DB에서 방 목록을 가져옴
        sqlManager.FetchRoomList();

        // 기존 UI 삭제
        foreach (GameObject button in roomButtons)
        {
            Destroy(button);
        }
        roomButtons.Clear();

        // 방 목록 UI에 추가
        foreach (Room_info room in sqlManager.roomList)
        {
            AddRoomToUI(room);
        }
    }

    /// <summary>
    /// 방 정보를 UI에 추가하는 메서드
    /// </summary>
    /// <param name="roomInfo">방 정보 객체</param>
    private void AddRoomToUI(Room_info roomInfo)
    {
        GameObject newButton = Instantiate(roomButtonPrefab, contentPanel);

        // UI 요소 가져오기
        Text name = newButton.transform.Find("Room_Text").GetComponent<Text>();
        Image profile = newButton.transform.Find("Profile").GetComponent<Image>();
        Text game = newButton.transform.Find("Game_Text").GetComponent<Text>();

        // UI 요소 업데이트
        name.text = SQL_Manager.instance.info.User_Name;
        profile.sprite = Resources.Load<Sprite>($"{SQL_Manager.instance.info.User_Img}");
        game.text = "오셀로"; // 혹은 roomInfo.GameType에 따른 게임 이름 설정

        // 방에 입장하는 버튼 클릭 이벤트 설정
        newButton.GetComponent<Button>().onClick.AddListener(() => JoinRoom(roomInfo.Room_ID));
        roomButtons.Add(newButton);
    }

    /// <summary>
    /// 특정 방에 접속하는 메서드
    /// </summary>
    /// <param name="roomID">방 ID</param>
    public void JoinRoom(int roomID)
    {
        // DB에서 해당 방 정보를 가져옴
        Room_info room = sqlManager.roomList.Find(r => r.Room_ID == roomID);

        if (room != null)
        {
            // 클라이언트로서 해당 방에 접속
            NetworkManager.singleton.networkAddress = room.Host_ID;
            NetworkManager.singleton.StartClient();
        }
        else
        {
            Debug.LogError("Room not found in DB");
        }
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
