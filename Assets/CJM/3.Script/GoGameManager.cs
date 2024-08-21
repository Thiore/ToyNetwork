using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGameManager : MonoBehaviour
{
    public int myTurn = 1;

    [SerializeField] Gomoku_Logic logic;
    [SerializeField] Player player;

    private void Awake()
    {
        logic = GameObject.FindObjectOfType<Gomoku_Logic>();
        player = GameObject.FindObjectOfType<Player>();
    }




}
