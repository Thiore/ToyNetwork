using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Othello_Chip : MonoBehaviour
{
    private Transform parent;
    private Material mat;

    private void Start()
    {
        parent = transform.parent;
        mat = GetComponent<MeshRenderer>().materials[0];
        mat .SetColor("_BaseColor",new Color(1,1,1,0));
    }
    
    private void OnMouseEnter()
    {
        if (parent.GetComponent<Othello_Tile>().tileStatus == Tile_Status.Clicked)
        {
            return;
        }
        else
        {
            //player 타입에 따라서 블랙이면 x기준 그거 180 돌려야함
            parent.GetComponent<Othello_Tile>().tileStatus = Tile_Status.Hover;
        }
    }
    private void OnMouseExit()
    {
        if (parent.GetComponent<Othello_Tile>().tileStatus != Tile_Status.Clicked)
        {
            parent.GetComponent<Othello_Tile>().tileStatus = Tile_Status.Tile;
        }
        else
        {

        }
        
    }
    private void OnMouseDown()
    {
        //player 타입에 따라서 블랙이면 x기준 그거 180 돌려야함
        parent.GetComponent<Othello_Tile>().tileStatus = Tile_Status.Clicked;
    }
    private void Update()
    {
        if (parent.GetComponent<Othello_Tile>().tileStatus == Tile_Status.Tile)
        {
            Change_Color(0f);
        }else if (parent.GetComponent<Othello_Tile>().tileStatus == Tile_Status.Hover)
        {
            Change_Color(0.5f);
        }
        else
        {
            Change_Color(1f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().useGravity=true;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            this.GetComponent<Rigidbody>().AddForce(0f, 100f, 100f);
            float randomX = Random.RandomRange(-100f, 100f);
            float randomY = Random.RandomRange(-100f, 100f);
            float randomZ = Random.RandomRange(-100f, 100f);

            this.GetComponent<Rigidbody>().MoveRotation(new Quaternion(randomX, randomY, randomZ, 1f).normalized);
        }
    }

    private void Change_Color(float value)
    {
        mat.SetColor("_BaseColor", new Color(1, 1, 1, value));
    }

    private Animator Ani;

    public void Flip(PlayerType type)
    {
        if (type == PlayerType.White)
        {
            Ani.SetTrigger("Flip_White");
        }
        else
        {
            Ani.SetTrigger("Flip_Black");
        }
    }
}
