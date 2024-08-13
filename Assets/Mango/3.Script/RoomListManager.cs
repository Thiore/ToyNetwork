using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;

public class RoomListManager : MonoBehaviour
{
    public string selectedGame;  // 사용자가 선택한 게임
    public Dropdown gameDropdown;

    public void Start()
    {
        // Dropdown UI를 통해 선택한 게임을 받아옵니다.
        gameDropdown.onValueChanged.AddListener(delegate { OnGameSelected(gameDropdown); });
    }

    public void OnGameSelected(Dropdown dropdown)
    {
        selectedGame = dropdown.options[dropdown.value].text;
    }

    public void OnCreateRoomButton()
    {
        // 선택한 게임을 다른 플레이어에게 알리기 위해 설정
        NetworkManager.singleton.networkAddress = selectedGame;
        NetworkManager.singleton.StartHost();  // 방을 호스팅합니다.

        // LobbyScene으로 이동
        SceneManager.LoadScene("LobbyScene");
    }
}
