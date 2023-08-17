using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

// TetraminosBoard for the CSYear1 level
public class CSYear1TetraminosBoard : TetraminosBoard
{
    // Array of the tiles corresonding to an UV Type
    public CSYear1UVTypeTile[] uvTiles;

    // Saves the data of the next 4 moves
    private CSYear1UVType typeActiveTetramino;
    // Represents where the tiles of placed UV tetraminoes are
    private Dictionary<int,  Dictionary<int, CSYear1UVTile>> boardCSYear1UVTiles;
    // Reference to the data of the UVs to complete to finish the level
    public CSYear1UVsToComplete uvsToComplete;
    public CSYear1ActiveTetramino activePiece { get; protected set; }

    // Variable used to know in how many pieces will the faster mode activate for
    private int fasterPiecesIn;
    // Variable used to know for how many pieces will the faster mode be activated for
    private int fasterPiecesFor;
    // Text used to show information about the faster mode
    public TextMeshProUGUI FasterTextUI;

    private void Awake() {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<CSYear1ActiveTetramino>();
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
        boardCSYear1UVTiles = new Dictionary<int,  Dictionary<int, CSYear1UVTile>>();
        Dictionary<int, CSYear1UVTile> line;
        for (int y = bounds.yMin; y <= bounds.yMax; y++)
        {
            line = new Dictionary<int, CSYear1UVTile>();

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                line.Add(x, null);
            }
            boardCSYear1UVTiles.Add(y, line);
        }

        // initialze the variables used for the faster mode
        RandomizeFaster();
        FasterTextUI.text = "Dans "+ fasterPiecesIn.ToString() + " pièce(s)";
    }

    private void Start()
    {
        SpawnPiece();
    }

    public override void SpawnPiece()
    {
        // Construction next tetramino
        TetraminoData data = dataNextMoves.Dequeue();
        int randomUVType = Random.Range(0, 21);
        CSYear1UVType type;
        
        // Randomly sets if the next tetramino is a UV tetramino or a normal tetramino
        if(randomUVType <= 4) {
            typeActiveTetramino = uvTiles[randomUVType].type;
            data.tile = uvTiles[randomUVType].tile;
        } else if(randomUVType <= 7) {
            typeActiveTetramino = uvTiles[5].type;
            data.tile = uvTiles[5].tile;
        } else if(randomUVType <= 10) {
            typeActiveTetramino = uvTiles[6].type;
            data.tile = uvTiles[6].tile;
        } else {
            typeActiveTetramino = CSYear1UVType.NULL;
        }

        // Generate the data of a futur move
        dataNextMoves.Enqueue(tetraminoes[Random.Range(0, tetraminoes.Length)]);
        // Update the display of the next move
        nextTetraminosDisplay.UpdateNextMoves(dataNextMoves);

        // initiats the next active piece in normal mode if fasterPieceIn > 0, otherwise the piece is initialized in faster mode 
        if(fasterPiecesIn > 0) {
            FasterTextUI.text = "Dans "+ fasterPiecesIn.ToString() + " pièce(s)";
            fasterPiecesIn--;
            activePiece.Initialize(this, spawnPosition, data, typeActiveTetramino != CSYear1UVType.NULL, false);
        } else {
            if(fasterPiecesFor == 0) {
                RandomizeFaster();
                FasterTextUI.text = "Dans "+ fasterPiecesIn.ToString() + " pièce(s)";
            } else {
                FasterTextUI.text = "Pendant "+ fasterPiecesFor.ToString() + " pièce(s)";
            }
            fasterPiecesFor--;
            activePiece.Initialize(this, spawnPosition, data, typeActiveTetramino != CSYear1UVType.NULL, true);
        }
        
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
            if(typeActiveTetramino == CSYear1UVType.NULL) {
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
                CSYear1UVTetramino CSYear1UvTetramino = new CSYear1UVTetramino(typeActiveTetramino, uvsToComplete);
                CSYear1UVTile CSYear1UVTile;

                for (int i = 0; i < piece.cells.Length; i++)
                {
                    // Sets a tile for tetramino
                    Vector2Int tilePosition = piece.cells[i] + piece.position;
                    tilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), piece.tetraminoData.tile);
                    // Creats data for the tile of the UV tetramino
                    CSYear1UVTile = new CSYear1UVTile(CSYear1UvTetramino);
                    boardCSYear1UVTiles[tilePosition.y][tilePosition.x] = CSYear1UVTile;
                    CSYear1UvTetramino.AddCSYear1UVTile(CSYear1UVTile);
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
            if(boardCSYear1UVTiles[row][col] != null) {
                boardCSYear1UVTiles[row][col].DestroyTile();
                boardCSYear1UVTiles[row][col] = null;
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
                boardCSYear1UVTiles[row][col] =  boardCSYear1UVTiles[row+1][col];
            }

            row++;
        }
    }

    protected override void GameOver() {
        GameOverData.setLevel(Scene.ComputerScienceYear1);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }

    // randomize the variables used to make the level faster
    private void RandomizeFaster() {
        fasterPiecesFor = Random.Range(1, 6);
        fasterPiecesIn = Random.Range(10, 16);
    }
}
