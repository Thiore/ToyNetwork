using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OthelloChip : MonoBehaviour
{
    private int empty;
    private int black;
    private int white;
    private Animator Anim;

    private void Awake()
    {
        empty = LayerMask.NameToLayer("Empty");
        black = LayerMask.NameToLayer("Black");
        white = LayerMask.NameToLayer("White");
        
        if (gameObject.layer.Equals(white))
        {
            TryGetComponent(out Anim);
            Anim.SetTrigger("CrWhite");
        }
            
        if (gameObject.layer.Equals(black))
        {
            TryGetComponent(out Anim);
            Anim.SetTrigger("CrBlack");
        }
            
    }

    public void ChangeLayer(OthelloType type)
    {
        if(type.Equals(OthelloType.Black))
        {
            gameObject.layer = black;
        }
        else if(type.Equals(OthelloType.White))
        {
            gameObject.layer = white;
        }
        else
        {
            gameObject.layer = empty;
        }
            
    }
}
