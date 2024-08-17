using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Othello_Controller : MonoBehaviour
{
    //[SerializeField] private Material mat;
    
    private PlayerType playerType;
    private int empty;
    private int black;
    private int white;

    private GameObject emptyObj = null;





    private void Awake()
    {
        //룸매니저가 host면 playerType Black, client면 White
        empty = LayerMask.NameToLayer("Empty");
        black = LayerMask.NameToLayer("Black");
        white = LayerMask.NameToLayer("White");

    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit,empty))
        {
            if(emptyObj != hit.collider.gameObject)
            {
                emptyObj = hit.collider.gameObject;
                RayToEmptyObj(emptyObj);


            }
        }
    }

    private void RayToEmptyObj(GameObject obj)
    {
        RaycastHit[] hit;
        hit = Physics.RaycastAll(obj.transform.position, Vector3.right, 15f);
        for (int i = 0; i < hit.Length; i++)
        {
            Debug.Log(hit[i].collider.name);
        }
            
        //float angle = 0f;
        //for(int i = 0; i < 8; i++)
        //{
        //    if (playerType.Equals(PlayerType.Black))
        //    {
        //        hit = Physics.RaycastAll(obj.transform.position, Quaternion.Euler(0, angle, 0) * Vector3.forward, 15f, white);
        //        if(hit.Length>0)
        //        {
        //            
        //            //Array.Sort(hit, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

        //        }

        //    }
        //    else
        //    {

        //    }
        //}
    }

}
