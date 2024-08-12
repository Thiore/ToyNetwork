using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public int row { get; private set; }
    public int col { get; private set; }

    public Chip(int r, int c)
    {
        row = r;
        col = c;
    }
}