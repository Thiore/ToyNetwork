using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountControl : MonoBehaviour
{

    [SerializeField]private InputField ID_Input;
    [SerializeField]private InputField Pass_Input;
    [SerializeField]private InputField Pass_Check_Input;
    [SerializeField] private InputField Nickname;

    [SerializeField] private Text Log_1;
    [SerializeField] private Text Log_2;

    [SerializeField] private Button Sub_Btn;
    [SerializeField] private Button Exit_Btn;

    private bool isPasswordCheck;

    private void OnEnable()
    {
        isPasswordCheck = false;
    }

    private void Start()
    {
        if(ID_Input == null)
            ID_Input = GameObject.Find("Create_ID_Input").GetComponent<InputField>();
        if(Pass_Input == null)
            Pass_Input = GameObject.Find("Create_PW_Input").GetComponent<InputField>();
        if(Pass_Check_Input == null)
            Pass_Check_Input = GameObject.Find("PW_Check_Input").GetComponent<InputField>();
        if(Nickname == null)
            Nickname = GameObject.Find("Nickname_Input").GetComponent<InputField>();

        if(Sub_Btn == null)
            Sub_Btn = GameObject.Find("Submit").GetComponent<Button>();
        if(Exit_Btn == null)
            Exit_Btn = GameObject.Find("Exit_btn").GetComponent<Button>();

        if(Log_1 == null)
            Log_1 = GameObject.Find("Account_Log_1").GetComponent<Text>();
        if(Log_2 == null)
            Log_2 = GameObject.Find("Account_Log_2").GetComponent<Text>();

        Sub_Btn.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(isPasswordCheck && Empty_Check())
            Sub_Btn.gameObject.SetActive(true);
    }

    private bool Empty_Check()
    {
        if (ID_Input.text.Equals("") || Pass_Input.text.Equals("") || Pass_Check_Input.text.Equals("") || Nickname.text.Equals(""))
        {
            return false;
        }
        return true;
    }



    public void Password_Check()
    {
        if(Pass_Input.text.Length<4)
        {
            Log_1.text = "비밀번호가 적절하지 않습니다.";
            isPasswordCheck = false;
            return;
        }
        if (!Pass_Input.text.Equals(Pass_Check_Input.text)) 
        {
            Log_1.text = "비밀번호가 서로 맞지 않습니다.";
            isPasswordCheck = false;
            return;
        }
        isPasswordCheck = true;
        Log_1.text = string.Empty;
        
    }

    public void Duplicate_Check()
    {
        switch(SQL_Manager.instance.Duplicate_Check(ID_Input.text))
        {
            case 1:
                Log_2.text = "연결에 실패했습니다.";
                Invoke("ClearLog", 2f);
                return;
            case 2:
                Log_2.text = ($"중복되는 아이디입니다.\n다른 아이디를 입력해주세요.");
                Invoke("ClearLog", 2f);
                return;
            case 3:
                Log_2.text = "TryCatch오류";
                Invoke("ClearLog", 2f);
                return;
            default:
                Log_2.text = $"{Nickname.text}님 환영합니다.";
                break;
        }
        SQL_Manager.instance.Create(ID_Input.text, Pass_Input.text, Nickname.text);
        Invoke("ClearLog", 2f);
        gameObject.SetActive(false);
    }
    private void ClearLog()
    {
        Log_2.text = string.Empty;
    }



}
