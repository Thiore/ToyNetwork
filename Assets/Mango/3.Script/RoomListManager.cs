using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;

public class RoomListManager : MonoBehaviour
{
    public string selectedGame;  // ����ڰ� ������ ����
    public Dropdown gameDropdown;

    public void Start()
    {
        // Dropdown UI�� ���� ������ ������ �޾ƿɴϴ�.
        gameDropdown.onValueChanged.AddListener(delegate { OnGameSelected(gameDropdown); });
    }

    public void OnGameSelected(Dropdown dropdown)
    {
        selectedGame = dropdown.options[dropdown.value].text;
    }

    public void OnCreateRoomButton()
    {
        // ������ ������ �ٸ� �÷��̾�� �˸��� ���� ����
        NetworkManager.singleton.networkAddress = selectedGame;
        NetworkManager.singleton.StartHost();  // ���� ȣ�����մϴ�.

        // LobbyScene���� �̵�
        SceneManager.LoadScene("LobbyScene");
    }
}
