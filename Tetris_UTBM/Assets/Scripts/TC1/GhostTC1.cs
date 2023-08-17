using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// Shows a ghost version of the current active tetramino
public class GhostTC1 : MonoBehaviour
{

    public Tile tile;
    public TetraminosBoardTC1 board;
    public ActiveTetraminoTC1 trackingTetramino;

    public Tilemap tilemap {get; private set;}
    private List<Vector3Int> pos;
    private List<Tile> oldTile;
    /*public Vector2Int[] cells {get; private set;}
    public Vector2Int position {get; private set;}*/

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.pos = new List<Vector3Int>(trackingTetramino.pos);
        this.oldTile = new List<Tile>(trackingTetramino.activePiece);
    }

    private void LateUpdate() 
    {
        SetOrClear(false);
        Copy();
        Drop();
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

        for (int i = 0; i < this.pos.Count; i++)
        {
            //Vector2Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(new Vector3Int(pos[i].x, pos[i].y, 0), tile);
        }
    }

    private void Drop()
    {
        List<Vector3Int> oldPiece = new List<Vector3Int>(trackingTetramino.pos);
        int currentRow = Bottom(pos);
        int bottomRow = -this.board.boardSize.y / 2 - 1;

        this.board.SetOrClear(this.trackingTetramino.tilemap, oldTile, this.trackingTetramino.pos, false);
        
        for(int row = currentRow; row >= bottomRow; row--)
        {
            for(int i = 0; i < oldPiece.Count; ++i)
            {
                oldPiece[i] = new Vector3Int(oldPiece[i].x, oldPiece[i].y-1, 0);
            }

            if(this.board.IsValidPosition(oldPiece))
            {
                this.pos = new List<Vector3Int>(oldPiece);
            } 
            else
            {
                break;
            }
        }

        this.board.SetOrClear(this.trackingTetramino.tilemap, oldTile, this.trackingTetramino.pos, true);
    }

    private void Copy()
    {
        this.pos = new List<Vector3Int>(this.trackingTetramino.pos);
        this.oldTile = new List<Tile>(this.trackingTetramino.activePiece);
        /*
        for(int i = 0; i < this.pos.Count; ++i) {
            this.pos[i] = this.trackingTetramino.pos[i];
            this.oldTile[i] =  this.trackingTetramino.activePiece[i];
        }*/
    }

    private int Bottom(List<Vector3Int> pos)
    {
        int y = 100;

        for(int i = 0; i < this.pos.Count; ++i)
        {
            if(this.pos[i].y < y)
            {
                y = this.pos[i].y;
            }
        }

        return y;
    }
}
