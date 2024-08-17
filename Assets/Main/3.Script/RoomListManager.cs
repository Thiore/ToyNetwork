using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class RoomListManager : MonoBehaviour
{
    //public static RoomListManager Instance { get; private set; } // �̱��� �ν��Ͻ�

    [SerializeField] private Transform contentPanel; // Scroll View�� Content�� ����
    [SerializeField] private Room_Btn_Control roomButtonPrefab; // �� ��ư ������
    [SerializeField] private string lobbySceneName = "Lobby"; // �κ� �� �̸�

    //private List<GameObject> roomButtons = new List<GameObject>(); // �� ��ư ����Ʈ
    private Dictionary<int, Room_Btn_Control> roomButtonDic = new Dictionary<int, Room_Btn_Control>();

    private SQL_Manager sqlManager; // SQL_Manager �ν��Ͻ� ����

    private void Awake()
    {
        //// �̱��� �ν��Ͻ� ����
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject); // �� ��ȯ �� ��ü�� ����
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        sqlManager = SQL_Manager.instance; // SQL_Manager �ν��Ͻ� ��������
    }

    private void Start()
    {
        FetchRoomList();
    }



    /// <summary>
    /// DB���� �� ����� ������ UI�� ������Ʈ�ϴ� �޼���
    /// Start�� �־�ξ��� ���� ���ΰ�ħ ��ư�� ����� ��ư�� ���������� ȣ��ǰ� �� �� ����
    /// </summary>
    public void FetchRoomList()
    {
        // DB���� �� ����� ������
        sqlManager.FetchRoomList();

        // �� ��� UI�� �߰�
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
    /// �� ������ UI�� �߰��ϴ� �޼���
    /// </summary>
    /// <param name="roomInfo">�� ���� ��ü</param>
    private void AddRoomToUI(Room_info roomInfo)
    {
        Room_Btn_Control newButton = Instantiate(roomButtonPrefab, contentPanel);

        // UI ��� ������Ʈ
        newButton.SetRoomBtn(roomInfo);

        // �濡 �����ϴ� ��ư Ŭ�� �̺�Ʈ ����
        newButton.GetComponent<Button>().onClick.AddListener(() => newButton.Join_Room());
        roomButtonDic.Add(roomInfo.Room_ID,newButton);
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
