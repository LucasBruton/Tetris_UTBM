using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

// TetraminosBoard for the TC2 level
public class TC2TetraminosBoard : TetraminosBoard
{
    // Array of the tiles corresonding to a UV Type
    public TC2UVTypeTile[] uvTiles;

    // Saves the data of the next 4 moves
    private TC2UVType typeActiveTetramino;
    // Represents where the tiles of placed UV tetraminoes are
    private Dictionary<int,  Dictionary<int, TC2UVTile>> boardTC2UVTiles;
    // Reference to the data of the UVs to complete to finish the level
    public TC2UVsToComplete uvsToComplete;
    public TC2ActiveTetramino activePiece { get; protected set; }

    private void Awake() {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<TC2ActiveTetramino>();
        nextTetraminosDisplay = nextMoves.GetComponent<NextTetraminosDisplay>();

        // Creation of the 7 pieces
        for (int i = 0; i < tetraminoes.Length; i++) {
            tetraminoes[i].Initialize();
        }

        // Generate the next 4 moves at random
        for(int i = 0; i < 4; ++i) {
            dataNextMoves.Enqueue(tetraminoes[Random.Range(0, tetraminoes.Length)]);
        }

        // Initialization of the Dictionnary that stores where the tiles of placed UV tetraminoes are
        RectInt bounds = Bounds;
        boardTC2UVTiles = new Dictionary<int,  Dictionary<int, TC2UVTile>>();
        Dictionary<int, TC2UVTile> line;
        for (int y = bounds.yMin; y <= bounds.yMax; y++)
        {
            line = new Dictionary<int, TC2UVTile>();

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                line.Add(x, null);
            }
            boardTC2UVTiles.Add(y, line);
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public override void SpawnPiece()
    {
        // Construction next tetramino
        TetraminoData data = dataNextMoves.Dequeue();
        int randomUVType = Random.Range(0, 6);
        TC2UVType type;
        
        // Randomly sets if the next tetramino is a UV tetramino or a normal tetramino
        if(randomUVType < 3) {
            typeActiveTetramino = uvTiles[randomUVType].type;
            data.tile = uvTiles[randomUVType].tile;
        } else {
            typeActiveTetramino = TC2UVType.NULL;
        }
        // Generate the data of a futur move
        dataNextMoves.Enqueue(tetraminoes[Random.Range(0, tetraminoes.Length)]);
        // Update the display of the next move
        nextTetraminosDisplay.UpdateNextMoves(dataNextMoves);
        // initiats the next active piece
        activePiece.Initialize(this, spawnPosition, data, typeActiveTetramino != TC2UVType.NULL);

        if (IsValidPosition(activePiece, spawnPosition)) {
            SetOrClear(this.activePiece, true);
        } else {
            GameOver();
        }
    }

    // Sets or clears the active tetramino
    public override void SetOrClear(ActiveTetramino piece, bool set)
    {

        if(set)
        {   // Sets a normal tetramino
            if(typeActiveTetramino == TC2UVType.NULL) {
                for (int i = 0; i < piece.cells.Length; i++)
                {
                    // Sets a tile for tetramino
                    Vector2Int tilePosition = piece.cells[i] + piece.position;
                    tilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), piece.tetraminoData.tile);
                }
            }
            // Sets an UV tetramino
            else {
                // Creats data for the UV tetramino
                TC2UVTetramino tc2UvTetramino = new TC2UVTetramino(typeActiveTetramino, uvsToComplete);
                TC2UVTile tc2UVTile;

                for (int i = 0; i < piece.cells.Length; i++)
                {
                    // Sets a tile for tetramino
                    Vector2Int tilePosition = piece.cells[i] + piece.position;
                    tilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), piece.tetraminoData.tile);
                    // Creats data for the tile of the UV tetramino
                    tc2UVTile = new TC2UVTile(tc2UvTetramino);
                    boardTC2UVTiles[tilePosition.y][tilePosition.x] = tc2UVTile;
                    tc2UvTetramino.AddTC2UVTile(tc2UVTile);
                }
                // printBoard();
            }
        }
        // Clear the tile
        else
        {
            for (int i = 0; i < piece.cells.Length; i++)
            {
                Vector2Int tilePosition = piece.cells[i] + piece.position;
                tilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), null);
            }
        }
    }

    public override void LineClear(int row)
    {
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            TileBase tile = tilemap.GetTile(position);
            tilemap.SetTile(position, null);
            // If a tile of a UV tetramino was destroyed, it informs the data of the tetramino
            if(boardTC2UVTiles[row][col] != null) {
                boardTC2UVTiles[row][col].DestroyTile();
                boardTC2UVTiles[row][col] = null;
            }
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
                boardTC2UVTiles[row][col] =  boardTC2UVTiles[row+1][col];
            }

            row++;
        }
    }

    protected override void GameOver() {
        GameOverData.setLevel(Scene.TC2);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }
}
