using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class RoomListManager : MonoBehaviour
{
    public static RoomListManager Instance { get; private set; } // �̱��� �ν��Ͻ�

    [SerializeField] private Transform contentPanel; // Scroll View�� Content�� ����
    [SerializeField] private GameObject roomButtonPrefab; // �� ��ư ������
    [SerializeField] private string lobbySceneName = "Lobby_Scene"; // �κ� �� �̸�

    private List<GameObject> roomButtons = new List<GameObject>(); // �� ��ư ����Ʈ

    private SQL_Manager sqlManager; // SQL_Manager �ν��Ͻ� ����

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ��ü�� ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sqlManager = SQL_Manager.instance; // SQL_Manager �ν��Ͻ� ��������
    }

    private void Start()
    {
        FetchRoomList();
    }

    /// <summary>
    /// ���ο� ���� �����ϴ� �޼���
    /// </summary>
    /// <param name="roomName">�� �̸�</param>
    /// <param name="maxPlayers">�ִ� �÷��̾� ��</param>
    public void CreateRoom(string roomName, int maxPlayers)
    {
        if (RoomManager.singleton == null)
        {
            Debug.LogError("RoomManager Singleton is null");
            return;
        }

        // DB�� �� ���� ����
        bool isCreated = sqlManager.CreateRoom(roomName, maxPlayers, "127.0.0.1");

        if (isCreated)
        {
            // �� ��� UI ����
            FetchRoomList();

            // ȣ��Ʈ�� �κ� ������ �̵�
            RoomManager.singleton.StartHost();
            SceneManager.LoadScene(lobbySceneName);
        }
        else
        {
            Debug.LogError("Failed to create room in DB");
        }
    }

    /// <summary>
    /// DB���� �� ����� ������ UI�� ������Ʈ�ϴ� �޼���
    /// Start�� �־�ξ��� ���� ���ΰ�ħ ��ư�� ����� ��ư�� ���������� ȣ��ǰ� �� �� ����
    /// </summary>
    public void FetchRoomList()
    {
        // DB���� �� ����� ������
        sqlManager.FetchRoomList();

        // ���� UI ����
        foreach (GameObject button in roomButtons)
        {
            Destroy(button);
        }
        roomButtons.Clear();

        // �� ��� UI�� �߰�
        foreach (Room_info room in sqlManager.roomList)
        {
            AddRoomToUI(room);
        }
    }

    /// <summary>
    /// �� ������ UI�� �߰��ϴ� �޼���
    /// </summary>
    /// <param name="roomInfo">�� ���� ��ü</param>
    private void AddRoomToUI(Room_info roomInfo)
    {
        GameObject newButton = Instantiate(roomButtonPrefab, contentPanel);

        // UI ��� ��������
        Text name = newButton.transform.Find("Room_Text").GetComponent<Text>();
        Image profile = newButton.transform.Find("Profile").GetComponent<Image>();
        Text game = newButton.transform.Find("Game_Text").GetComponent<Text>();

        // UI ��� ������Ʈ
        name.text = SQL_Manager.instance.info.User_Name;
        profile.sprite = Resources.Load<Sprite>($"{SQL_Manager.instance.info.User_Img}");
        game.text = "������"; // Ȥ�� roomInfo.GameType�� ���� ���� �̸� ����

        // �濡 �����ϴ� ��ư Ŭ�� �̺�Ʈ ����
        newButton.GetComponent<Button>().onClick.AddListener(() => JoinRoom(roomInfo.Room_ID));
        roomButtons.Add(newButton);
    }

    /// <summary>
    /// Ư�� �濡 �����ϴ� �޼���
    /// </summary>
    /// <param name="roomID">�� ID</param>
    public void JoinRoom(int roomID)
    {
        // DB���� �ش� �� ������ ������
        Room_info room = sqlManager.roomList.Find(r => r.Room_ID == roomID);

        if (room != null)
        {
            // Ŭ���̾�Ʈ�μ� �ش� �濡 ����
            NetworkManager.singleton.networkAddress = room.Host_ID;
            NetworkManager.singleton.StartClient();
        }
        else
        {
            Debug.LogError("Room not found in DB");
        }
    }

    /// <summary>
    /// ���� �����ϴ� �޼���
    /// </summary>
    /// <param="roomID">������ �� ID</param>
    public void DeleteRoom(int roomID)
    {
        bool isDeleted = sqlManager.DeleteRoom(roomID);

        if (isDeleted)
        {
            // UI ������Ʈ
            FetchRoomList();
        }
        else
        {
            Debug.LogError("Failed to delete room in DB");
        }
    }

    /// <summary>
    /// ���� ���� �÷��̾� ���� ������Ʈ�ϴ� �޼���
    /// </summary>
    /// <param="roomID">�� ID</param>
    /// <param="currentPlayers">���� �÷��̾� ��</param>
    public void UpdateRoomPlayerCount(int roomID, int currentPlayers)
    {
        bool isUpdated = sqlManager.UpdateRoomPlayerCount(roomID, currentPlayers);

        if (!isUpdated)
        {
            Debug.LogError("Failed to update room player count in DB");
        }
    }
}
