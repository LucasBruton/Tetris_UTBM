using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Class representing a tile of a UV tetramino
public class TC2UVTile
{
    // UV tetramino that the tile is a part of
    private TC2UVTetramino tc2UvTetramino;

    public TC2UVTile(TC2UVTetramino tc2UvTetramino) 
    {
        this.tc2UvTetramino = tc2UvTetramino;
    }

    // Tells the UV tetramino that the tile has been destroyed
    public void DestroyTile()
    {
        tc2UvTetramino.RemoveTC2UVTile(this);
    }
    
}