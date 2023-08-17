using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum Tetramino
{
    I, J, L, O, T, S, Z
}

[System.Serializable]
public struct TetraminoData
{
    public Tile tile;
    public Tetramino tetramino;
    public Vector2Int[] cells {get; private set;}
    public Vector2Int[] rotation {get; private set;}
    public Vector2Int[] horizontal {get; private set;}
    public Vector2Int[] vertical {get; private set;}

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetramino];
        this.rotation = Data.Rotation[this.tetramino];
        switch(tetramino)
        {
            case Tetramino.J:
            case Tetramino.L:
            case Tetramino.T:
                this.horizontal = Data.Horizontal;
                this.vertical = Data.Vertical;
                break;
        }
    }
}
