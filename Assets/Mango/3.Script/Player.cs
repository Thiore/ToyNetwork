using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color color;
    public enum Color
    {
        Black=0,
        White
    }

    private int myColor = 1;
    public int MyColor { get { return myColor; } }
    //0�� 1��
    [SerializeField] private Material[] chip_material;



}
