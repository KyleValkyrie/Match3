using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    private int dimension = 8;
    private GameObject[,] Grid;
    private float distance = 1.3f;
    public TMP_Text score;

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;
            score.text = "Score: " + _score.ToString();
        }
    }

    SpriteRenderer GetSpriteRendererAt(int column, int row)//get the renderer of a tile
    {
        if (column < 0 || column >= dimension
             || row < 0 || row >= dimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
    }

    Sprite GetSpriteAt(int column, int row) //get the sprite value of a tile
    {
        if (column < 0 || column >= dimension
            || row < 0 || row >= dimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position) //swapping 2 tiles
    {

        
        GameObject tile1 = Grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

        GameObject tile2 = Grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();

        
        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;

        bool changesOccurs = CheckMatches();
        if (!changesOccurs)
        {
            temp = renderer1.sprite;
            renderer1.sprite = renderer2.sprite;
            renderer2.sprite = temp;
        }
        else
        {
            do
            {
                PieceFall();
            } while (CheckMatches());
        }
    }

    bool CheckMatches()//flag when matches appear
    {
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer> (); 
        for (int row = 0; row < dimension; row++)
        {
            for (int column = 0; column < dimension; column++) 
            {
                SpriteRenderer current = GetSpriteRendererAt(column, row); 

                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(column, row, current.sprite); 
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current); 
                }

                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(column, row, current.sprite); 
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(current);
                }
            }
        }

        foreach (SpriteRenderer renderer in matchedTiles) 
        {
            renderer.sprite = null;
        }
        Score += matchedTiles.Count;
        return matchedTiles.Count > 0; 
    }

    List<SpriteRenderer> FindColumnMatchForTile(int col, int row, Sprite sprite)//check for horizontal matches
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < dimension; i++)
        {
            
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
            
        }
        return result;
    }

    List<SpriteRenderer> FindRowMatchForTile(int col, int row, Sprite sprite)//check for vertical matches
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < dimension; i++)
        {
            
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
            
        }
        return result;
    }


    void fillBoard() //fill our board with jewel sprites
    {
        Vector3 positionOffset = transform.position - new Vector3(dimension * distance / 2.0f, dimension * distance / 2.0f, 0);
        for (int row = 0; row < dimension; row++)
        {
            for (int column = 0; column < dimension; column++)
            {
                List<Sprite> possibleSprites = new List<Sprite>(Sprites); // list of sprites that do not contains the current sprite being checked


                Sprite left1 = GetSpriteAt(column - 1, row);
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left2 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }

                Sprite down1 = GetSpriteAt(column, row - 1);
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down2 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }

                GameObject newTile = Instantiate(TilePrefab);
                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = Sprites[Random.Range(0, possibleSprites.Count)];
                Tile tile = newTile.AddComponent<Tile>(); //call the Tile from the Tile script
                tile.Position = new Vector2Int(column, row); //call the position from the Tile script
                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * distance, row * distance, 0) + positionOffset;

                Grid[column, row] = newTile;
            }
        }
    }

    void PieceFall() //Make upper pieces fall down after match
    {
        for (int column = 0; column < dimension; column++)
        {
            for (int row = 0; row < dimension; row++) 
            {
                while (GetSpriteRendererAt(column, row).sprite == null) 
                {
                    for (int filler = row; filler < dimension - 1; filler++) 
                    {
                        SpriteRenderer current = GetSpriteRendererAt(column, filler); 
                        SpriteRenderer next = GetSpriteRendererAt(column, filler + 1);
                        current.sprite = next.sprite;
                    }
                    SpriteRenderer last = GetSpriteRendererAt(column, dimension - 1);
                    last.sprite = Sprites[Random.Range(0, Sprites.Count)]; 
                }
            }
        }
    }

    public void resetBoard()//this will Reset the board 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void quitGame()//this will quit the game
    {
        Application.Quit();
    }

    public static GridManager Instance { get; private set; } // create public instance for GridManager script
    void Awake() 
    { 
        Instance = this;
        Score = 0;
    }

 

    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[dimension, dimension];
        fillBoard();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
