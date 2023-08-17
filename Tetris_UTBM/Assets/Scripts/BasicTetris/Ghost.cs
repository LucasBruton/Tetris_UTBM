using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// Shows a ghost version of the current active tetramino
public class Ghost : MonoBehaviour
{
    // Tile used to display the ghost piece
    public Tile tile;
    // Board where the ghost appeares
    public TetraminosBoard board;
    // The activie piece of the ghost
    public ActiveTetramino trackingTetramino;
    // Tilemap used by the ghost
    public Tilemap tilemap {get; private set;}
    // Cells used by the ghost piece
    public Vector2Int[] cells {get; private set;}
    // Position of the ghost piece
    public Vector2Int position {get; private set;}

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector2Int[Data.Cells[this.trackingTetramino.tetraminoData.tetramino].Length];
    }

    private void LateUpdate() 
    {
        // Clear the previous ghost piece
        SetOrClear(false);
        // Copy the position of the active tetramino
        Copy();
        // Drop the posistion to the lowest possible point
        Drop();
        // Set a new ghost piece at the previous calculated position
        SetOrClear(true);
    }

    private void SetOrClear(bool set)
    {
        Tile tile;
        // Set the tile
        if(set)
        {
            tile = this.tile;
        }
        // Clear the tile
        else
        {
            tile = null;
        }

        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector2Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), tile);
        }
    }

    private void Drop()
    {
        Vector2Int position = this.trackingTetramino.position;
        
        int currentRow = position.y;
        int bottomRow = -this.board.boardSize.y / 2 - 1;

        this.board.Clear(this.trackingTetramino);
        
        for(int row = currentRow; row >= bottomRow; row--)
        {
            position.y = row;
            if(this.board.IsValidPosition(this.trackingTetramino, position)) {
                this.position = position;
            } else {
                break;
            }
        }

        this.board.SetOrClear(this.trackingTetramino, true);
    }

    private void Copy()
    {
        for(int i = 0; i < this.cells.Length; ++i) {
            this.cells[i] = this.trackingTetramino.cells[i];
        }
    }
}
