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
        if(ID_input == null)
            ID_input = GameObject.Find("ID_Input").GetComponent<InputField>();
        if(Password_input == null)
            Password_input = GameObject.Find("PW_Input").GetComponent<InputField>();

        ID_input.Select();

        //create_panel = GameObject.Find("Account_Create");
        //create_panel.SetActive(false);

        //Log = GameObject.Find("Log").GetComponent<Text>();
    }

    public void Login_Btn()
    {
        if(ID_input.text.Equals(string.Empty)||Password_input.text.Equals(string.Empty))
        {
            Log.text = "Insert your ID or Password";
            return;
        }

        if (SQL_Manager.instance.Login(ID_input.text, Password_input.text))
        {

            //로그인이 성공
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            //로그인 실패
            Log.text = "Check your ID or Password";
        }
    }

    //public void Create_Btn()
    //{
    //    create_panel.SetActive(true);

    //}

    

}
