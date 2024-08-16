using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;



// 방 정보 클래스
public class Room_info
{
    public int Room_ID { get; private set; }  // 방 ID
    public string Room_Name { get; private set; }  // 방 이름
    public string Game_Type { get; private set; }  // 게임 이름
    public int Current_Players;  // 현재 플레이어 수
    public string Host_Name;  // 방장 이름
    public string Password;  // 비밀번호

    // Room_info 클래스의 생성자
    public Room_info(int roomID, string roomName, string gameType, int currentPlayers, string hostName, string password = null)
    {
        Room_ID = roomID;
        Room_Name = roomName;
        Game_Type = gameType;
        Current_Players = currentPlayers;
        Host_Name = hostName;
        Password = password;
    }
}
// 사용자의 정보를 저장하는 클래스
public class User_info
{
    // User_info 클래스의 속성들 (읽기 전용)
    public string User_ID { get; private set; }
    public string User_Name { get; private set; }
    public string User_Password { get; private set; }
    public string User_Img { get; private set; }
    public int K_win { get; private set; }
    public int K_lose { get; private set; }
    public int O_win { get; private set; }
    public int O_lose { get; private set; }

    public bool Login { get; private set; }

    // User_info 클래스의 생성자
    // 사용자의 ID, 이름, 비밀번호, 이미지, 승리 및 패배 기록을 초기화함
    public User_info(string id, string name, string password, string img, int k_win, int k_lose, int o_win, int o_lose, int login)
    {
        User_ID = id;
        User_Name = name;
        User_Password = password;
        User_Img = img;
        K_win = k_win;
        K_lose = k_lose;
        O_win = o_win;
        O_lose = o_lose;
        Login = login != 0;
    }
    public void SetLogin(int login)
    {
        Login = login != 0;
    }
}

public class SQL_Manager : MonoBehaviour
{
    public User_info info; // 현재 로그인된 사용자의 정보를 저장
    //public List<Room_info> roomList = new List<Room_info>(); // 방 목록
    public Dictionary<int, Room_info> roomDic = new Dictionary<int, Room_info>();

    public MySqlConnection connection; // DB 연결을 위한 객체
    public MySqlDataReader reader; // SQL 쿼리 결과를 읽어오는 객체

    private string DB_Path = string.Empty; // DB 설정 파일 경로

    public static SQL_Manager instance = null; // 싱글톤 패턴을 위한 인스턴스

    public string ServerIP { get; private set; }

    // 싱글톤 패턴을 위한 초기화 메서드
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트가 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DB_Path = Path.Combine(Application.streamingAssetsPath, "Database/config.json"); // DB 설정 파일의 경로를 지정
        string serverinfo = Server_set(DB_Path); // 서버 정보 로드
        Debug.Log(serverinfo);

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
    #region DB 설정정보 읽어오기
    // 서버 설정 파일을 읽어와 DB 접속 정보를 생성하는 메서드
    // 파일 경로를 입력받아 Json 파일에서 서버 접속 정보를 읽어옴
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
    #endregion

    #region DB 연결상태 확인메서드
    // DB 연결 상태를 확인하고, 연결이 닫혀있다면 재연결 시도
    // DB 작업을 수행하기 전에 항상 호출하여 연결 상태를 확인
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
    #endregion

    #region 로그인
    // 로그인 기능을 수행하는 메서드
    // 입력된 ID와 비밀번호가 DB에 저장된 정보와 일치하는지 확인
    public bool Login(string id, string password)
    {
        try
        {
            if (!connection_check(connection))
        {
            return false;
        }

        // SQL 쿼리를 통해 사용자 정보를 조회
        string SQL_Command =
            string.Format($@"SELECT *
                                FROM user_info 
                                WHERE User_ID= '{id}' AND User_Password ='{password}';");
        MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
        reader = cmd.ExecuteReader();

        // 조회된 데이터가 존재하는지 확인
        if (reader.HasRows)
        {
            reader.Read();

            string Id = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_ID"];
            string Name = (reader.IsDBNull(1)) ? string.Empty : (string)reader["User_Name"];
            string Pass = (reader.IsDBNull(2)) ? string.Empty : (string)reader["User_Password"];
            string img = (reader.IsDBNull(3)) ? string.Empty : (string)reader["User_Img"];
            int k_win = reader.GetInt32(4);
            int k_lose = reader.GetInt32(5);
            int o_win = reader.GetInt32(6);
            int o_lose = reader.GetInt32(7);
            int login = reader.GetInt32(8);

            if (!Id.Equals(string.Empty) || !Pass.Equals(string.Empty))
            {

                // 로그인 성공 시 User_info 객체 초기화
                info = new User_info(Id, Name, Pass, img, k_win, k_lose, o_win, o_lose, login);
                if (!reader.IsClosed)
                    reader.Close();

                SQL_Command = string.Format($@"UPDATE user_info SET Login = {1} WHERE User_ID = '{id}';");
                cmd = new MySqlCommand(SQL_Command, connection);
                int count = cmd.ExecuteNonQuery();

                if (count >= 1)
                {
                    info.SetLogin(1);
                    Debug.Log($"{id} : Login successfully");
                    return true;
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
    #endregion

    #region 로그아웃
    public void UdateLogout()
    {
        connection_check(connection);

        // 현재 로그인된 사용자의 정보를 업데이트
        string SQL_Command =
            string.Format($@"UPDATE user_info 
                                 SET Login = {0} 
                                 WHERE User_ID = '{info.User_ID}';");
        MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
        int count = cmd.ExecuteNonQuery();

        if (count >= 1)
        {
            info.SetLogin(0);
            Debug.Log($"{info.User_ID} : Logout successfully");
            return;
        }
    }
    #endregion

    #region ID 중복 확인 메서드
    // 회원가입 시 ID가 이미 존재하는지 확인하기 위해 사용
    public int Duplicate_Check(string id)
    {
        try
        {
            if (!connection_check(connection))
            {
                return 1;
            }

            // SQL 쿼리로 ID가 존재하는지 확인
            string SQL_Command =
                string.Format($@"SELECT * FROM user_info WHERE User_ID = '{id}'");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();

            // ID가 이미 존재하면 false 반환
            if (reader.HasRows)
            {
                if (!reader.IsClosed) reader.Close();
                return 2;
            }
            if (!reader.IsClosed) reader.Close();
            return 0; // 중복되지 않으면 true 반환
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return 3;
        }
    }
    #endregion

    #region 회원가입
    // 새로운 사용자 계정을 생성하는 메서드
    // 회원가입 시 호출되어 DB에 사용자 정보를 삽입
    public bool Create(string id, string password, string name)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // SQL 쿼리로 새로운 사용자 정보 삽입
            string SQL_Command =
                string.Format($@"INSERT INTO user_info (User_ID, User_Name, User_Password) 
                                 VALUES('{id}', '{name}', '{password}');");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count.Equals(1))
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
    #endregion

    #region 사용자 데이터 업데이트
    // 현재 로그인된 사용자의 정보를 업데이트하는 메서드
    // 게임 기록이 변경되었을 때, DB에 반영하기 위해 사용
    public bool UpdateUserInfo(int img, int k_win, int k_lose, int o_win, int o_lose)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // 현재 로그인된 사용자의 정보를 업데이트
            string SQL_Command =
                string.Format($@"UPDATE user_info 
                                 SET User_Img = {img}, Kick_Win = {k_win}, Kick_Lose = {k_lose}, O_Win = {o_win}, O_Lose = {o_lose} 
                                 WHERE User_ID = '{info.User_ID}';");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count.Equals(1))
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
    #endregion

    #region 회원탈퇴
    // 사용자 계정을 삭제하는 메서드
    // 회원 탈퇴 시 호출되어 DB에서 사용자 정보를 삭제
    public bool DeleteAccount(string id)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // SQL 쿼리로 사용자 정보 삭제
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
    #endregion

    #region IP받아오기
    public bool GetIP()
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // SQL 쿼리로 ID가 존재하는지 확인
            string SQL_Command =
                string.Format($@"SELECT IP FROM admin WHERE User_ID = '관리자'");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();

            // ID가 이미 존재하면 false 반환
            if (reader.HasRows)
            {
                reader.Read();
                ServerIP = (reader.IsDBNull(0)) ? string.Empty : (string)reader["IP"];
                if (!reader.IsClosed) reader.Close();
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
    #endregion

    // 새로운 방을 생성하고 RoomList 테이블에 저장하는 메서드
    // 방을 호스팅할 때 호출되어 DB에 방 정보를 삽입
    public bool CreateRoom(string roomName, string gameType, string password)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }
            string SQL_Command = string.Empty;
            // 새로운 방 정보를 RoomList 테이블에 삽입
            if (password != null)
            {
                SQL_Command =
                string.Format($@"SHOW TABLE STATUS LIKE 'roomlist';
                                 ALTER TABLE roomlist AUTO_INCREMENT = 1;
                                 INSERT INTO RoomList (RoomName, GameType, HostName, Password) 
                                 VALUES('{roomName}', {gameType}, '{info.User_Name}', '{password}');");
            }
            else
            {
                SQL_Command =
                string.Format($@"SHOW TABLE STATUS LIKE 'roomlist';
                                 ALTER TABLE roomlist AUTO_INCREMENT = 1;
                                 INSERT INTO RoomList (RoomName, GameType, HostName) 
                                 VALUES('{roomName}', {gameType}, '{info.User_Name}');");
            }
            
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count.Equals(1))
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

    public int SelectRoomID()
    {
        try
        {
            if (!connection_check(connection))
            {
                return 0;
            }

            // SQL 쿼리로 ID가 존재하는지 확인
            string SQL_Command =
                string.Format($@"SELECT RoomID FROM roomlist WHERE HostName = '{info.User_Name}'");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();

            // ID가 이미 존재하면 false 반환
            if (reader.HasRows)
            {
                reader.Read();
                int roomid = reader.GetInt32("RoomID");
                if (!reader.IsClosed) reader.Close();
                return roomid;
            }
            if (!reader.IsClosed) reader.Close();
            return 0;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return 0;
        }
    }

    // RoomList 테이블에서 특정 방을 삭제하는 메서드
    // 방이 종료되거나 삭제될 때 호출되어 DB에서 방 정보를 제거
    public bool DeleteRoom(int roomID)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // RoomList 테이블에서 방 정보 삭제
            string SQL_Command =
                string.Format($@"DELETE FROM RoomList WHERE Room_ID = {roomID};");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count.Equals(1))
            {
                roomDic.Remove(roomID);
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

    // RoomList 테이블에서 모든 방 정보를 조회하여 roomList에 저장하는 메서드
    // 방 목록을 갱신하거나 클라이언트가 방 목록을 요청할 때 호출
    public void FetchRoomList()
    {
        try
        {
            if (!connection_check(connection))
            {
                return;
            }

            roomDic.Clear(); // 기존 방 목록 초기화

            // RoomList 테이블에서 모든 방 정보 조회
            string SQL_Command = "SELECT * FROM RoomList;";
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int roomID = reader.GetInt32("RoomID");
                    string roomName = (reader.IsDBNull(1)) ? string.Empty : (string)reader["RoomName"];
                    string gameType = (reader.IsDBNull(2)) ? string.Empty : (string)reader["GameType"];
                    int currentPlayers = reader.GetInt32("CurrentPlayers");
                    string hostName = (reader.IsDBNull(4)) ? string.Empty : (string)reader["GameType"];
                    string password = (reader.IsDBNull(5)) ? null : (string)reader["Password"];

                    // 조회한 방 정보를 Room_info 객체로 생성하여 roomList에 추가
                    Room_info room = new Room_info(roomID, roomName, gameType, currentPlayers, hostName, password);
                    if(!roomDic.ContainsKey(roomID))
                        roomDic.Add(roomID,room);
                }
            }
            if (!reader.IsClosed) reader.Close();
            return;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return;
        }
    }

    // 특정 방의 현재 플레이어 수를 업데이트하는 메서드
    // 플레이어가 방에 참가하거나 나갈 때 호출
    public bool UpdateRoomPlayerCount(int roomID, int currentPlayers)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // 특정 방의 현재 플레이어 수 업데이트
            string SQL_Command =
                string.Format($@"UPDATE RoomList 
                                 SET Current_Players = {currentPlayers} 
                                 WHERE Room_ID = {roomID};");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count >= 1)
            {
                roomDic[roomID].Current_Players = currentPlayers;
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
    // 특정 방의 현재 플레이어 수를 업데이트하는 메서드
    // 플레이어가 방에 참가하거나 나갈 때 호출
    public bool UpdateRoomChangeHost(int roomID, string hostName)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            // 특정 방의 현재 플레이어 수 업데이트
            string SQL_Command =
                string.Format($@"UPDATE RoomList 
                                 SET HostName = {hostName} 
                                 WHERE Room_ID = {roomID};");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();

            if (count >= 1)
            {
                roomDic[roomID].Host_Name = hostName;
                Debug.Log("Room change Host updated successfully");
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
