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

    [SerializeField]private Button Sub_Btn;
    [SerializeField] private Button Exit_Btn;
    private void Start()
    {
        ID_Input = GameObject.Find("Create_ID_Input").GetComponent<InputField>();
        Pass_Input = GameObject.Find("Create_PW_Input").GetComponent<InputField>();
        Pass_Check_Input = GameObject.Find("PW_Check_Input").GetComponent<InputField>();
        Nickname = GameObject.Find("Nickname_Input").GetComponent<InputField>();

        Sub_Btn = GameObject.Find("Submit").GetComponent<Button>();
        Exit_Btn = GameObject.Find("Exit_btn").GetComponent<Button>();

        Log_1 = GameObject.Find("Account_Log_1").GetComponent<Text>();
        Log_2 = GameObject.Find("Account_Log_2").GetComponent<Text>();

        Sub_Btn.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Empty_Check())
        {
            Password_Check();
        }
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
        if (!Pass_Input.text.Equals(Pass_Check_Input.text)) 
        {
            Log_1.text = "��й�ȣ�� ���� ���� �ʽ��ϴ�.";
            Sub_Btn.gameObject.SetActive(false);
            return;
        }
        Log_1.text = "";
        Sub_Btn.gameObject.SetActive(true);
    }

    public void Duplicate_Check()
    {
        if (SQL_Manager.instance.Duplicate_Check(ID_Input.text).Equals(1))
        {

            Log_2.text = ($"�ߺ��Ǵ� ���̵��Դϴ�.\n�ٸ� ���̵� �Է����ּ���.");
            return;
        }
        switch(SQL_Manager.instance.Duplicate_Check(ID_Input.text))
        {
            case 1:
                Log_2.text = "���ῡ �����߽��ϴ�.";
                break;
            case 2:
                Log_2.text = "ID�� 15���ں��� ���� �� �����ϴ�.";
                break;
            case 3:
                Log_2.text = ($"�ߺ��Ǵ� ���̵��Դϴ�.\n�ٸ� ���̵� �Է����ּ���.");
                break;
            case 4:
                Log_2.text = "TryCatch����";
                break;
            default:
                break;
        }

        this.gameObject.SetActive(false);
        SQL_Manager.instance.Create(ID_Input.text, Pass_Input.text, Nickname.text);

    }



}
