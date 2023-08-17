using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Types of UV for CSYear1
public enum CSYear1UVType{
    NULL, TheoreticalComputerScience, AlgoAndPrograming, Network, Database, HardwareAndSystems, T2S, Language, CS
}

// Structure that is used to correspond a CSYear1UVType to a tile
[System.Serializable]
public struct CSYear1UVTypeTile {
    public CSYear1UVType type;
    public Tile tile;
}

// This class stores data for a UV tetramino
public class CSYear1UVTetramino
{
    // UVType of the tetramino
    public CSYear1UVType uvType {get; private set; }
    // List of the tiles of the tetramino
    private List<CSYear1UVTile> tiles = new List<CSYear1UVTile>();
    // Reference to the data of the UVs to complete to finish the level
    private CSYear1UVsToComplete uvsToComplete;

    public CSYear1UVTetramino(CSYear1UVType uvType, CSYear1UVsToComplete uvsToComplete) {
        this.uvType = uvType;
        this.uvsToComplete = uvsToComplete;
    }

    // Ads tile to list of the tiles of the tetramino
    public void AddCSYear1UVTile(CSYear1UVTile tile) {
        tiles.Add(tile);
    }


    // removes tiles of the list of tetramino
    public void RemoveCSYear1UVTile(CSYear1UVTile tile) {
        tiles.Remove(tile);
        
        // If the list is empty, then the uv is completed
        if(tiles.Count == 0) {
            uvsToComplete.CompleteUV(uvType);
        }
    }
}