using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;

// �� ���� Ŭ����
public class Room_info
{
    public int Room_ID { get; private set; }  // �� ID
    public string Room_Name { get; private set; }  // �� �̸�
    public int Max_Players { get; private set; }  // �ִ� �÷��̾� ��
    public int Current_Players { get; private set; }  // ���� �÷��̾� ��
    public string Host_ID { get; private set; }  // ���� ID

    // Room_info Ŭ������ ������
    public Room_info(int roomID, string roomName, int maxPlayers, int currentPlayers, string hostID)
    {
        Room_ID = roomID;
        Room_Name = roomName;
        Max_Players = maxPlayers;
        Current_Players = currentPlayers;
        Host_ID = hostID;
    }
}
// ������� ������ �����ϴ� Ŭ����
public class User_info
{
    // User_info Ŭ������ �Ӽ��� (�б� ����)
    public string User_ID { get; private set; }
    public string User_Name { get; private set; }
    public string User_Password { get; private set; }
    public int User_Img { get; private set; }
    public int K_win { get; private set; }
    public int K_lose { get; private set; }
    public int O_win { get; private set; }
    public int O_lose { get; private set; }

    // User_info Ŭ������ ������
    // ������� ID, �̸�, ��й�ȣ, �̹���, �¸� �� �й� ����� �ʱ�ȭ��
    public User_info(string id, string name, string password, int img, int k_win, int k_lose, int o_win, int o_lose)
    {
        User_ID = id;
        User_Name = name;
        User_Password = password;
        User_Img = img;
        K_win = k_win;
        K_lose = k_lose;
        O_win = o_win;
        O_lose = o_lose;
    }
}

public class SQL_Manager : MonoBehaviour
{
    public User_info info; // ���� �α��ε� ������� ������ ����
    public List<Room_info> roomList = new List<Room_info>(); // �� ���

    public MySqlConnection connection; // DB ������ ���� ��ü
    public MySqlDataReader reader; // SQL ���� ����� �о���� ��ü

    private string DB_Path = string.Empty; // DB ���� ���� ���

    public static SQL_Manager instance = null; // �̱��� ������ ���� �ν��Ͻ�

    // �̱��� ������ ���� �ʱ�ȭ �޼���
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ�� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DB_Path = Path.Combine(Application.streamingAssetsPath, "Database/config.json"); // DB ���� ������ ��θ� ����
        string serverinfo = Server_set(DB_Path); // ���� ���� �ε�

        try
        {
            if (serverinfo.Equals(string.Empty))
            {
                Debug.Log("server info null");
                return;
            }
            connection = new MySqlConnection(serverinfo);
            connection.Open();
            Debug.Log("DB Open connection compelete");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    // ���� ���� ������ �о�� DB ���� ������ �����ϴ� �޼���
    // ���� ��θ� �Է¹޾� Json ���Ͽ��� ���� ���� ������ �о��
    private string Server_set(string path)
    {
        if (File.Exists(path))
        {
            string JsonString = File.ReadAllText(path);
            JsonData itemData = JsonMapper.ToObject(JsonString);
            string serverInfo = $"Server = {itemData[0]["IP"]};" +
                                $"Database = {itemData[0]["TableName"]};" +
                                $"Uid = {itemData[0]["ID"]};" +
                                $"Pwd = {itemData[0]["PW"]};" +
                                $"Port = {itemData[0]["PORT"]};" +
                                "CharSet = utf8";
            return serverInfo;
        }


        return string.Empty;
    }

    // DB ���� ���¸� Ȯ���ϰ�, ������ �����ִٸ� �翬�� �õ�
    // DB �۾��� �����ϱ� ���� �׻� ȣ���Ͽ� ���� ���¸� Ȯ��
    private bool connection_check(MySqlConnection con)
    {
        if (con.State != System.Data.ConnectionState.Open)
        {
            con.Open();
            if (con.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        return true;
    }

    // �α��� ����� �����ϴ� �޼���
    // �Էµ� ID�� ��й�ȣ�� DB�� ����� ������ ��ġ�ϴ��� Ȯ��
    public bool Login(string id, string password)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // SQL ������ ���� ����� ������ ��ȸ
            string SQL_Command =
                string.Format($@"SELECT User_ID, User_Name, User_Password, User_Img, Kick_Win, Kick_Lose, O_Win, O_Lose 
                                FROM user_info 
                                WHERE User_ID= '{id}' AND User_Password ='{password}';");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();

            // ��ȸ�� �����Ͱ� �����ϴ��� Ȯ��
            if (reader.HasRows)
            {
                // ��ȸ�� �����͸� User_info ��ü�� ����
                while (reader.Read())
                {
                    string Id = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_ID"];
                    string Name = (reader.IsDBNull(1)) ? string.Empty : (string)reader["User_Name"];
                    string Pass = (reader.IsDBNull(2)) ? string.Empty : (string)reader["User_Password"];
                    int img = reader.GetInt32(3);
                    int k_win = reader.GetInt32(4);
                    int k_lose = reader.GetInt32(5);
                    int o_win = reader.GetInt32(6);
                    int o_lose = reader.GetInt32(7);

                    if (!Id.Equals(string.Empty) || !Pass.Equals(string.Empty))
                    {
                        // �α��� ���� �� User_info ��ü �ʱ�ȭ
                        info = new User_info(Id, Name, Pass, img, k_win, k_lose, o_win, o_lose);
                        if (!reader.IsClosed) reader.Close();
                        return true;
                    }
                    else
                    {
                        // �α��� ���� ó��
                        break;
                    }
                }
            }
            if (!reader.IsClosed) reader.Close();
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return false;
        }
    }

    // ID �ߺ� Ȯ�� �޼���
    // ȸ������ �� ID�� �̹� �����ϴ��� Ȯ���ϱ� ���� ���
    public int Duplicate_Check(string id)
    {
        try
        {
            if (!connection_check(connection))
            {
                return 1;
            }
            if(id.Length>15)
            {
                return 2;
            }

            // SQL ������ ID�� �����ϴ��� Ȯ��
            string SQL_Command =
                string.Format($@"SELECT * FROM user_info WHERE User_ID = '{id}'");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();

            // ID�� �̹� �����ϸ� false ��ȯ
            if (reader.HasRows)
            {
                if (!reader.IsClosed) reader.Close();
                return 3;
            }
            if (!reader.IsClosed) reader.Close();
            return 0; // �ߺ����� ������ true ��ȯ
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return 4;
        }
    }

    // ���ο� ����� ������ �����ϴ� �޼���
    // ȸ������ �� ȣ��Ǿ� DB�� ����� ������ ����
    public bool Create(string id, string password, string name)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // SQL ������ ���ο� ����� ���� ����
            string SQL_Command =
                string.Format($@"INSERT INTO user_info (User_ID, User_Name, User_Password) 
                                 VALUES('{id}', '{name}', '{password}');");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count >= 1)
            {
                Debug.Log("New user created successfully");
                return true;
            }
            if (!reader.IsClosed) reader.Close();
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return false;
        }
    }

    // ���� �α��ε� ������� ������ ������Ʈ�ϴ� �޼���
    // ���� ����� ����Ǿ��� ��, DB�� �ݿ��ϱ� ���� ���
    public bool UpdateUserInfo(int img, int k_win, int k_lose, int o_win, int o_lose)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // ���� �α��ε� ������� ������ ������Ʈ
            string SQL_Command =
                string.Format($@"UPDATE user_info 
                                 SET User_Img = {img}, Kick_Win = {k_win}, Kick_Lose = {k_lose}, O_Win = {o_win}, O_Lose = {o_lose} 
                                 WHERE User_ID = '{info.User_ID}';");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count >= 1)
            {
                Debug.Log("User information updated successfully");
                return true;
            }
            if (!reader.IsClosed) reader.Close();
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return false;
        }
    }

    // ����� ������ �����ϴ� �޼���
    // ȸ�� Ż�� �� ȣ��Ǿ� DB���� ����� ������ ����
    public bool DeleteAccount(string id)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // SQL ������ ����� ���� ����
            string SQL_Command =
                string.Format($@"DELETE FROM user_info WHERE User_ID = '{id}';");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count >= 1)
            {
                Debug.Log("User account deleted successfully");
                return true;
            }
            if (!reader.IsClosed) reader.Close();
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return false;
        }
    }


    // ���ο� ���� �����ϰ� RoomList ���̺� �����ϴ� �޼���
    // ���� ȣ������ �� ȣ��Ǿ� DB�� �� ������ ����
    public bool CreateRoom(string roomName, int maxPlayers, string hostID)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // ���ο� �� ������ RoomList ���̺� ����
            string SQL_Command =
                string.Format($@"INSERT INTO RoomList (RoomName, MaxPlayers, CurrentPlayers, RoomIP) 
                                 VALUES('{roomName}', {maxPlayers}, 1, '{hostID}');");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count >= 1)
            {
                Debug.Log("Room created successfully");
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    // RoomList ���̺��� Ư�� ���� �����ϴ� �޼���
    // ���� ����ǰų� ������ �� ȣ��Ǿ� DB���� �� ������ ����
    public bool DeleteRoom(int roomID)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // RoomList ���̺��� �� ���� ����
            string SQL_Command =
                string.Format($@"DELETE FROM RoomList WHERE Room_ID = {roomID};");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count >= 1)
            {
                Debug.Log("Room deleted successfully");
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    // RoomList ���̺��� ��� �� ������ ��ȸ�Ͽ� roomList�� �����ϴ� �޼���
    // �� ����� �����ϰų� Ŭ���̾�Ʈ�� �� ����� ��û�� �� ȣ��
    public void FetchRoomList()
    {
        try
        {
            if (!connection_check(connection))
            {
                return;
            }

            roomList.Clear(); // ���� �� ��� �ʱ�ȭ

            // RoomList ���̺��� ��� �� ���� ��ȸ
            string SQL_Command = "SELECT * FROM RoomList;";
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int roomID = reader.GetInt32("RoomID");
                string roomName = reader.GetString("RoomName");
                int maxPlayers = reader.GetInt32("MaxPlayers");
                int currentPlayers = reader.GetInt32("CurrentPlayers");
                string hostID = "asd";

                // ��ȸ�� �� ������ Room_info ��ü�� �����Ͽ� roomList�� �߰�
                Room_info room = new Room_info(roomID, roomName, maxPlayers, currentPlayers, hostID);
                roomList.Add(room);
            }

            if (!reader.IsClosed) reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
        }
    }

    // Ư�� ���� ���� �÷��̾� ���� ������Ʈ�ϴ� �޼���
    // �÷��̾ �濡 �����ϰų� ���� �� ȣ��
    public bool UpdateRoomPlayerCount(int roomID, int currentPlayers)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // Ư�� ���� ���� �÷��̾� �� ������Ʈ
            string SQL_Command =
                string.Format($@"UPDATE RoomList 
                                 SET Current_Players = {currentPlayers} 
                                 WHERE Room_ID = {roomID};");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count >= 1)
            {
                Debug.Log("Room player count updated successfully");
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

}
