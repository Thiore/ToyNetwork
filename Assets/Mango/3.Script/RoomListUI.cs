using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mirror;

public class RoomListUI : MonoBehaviour
{
    public GameObject roomButtonPrefab;
    public Transform contentPanel;

    private Dictionary<string, GameObject> roomButtons = new Dictionary<string, GameObject>();

    public void AddRoom(string roomName, string hostName, string gameType)
    {
        if (!roomButtons.ContainsKey(roomName))
        {
            GameObject newButton = Instantiate(roomButtonPrefab, contentPanel);
            newButton.transform.Find("HostNameText").GetComponent<Text>().text = hostName;
            newButton.transform.Find("GameTypeText").GetComponent<Text>().text = gameType;

            // 방에 입장하는 버튼 클릭 이벤트 설정
            newButton.GetComponent<Button>().onClick.AddListener(() => JoinRoom(roomName));

            roomButtons[roomName] = newButton;
        }
    }

    public void RemoveRoom(string roomName)
    {
        if (roomButtons.ContainsKey(roomName))
        {
            Destroy(roomButtons[roomName]);
            roomButtons.Remove(roomName);
        }
    }

    public void JoinRoom(string roomName)
    {
        NetworkManager.singleton.networkAddress = roomName;
        NetworkManager.singleton.StartClient();
    }
}
