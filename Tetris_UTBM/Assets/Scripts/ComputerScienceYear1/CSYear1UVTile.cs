using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Class representing a tile of an UV tetramino
public class CSYear1UVTile
{
    // UV tetramino that the tile is a part of
    private CSYear1UVTetramino CSYear1UvTetramino;

    public CSYear1UVTile(CSYear1UVTetramino CSYear1UvTetramino) 
    {
        this.CSYear1UvTetramino = CSYear1UvTetramino;
    }

    // Tells the UV tetramino that the tile has been destroyed
    public void DestroyTile()
    {
        CSYear1UvTetramino.RemoveCSYear1UVTile(this);
    }
    
}