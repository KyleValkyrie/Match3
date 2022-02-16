using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Tile selected; //storage for selected tile
    private SpriteRenderer Renderer; //storage for selected tile's renderer
    public Vector2Int Position; //storage for the position of selected tile

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }
    public void Select()
    {
        Renderer.color = Color.grey;
    }

    public void Unselect()
    {
        Renderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (selected != null)
        {
            if (selected == this)
                return;
            selected.Unselect();
            if (Vector2Int.Distance(selected.Position, Position) == 1)
            {
                GridManager.Instance.SwapTiles(Position, selected.Position);
                selected = null;
            }
            else
            {
                selected = this;
                Select();
            }
        }
        else
        {
            selected = this;
            Select();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
