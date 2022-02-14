using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    private int width = 8;
    private int height = 8;
    public float distance = 1.3f;
    private GameObject[,] Grid;

    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position) // 1
    {

        // 2
        GameObject tile1 = Grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

        GameObject tile2 = Grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();

        // 3
        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;
    }

    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3(width * distance / 2.0f, height * distance / 2.0f, 0); 
        for (int row = 0; row < width; row++)
            for (int column = 0; column < height; column++) 
            {
                GameObject newTile = Instantiate(TilePrefab); 
                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>(); 
                renderer.sprite = Sprites[Random.Range(0, Sprites.Count)]; 
                newTile.transform.parent = transform; 
                newTile.transform.position = new Vector3(column * distance, row * distance, 0) + positionOffset; 
                Grid[column, row] = newTile;
                Tile tile = newTile.AddComponent<Tile>();
                tile.Position = new Vector2Int(column, row);
            }
    }
    public static GridManager Instance { get; private set; }
    void Awake() { Instance = this; }
    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[width, height];
        InitGrid();
    }

// Update is called once per frame
void Update()
    {
        
    }
}
