using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

// TetraminosBoard for the Sector level
public class SectorTetraminosBoard : TetraminosBoard
{
    // Array of the tiles corresponding to a UV Type
    public SectorUVTypeTile[] uvTiles;

    // Array of the tiles corresponding to a sector
    public SectorTile[] sectorTiles;

    // Saves the data of the next 4 moves
    private SectorUVType typeActiveTetramino;
    // Represents where the tiles of placed UV tetraminoes are
    private Dictionary<int,  Dictionary<int, SectorUVTile>> boardSectorUVTiles;
    // Reference to the data of the UVs to complete to finish the level
    public SectorUVsToComplete uvsToComplete;
    public SectorActiveTetramino activePiece { get; protected set; }
    // Tile used for disabled lines
    public Tile tileDisabledLine;
    // Number of active pieces before a line is disabled
    private int nbPiecesBeforeDisabledLine;
    // Y coordinate of the next disabled line
    private int nextDisabledLine;
    // Number of active pieces that have been played since the last disabled line 
    private int disableLineIn = 0;
    // Text used to shows information about when the next line will be disabled
    public TextMeshProUGUI disableLineTextUI;

    // Tile used for by the TM tetraminoes
    private Tile tileTM;

    // UI image that shows the tile of the TM pieces
    public Image imageTM;

    private void Awake() {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<SectorActiveTetramino>();
        nextTetraminosDisplay = nextMoves.GetComponent<NextTetraminosDisplay>();
        nextDisabledLine = Bounds.yMin;
        nbPiecesBeforeDisabledLine = Random.Range(10, 16);

        disableLineTextUI.text = "Dans " + nbPiecesBeforeDisabledLine.ToString() + " pièce(s)";

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
        boardSectorUVTiles = new Dictionary<int,  Dictionary<int, SectorUVTile>>();
        Dictionary<int, SectorUVTile> line;
        for (int y = bounds.yMin; y <= bounds.yMax; y++)
        {
            line = new Dictionary<int, SectorUVTile>();

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                line.Add(x, null);
            }
            boardSectorUVTiles.Add(y, line);
        }

        // Find the TM sprite of the chosen sector
        for(int i = 0; i < sectorTiles.Length; ++i) {
            if(SectorData.getCurrentSector() == sectorTiles[i].sector) {
                tileTM = sectorTiles[i].tile;
                break;
            }
        }
        // Set the TM image of the UI of chosen TM sprite
        imageTM.sprite = tileTM.sprite;
    }

    private void Start()
    {
        SpawnPiece();
    }

    public override void SpawnPiece()
    {
        // Checks if a line needs to be disabled
        if(disableLineIn == nbPiecesBeforeDisabledLine) {
            disableLineIn = 1;
            nbPiecesBeforeDisabledLine = Random.Range(10, 16);
            DisableLine();
        }else {
            disableLineIn++;
        }
        disableLineTextUI.text = "Dans " + (nbPiecesBeforeDisabledLine - disableLineIn).ToString() + " pièce(s)";

        // Construction next tetramino
        TetraminoData data = dataNextMoves.Dequeue();
        int randomUVType = Random.Range(0, 6);
        SectorUVType type;
        
        // Randomly sets if the next tetramino is a UV tetramino or a normal tetramino
        if(randomUVType < 3) {
            typeActiveTetramino = uvTiles[randomUVType].type;
            // Set the tile used by the piece
            if(typeActiveTetramino == SectorUVType.TM) {
                data.tile = tileTM;
            } else {
                data.tile = uvTiles[randomUVType].tile;
            } 
        } else {
            typeActiveTetramino = SectorUVType.NULL;
        }
        // Generate the data of a futur move
        dataNextMoves.Enqueue(tetraminoes[Random.Range(0, tetraminoes.Length)]);
        // Update the display of the next move
        nextTetraminosDisplay.UpdateNextMoves(dataNextMoves);
        // initiats the next active piece
        activePiece.Initialize(this, spawnPosition, data, typeActiveTetramino != SectorUVType.NULL);

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
            if(typeActiveTetramino == SectorUVType.NULL) {
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
                SectorUVTetramino SectorUvTetramino = new SectorUVTetramino(typeActiveTetramino, uvsToComplete);
                SectorUVTile SectorUVTile;

                for (int i = 0; i < piece.cells.Length; i++)
                {
                    // Sets a tile for tetramino
                    Vector2Int tilePosition = piece.cells[i] + piece.position;
                    tilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), piece.tetraminoData.tile);
                    // Creats data for the tile of the UV tetramino
                    SectorUVTile = new SectorUVTile(SectorUvTetramino);
                    boardSectorUVTiles[tilePosition.y][tilePosition.x] = SectorUVTile;
                    SectorUvTetramino.AddSectorUVTile(SectorUVTile);
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
            if(boardSectorUVTiles[row][col] != null) {
                boardSectorUVTiles[row][col].DestroyTile();
                boardSectorUVTiles[row][col] = null;
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
                boardSectorUVTiles[row][col] =  boardSectorUVTiles[row+1][col];
            }

            row++;
        }
    }

    public override void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = nextDisabledLine;

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

    protected override void GameOver() {
        GameOverData.setLevel(Scene.Sector);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }

    // A line is disabled
    private void DisableLine() {
        RectInt bounds = Bounds;

        for(int row = bounds.yMax; row > nextDisabledLine; --row) {
            for(int col = bounds.xMin; col < bounds.xMax; ++col) {
                Vector3Int position = new Vector3Int(col, row-1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
                boardSectorUVTiles[row][col] =  boardSectorUVTiles[row-1][col];
            }
        }

        for(int col = bounds.xMin; col < bounds.xMax; ++col) {
            tilemap.SetTile(new Vector3Int(col, nextDisabledLine, 0), tileDisabledLine);
            boardSectorUVTiles[nextDisabledLine][col] = null;
        }

        nextDisabledLine++;
    }
}
