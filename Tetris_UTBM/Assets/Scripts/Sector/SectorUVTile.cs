using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Class representing a tile of a UV tetramino
public class SectorUVTile
{
    // UV tetramino that the tile is a part of
    private SectorUVTetramino SectorUvTetramino;

    public SectorUVTile(SectorUVTetramino SectorUvTetramino) 
    {
        this.SectorUvTetramino = SectorUvTetramino;
    }

    // Tells the UV tetramino that the tile has been destroyed
    public void DestroyTile()
    {
        SectorUvTetramino.RemoveSectorUVTile(this);
    }
    
}