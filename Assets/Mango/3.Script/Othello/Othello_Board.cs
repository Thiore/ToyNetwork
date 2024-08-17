using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Othello_Board : MonoBehaviour
{

    [Header("Art Stuff")]
    [SerializeField] private Material tileMaterial;

    //Logic
    private int tileCount = 8;
    [SerializeField]  private GameObject chipPrefab;

    

    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    Vector2Int hitPosition;

    private void Awake()
    {
        GenerateAllTiles(2, tileCount);
    }

    private void Update()
    {
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryGetComponent(out Rigidbody rigid);
            

            rigid.AddForce(Vector3.up*50f + Vector3.forward*5f, ForceMode.Impulse);
            rigid.angularVelocity = Vector3.right * 10f;
            //this.GetComponent<Rigidbody>().useGravity = true;
            //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //this.GetComponent<Rigidbody>().AddForce(0f,3000f,3000f);
            //float randomX = Random.RandomRange(-1f, 1f);
            //float randomY = Random.RandomRange(-1f, 1f);
            //float randomZ = Random.RandomRange(-1f, 1f);

            //this.GetComponent<Rigidbody>().MoveRotation(new Quaternion(randomX,randomY,randomZ,1f).normalized);
        }
    }

    private void Status_Change(int x,int y)
    {
        currentHover = hitPosition;
        tiles[x, y].layer = LayerMask.NameToLayer("Hover");
        tiles[x, y].gameObject.GetComponent<Othello_Tile>().tileStatus = Tile_Status.Hover;
    }
    private void Status_Change(int x, int y, int hit_x,int hit_y)
    {
        tiles[x, y].layer = LayerMask.NameToLayer("Tile");
        tiles[x, y].gameObject.GetComponent<Othello_Tile>().tileStatus = Tile_Status.Tile;
        currentHover = hitPosition;
        tiles[hit_x, hit_y].layer = LayerMask.NameToLayer("Hover");
        tiles[hit_x, hit_y].gameObject.GetComponent<Othello_Tile>().tileStatus = Tile_Status.Hover;
    }




    //Init Tile
    private void GenerateAllTiles(float tileSize, int tileCount)
    {
        tiles = new GameObject[tileCount, tileCount];
        for(int i = 0; i < tileCount; i++)
        {
            for(int j = 0; j < tileCount; j++)
            {
                tiles[i, j] = GenerateSingleTile(tileSize, i,j);
                
            }
        }
    }

    private GameObject GenerateSingleTile(float tileSize,int x,int y)
    {
        GameObject tileObject = new GameObject(string.Format($"X:{x}, Y:{y}"));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material=tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x*tileSize,0.51f,y*tileSize);
        vertices[1] = new Vector3(x*tileSize, 0.51f, (y+1)*tileSize);
        vertices[2] = new Vector3((x+1)*tileSize, 0.51f, y*tileSize);
        vertices[3] = new Vector3((x+1)*tileSize, 0.51f, (y+1)*tileSize);

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;

        tileObject.layer = LayerMask.NameToLayer("Tile");
        mesh.RecalculateNormals();

        tileObject.AddComponent<BoxCollider>();
        tileObject.AddComponent<Othello_Tile>();
        GameObject Chip = Instantiate(chipPrefab);
        Chip.name = $"X:{x}, Y:{y}";
        Chip.transform.parent = tileObject.transform;
        Chip.transform.position = new Vector3(x*tileSize+1, tileObject.transform.position.y+0.6f, y*tileSize+1);

        return tileObject;
    }

    //Operation
    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for(int i = 0; i < tileCount; i++)
        {
            for(int j = 0; j < tileCount; j++)
            {
                if (tiles[i, j] == hitInfo)
                    return new Vector2Int(i, j);
            }
        }

        return -Vector2Int.one;
    }
}
