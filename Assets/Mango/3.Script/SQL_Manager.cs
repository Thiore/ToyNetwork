using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;

//Table 데이터 자료형으로 만들어 놓기~
public class User_info
{
    public string User_ID { get; private set; }
    public string User_Name { get; private set; }
    public string User_Password { get; private set; }

    public int k_win;
    public int k_lose;
    public int o_win;
    public int o_lose;

    public User_info(string id, string name, string password)
    {
        User_ID = id;
        User_Name = name;
        User_Password = password;
    }

}


public class SQL_Manager : MonoBehaviour
{
    public User_info info;

    public MySqlConnection connection; //연결
    public MySqlDataReader reader; // 데이터를 직접적으로 읽어오는 녀석

    [SerializeField] private string DB_Path = string.Empty;

    public static SQL_Manager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DB_Path = Application.dataPath + "/Database";
        string serverinfo = Server_set(DB_Path);
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


    private string Server_set(string path)
    {//파일이 있다면 Json을 풀어서 전달
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);//경로 파일
        }
        string JsonString = File.ReadAllText(path + "/config.json");

        JsonData itemdata = JsonMapper.ToObject(JsonString);
        string serverInfo =
            $"Server={itemdata[0]["IP"]};" +
            $"Database={itemdata[0]["TableName"]}; " +
            $"Uid={itemdata[0]["ID"]}; " +
            $"Pwd={itemdata[0]["PW"]}; " +
            $"Port={itemdata[0]["PORT"]}; " +
            "CharSet=utf8;";

        return serverInfo;
    }

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



    public bool Login(string id, string password)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }
            string SQL_Command =
                string.Format($@"SELECT User_ID,User_Name,User_Password FROM user_info
                                WHERE User_ID= '{id}' AND User_Password ='{password}';");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {//Reader가 읽은 데이터가 1개이상 존재합니까?
                //읽은 데이터를 하나씩 나열해야한다.
                while (reader.Read())
                {
                    //삼항연산자 
                    string Id = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_ID"];
                    string Name = (reader.IsDBNull(1)) ? string.Empty : (string)reader["User_Name"];
                    string Pass = (reader.IsDBNull(2)) ? string.Empty : (string)reader["User_Password"];

                    if (!Id.Equals(string.Empty) || !Pass.Equals(string.Empty))
                    {
                        info = new User_info(Id, Name, Pass);

                        if (!reader.IsClosed) reader.Close();
                        return true;
                    }
                    else
                    {
                        //로그인이 실패
                        break;
                    }

                }//while 끝
            }//if 끝
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

    //true면 가입 가능
    public bool Duplicate_Check(string id)
    {
        try
        {
            if (!connection_check(connection))
            {
                return false;
            }
            string SQL_Command =
                string.Format($@"SELECT * From user_info WHERE USER_ID = {id}");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                return false;
            }//if 끝
            if (!reader.IsClosed) reader.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return false;
        }
    }

    public bool Create(string id, string password, string name)
    {

        try
        {
            if (!connection_check(connection))
            {
                return false;
            }
            string SQL_Command =
                string.Format($@"INSERT INTO user_info VALUES('{id}','{name}','{password}');");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            int count = cmd.ExecuteNonQuery();
            if (count>=1)
            {

            }//if 끝
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


}
