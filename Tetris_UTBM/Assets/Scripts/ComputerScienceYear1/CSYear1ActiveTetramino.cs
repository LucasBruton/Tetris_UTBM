using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ActiveTetramino for the CSYear1 level (inherits from ActiveTetramino)
public class CSYear1ActiveTetramino : ActiveTetramino
{
    // True if the tetramino is a UVTetramino
    private bool UVTetramino;

    // To initialize each time our active tetramino with different data
    public void Initialize(TetraminosBoard tetraminosBoard, Vector2Int position, TetraminoData tetraminoData, bool UVTetramino, bool fasterTetramino)
    {
        base.Initialize(tetraminosBoard, position, tetraminoData);
        this.UVTetramino = UVTetramino;
        
        // If true, the tetramino goes faster
        if(fasterTetramino) {
            stepDelay = 0.25f;
        } else {
            stepDelay = 1.0f;
        }
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
