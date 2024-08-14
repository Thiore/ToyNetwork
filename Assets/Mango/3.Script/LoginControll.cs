using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginControll : MonoBehaviour
{
    private InputField ID_input;
    private InputField Password_input;

    [SerializeField]
    private GameObject create_panel;

    [SerializeField] private Text Log;

    private void Start()
    {
        ID_input = GameObject.Find("ID_Input").GetComponent<InputField>();
        Password_input = GameObject.Find("PW_Input").GetComponent<InputField>();

        create_panel = GameObject.Find("Account_Create");
        create_panel.SetActive(false);

        Log = GameObject.Find("Log").GetComponent<Text>();
    }

    public void Login_Btn()
    {
        if(ID_input.text.Equals(string.Empty)||Password_input.text.Equals(string.Empty))
        {
            Log.text = "Insert your ID or Password";
            return;
        }

        if(SQL_Manager.instance.Login(ID_input.text,Password_input.text))
        {
            //로그인이 성공
            User_info info = SQL_Manager.instance.info;
            Debug.Log(info.User_ID + " | " + info.User_Name + " | " + info.User_Password);
            gameObject.SetActive(false);

            SceneManager.LoadScene("RoomList_Scene");
        }
        else
        {
            //로그인 실패
            Log.text = "Check your ID or Password";
        }
    }

    public void Create_Btn()
    {
        create_panel.SetActive(true);

    }

    

}
