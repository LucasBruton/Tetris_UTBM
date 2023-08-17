using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TetraminosBoard : MonoBehaviour
{
    public TetraminoData[] tetraminoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Tilemap tilemap { get; protected set; }
    public Tilemap borderTilemap;
    public ActiveTetramino activePiece { get; protected set; }
    public Vector2Int spawnPosition = new Vector2Int(-1, 8);
    
    // Image used to show the next 4 moves
    public Image nextMoves;
    // Saves the data of the next 4 moves
    protected Queue<TetraminoData> dataNextMoves = new Queue<TetraminoData>();
    // Script that updates the image showing the newt 4 moves
    protected NextTetraminosDisplay nextTetraminosDisplay;

    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }
    
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<ActiveTetramino>();
        nextTetraminosDisplay = nextMoves.GetComponent<NextTetraminosDisplay>();

        // Creation of the 7 tiles
        for (int i = 0; i < tetraminoes.Length; i++) {
            tetraminoes[i].Initialize();
        }

        // Generate the next 4 moves at random
        for(int i = 0; i < 4; ++i) {
            dataNextMoves.Enqueue(tetraminoes[Random.Range(0, tetraminoes.Length)]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPiece();
    }

    public virtual void SpawnPiece()
    {
        // Retrieve data next tetramino
        TetraminoData data = dataNextMoves.Dequeue();
        // Generate the data of a futur move
        dataNextMoves.Enqueue(tetraminoes[Random.Range(0, tetraminoes.Length)]);
        // Update the display of the next move
        nextTetraminosDisplay.UpdateNextMoves(dataNextMoves);

        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition)) {
            SetOrClear(this.activePiece, true);
        } else {
            GameOver();
        }
    }

    public virtual void SetOrClear(ActiveTetramino piece, bool set)
    {
        Tile tile;
        // Set the tile
        if(set)
        {
            tile = piece.tetraminoData.tile;
        }
        // Clear the tile
        else
        {
            tile = null;
        }

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), tile);
        }
    }

    public void Clear(ActiveTetramino piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), null);
        }
    }

    public bool IsValidPosition(ActiveTetramino piece, Vector2Int position)
    {
        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePosition = piece.cells[i] + position;

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(new Vector3Int (tilePosition.x, tilePosition.y, 0))) 
            {
                return false;
            }

            // The piece go to the border of the board
            if (borderTilemap.HasTile(new Vector3Int (tilePosition.x, tilePosition.y, 0))) 
            {
                return false;
            }
        }

        return true;
    }

    /*public bool IsTileBelow(ActiveTetramino piece, Vector2Int position)
    {
        Vector2Int saveTile;
        List<Vector2Int> saveTiles = new List<Vector2Int>();

        saveTile = piece.cells[0] + position;

        // To find the minimal y
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePosition = piece.cells[i] + position;

            if(tilePosition.y <= saveTile.y)
            {
                saveTile = tilePosition;
            }

        }

        saveTiles.Add(saveTile);

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tilePosition = piece.cells[i] + position;

            if((tilePosition.y == saveTile.y) && (tilePosition != saveTile))
            {
                saveTiles.Add(tilePosition);
            }

        }

        foreach(Vector2Int tile in saveTiles)
        {
            if (tilemap.HasTile(new Vector3Int (tile.x, tile.y-1, 0))) 
            {
                return true;
            }

            if (borderTilemap.HasTile(new Vector3Int (tile.x, tile.y-1, 0))) 
            {
                return true;
            }
        }

        return false;
    }*/

    public bool IsTileBelow(ActiveTetramino piece, Vector2Int position)
    {
        List<Vector2Int> saveTiles = new List<Vector2Int>();

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector2Int tile = piece.cells[i];

            bool below = false;

            tile.y = tile.y - 1;

            // Save only the cells that can have a tile below
            for(int j = 0; j < piece.cells.Length; j++)
            {
                Vector2Int t = piece.cells[j];

                if(i != j)
                {
                    if(tile.x == piece.cells[j].x && tile.y == piece.cells[j].y)
                    {
                        below = true;
                    }

                }
            }
            
            if(!below)
            {
                tile.y = tile.y + 1;
                saveTiles.Add(tile);
            }
        }

        /*Debug.Log("SAVE TILES");
        Debug.Log(saveTiles.Count);*/

        // Research if there is a tile below each save cells
        foreach(Vector2Int tile in saveTiles)
        {
            Vector2Int newTile = tile + position;
            if (tilemap.HasTile(new Vector3Int (newTile.x, newTile.y-1, 0))) 
            {
                return true;
            }

            if (borderTilemap.HasTile(new Vector3Int (newTile.x, newTile.y-1, 0))) 
            {
                return true;
            }

        }

        return false;
    
    }

    public virtual void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row)) {
                LineClear(row);
            } else {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position)) {
                return false;
            }
        }

        return true;
    }

    public virtual void LineClear(int row)
    {
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }

    public Tilemap getTilemap()
    {
        return tilemap;
    }

    protected virtual void GameOver() {
        GameOverData.setLevel(Scene.TC11);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }

}
