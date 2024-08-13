using Org.BouncyCastle.Asn1.Crmf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGameManager : MonoBehaviour
{
    private float currentTime = 0f;
    private float limitTime = 10f;


    [SerializeField] Gomoku_Logic logic;
    [SerializeField] Player player;

    private void Awake()
    {
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
        player = GameObject.FindObjectOfType<Player>();
    }


    public void GoGame()
    {
        StartCoroutine(GoGame_co());
    }

    private IEnumerator GoGame_co()
    {
        while(currentTime < limitTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= limitTime)
            {
                Debug.Log(currentTime);
                currentTime = 0f;
                yield return new WaitForSecondsRealtime(limitTime);
            }
        }
        Debug.Log("≈œ√æ");
        StartCoroutine(GoGame_co());
    }



}
