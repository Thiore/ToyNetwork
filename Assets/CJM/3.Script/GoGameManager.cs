using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGameManager : NetworkBehaviour
{
    public int myTurn = 1;

    private float currentTime = 0f;
    private float limitTime = 10f;


    [SerializeField] Gomoku_Logic logic;
    [SerializeField] Player player;

    private void Awake()
    {
        Debug.Log(myTurn % 2);
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
        player = GameObject.FindObjectOfType<Player>();
    }




}
