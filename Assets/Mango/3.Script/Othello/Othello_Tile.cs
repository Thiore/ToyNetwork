using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Tile_Status { Tile, Hover, Clicked }
public class Othello_Tile : MonoBehaviour
{
    public Tile_Status tileStatus;
    private void Awake()
    {
        tileStatus = Tile_Status.Tile;
    }
}
