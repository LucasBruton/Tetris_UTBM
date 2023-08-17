using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Types of UV for Sector
public enum SectorUVType{
    NULL, TM, T2S, Language
}

// Structure that is used to correspond a SectorUVType to a tile
[System.Serializable]
public struct SectorUVTypeTile {
    public SectorUVType type;
    public Tile tile;
}

// Structure that is used to correspond a Sector to a tile
[System.Serializable]
public struct SectorTile {
    public Sector sector;
    public Tile tile;
}

// This class stores data for a UV tetramino
public class SectorUVTetramino
{
    // UVType of the tetramino
    public SectorUVType uvType {get; private set; }
    // List of the tiles of the tetramino
    private List<SectorUVTile> tiles = new List<SectorUVTile>();
    // Reference to the data of the UVs to complete to finish the level
    private SectorUVsToComplete uvsToComplete;

    public SectorUVTetramino(SectorUVType uvType, SectorUVsToComplete uvsToComplete) {
        this.uvType = uvType;
        this.uvsToComplete = uvsToComplete;
    }

    // Ads tile to list of the tiles of the tetramino
    public void AddSectorUVTile(SectorUVTile tile) {
        tiles.Add(tile);
    }


    // removes tiles of the list of tetramino
    public void RemoveSectorUVTile(SectorUVTile tile) {
        tiles.Remove(tile);
        
        // If the list is empty, then the uv is completed
        if(tiles.Count == 0) {
            uvsToComplete.CompleteUV(uvType);
        }
    }
}