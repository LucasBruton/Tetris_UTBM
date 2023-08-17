using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ActiveTetramino for the Sector level (inherits from ActiveTetramino)
public class SectorActiveTetramino : ActiveTetramino
{
    // True if the tetramino is a UVTetramino
    private bool UVTetramino;

    // To initialize each time our active tetramino with different data
    public void Initialize(TetraminosBoard tetraminosBoard, Vector2Int position, TetraminoData tetraminoData, bool UVTetramino)
    {
        base.Initialize(tetraminosBoard, position, tetraminoData);
        this.UVTetramino = UVTetramino;
    }


    // Destroys active tetramino if it is a UVTetramino
    public void DestroyUVTetramino(InputAction.CallbackContext context)
    {
        // Drop the piece only when the input starts
        if(UVTetramino && context.phase.ToString() == "Started") {
            this.tetraminosBoard.SetOrClear(this, false);
            this.tetraminosBoard.SpawnPiece();
        }
    }
}
