using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBoard : MonoBehaviour
{
    private Text Nickname;
    private Image Profile;
    private Text O_Rate;
    private Text K_Rate;
    private void Start()
    {
        Nickname = GameObject.Find("Nickname").GetComponent<Text>();
        Profile = GameObject.Find("Profile_IMG").GetComponent<Image>();
        O_Rate = GameObject.Find("O_Rate_Text").GetComponent<Text>();
        K_Rate = GameObject.Find("K_Rate_Text").GetComponent<Text>();

        Init();
    }

    private void Init()
    {
        Nickname.text = SQL_Manager.instance.info.User_Name;
        Profile.sprite = Resources.Load<Sprite>($"{SQL_Manager.instance.info.User_Img}");
        O_Rate.text = $"¿À¸ñ : ½Â{SQL_Manager.instance.info.O_win} / ÆÐ{SQL_Manager.instance.info.O_lose}";
        K_Rate.text = $"¾Ë±î±â : ½Â{SQL_Manager.instance.info.K_win} / ÆÐ{SQL_Manager.instance.info.K_lose}";
    }
}
