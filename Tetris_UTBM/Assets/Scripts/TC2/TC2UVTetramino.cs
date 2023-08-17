using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Types of UV for TC2
public enum TC2UVType{
    NULL, CS, T2S, Language
}

// Structure that is used to correspond a TC2UVType to a tile
[System.Serializable]
public struct TC2UVTypeTile {
    public TC2UVType type;
    public Tile tile;
}

// This class stores data for a UV tetramino
public class TC2UVTetramino
{
    // UVType of the tetramino
    public TC2UVType uvType {get; private set; }
    // List of the tiles of the tetramino
    private List<TC2UVTile> tiles = new List<TC2UVTile>();
    // Reference to the data of the UVs to complete to finish the level
    private TC2UVsToComplete uvsToComplete;

    public TC2UVTetramino(TC2UVType uvType, TC2UVsToComplete uvsToComplete) {
        this.uvType = uvType;
        this.uvsToComplete = uvsToComplete;
    }

    // Ads tile to list of the tiles of the tetramino
    public void AddTC2UVTile(TC2UVTile tile) {
        tiles.Add(tile);
    }


    // removes tiles of the list of tetramino
    public void RemoveTC2UVTile(TC2UVTile tile) {
        tiles.Remove(tile);
        
        // If the list is empty, then the uv is completed
        if(tiles.Count == 0) {
            uvsToComplete.CompleteUV(uvType);
        }
    }
}