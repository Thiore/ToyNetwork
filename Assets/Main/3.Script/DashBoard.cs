using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DashBoard : MonoBehaviour
{
    [SerializeField] private Text Nickname;
    [SerializeField] private Image Profile;
    //[SerializeField] private Text Od_Rate;
    [SerializeField] private Text O_Rate;
    [SerializeField] private Text K_Rate;
    private Transform DashBoard_Panel;
    private void Start()
    {
        if(Nickname == null)
            Nickname = GameObject.Find("Nickname").GetComponent<Text>();
        if(Profile == null)
            Profile = GameObject.Find("Profile_IMG").GetComponent<Image>();
        //if(Od_Rate == null)
        //    Od_Rate = GameObject.Find("Od_Rate_Text").GetComponent<Text>();
        if(O_Rate == null)
            O_Rate = GameObject.Find("O_Rate_Text").GetComponent<Text>();
        if(K_Rate == null)
            K_Rate = GameObject.Find("K_Rate_Text").GetComponent<Text>();

        DashBoard_Panel = GameObject.Find("DashBoard_Panel").transform;
        transform.SetParent(DashBoard_Panel);
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
