using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveTetramino : MonoBehaviour
{
    public TetraminosBoard tetraminosBoard {get; private set;}
    public TetraminoData tetraminoData {get; private set;}
    public Vector2Int[] cells {get; private set;}
    public Vector2Int[] rotation {get; private set;}
    public Vector2Int[] horizontal {get; private set;}
    public Vector2Int[] vertical {get; private set;}
    public Vector2Int position {get; private set;}

    private Vector2 saveMovement;
    private Vector2Int[] saveCells;

    private bool oneMove;
    private bool oneRotation;
    private bool downPressed;
    private bool one;
    private bool step = false;
    private int rotationIndex = 0;

    protected float stepDelay = 1f;
    private float moveDelay = 0f;
    private float lockDelay = 0.1f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    // To initialize each time our active tetramino with different data
    public void Initialize(TetraminosBoard tetraminosBoard, Vector2Int position, TetraminoData tetraminoData)
    {
        this.tetraminoData = tetraminoData;
        this.tetraminosBoard = tetraminosBoard;
        this.position = position;

        oneMove = true;
        oneRotation = true;
        downPressed = false;
        one = false;

        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;

        if (cells == null) {
            cells = new Vector2Int[tetraminoData.cells.Length];
            saveCells = new Vector2Int[tetraminoData.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector2Int)tetraminoData.cells[i];
            //Debug.Log(cells[i]);
        }
    }

    void FixedUpdate()
    {
        lockTime += Time.deltaTime;

        // If key down is pushed a long time, the piece continue to come down 
        if (Time.time > moveTime && downPressed) 
        {   
            //Debug.Log("key pressed");
            Move();
        }

        // Advance the piece to the next row every x seconds
        if (Time.time > stepTime) {
            Step();
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        // Debug.Log(context.phase);
        // Active the piece only when the key is pressed
        if(context.phase.ToString() == "Canceled")
        {
            downPressed = false;
        }
        else
        {
            downPressed = true;
        }

        saveMovement = context.ReadValue<Vector2>();
    }

    public bool Move()
    {
        //Debug.Log("I'M MOVING");
        if(oneMove)
        {
            oneMove = false;
            bool tileBelow = true;

            // Control the speed of the active piece
            StartCoroutine(Wait(0.07f));

            this.tetraminosBoard.SetOrClear(this, false);

            Vector2Int newPosition = this.position;

            if(step)
            {
                newPosition.y += -1;
            }
            else
            {
                newPosition.x += (int) saveMovement.x;
                newPosition.y += (int) saveMovement.y;
            }

            step = false;

            bool valid = tetraminosBoard.IsValidPosition(this, newPosition);

            // Only save the movement if the new position is valid
            if (valid)
            {
                position = newPosition;
                moveTime = Time.time + moveDelay;

                tileBelow = tetraminosBoard.IsTileBelow(this, newPosition);

                if(!tileBelow)
                {
                    lockTime = 0f;
                }
            }
            this.tetraminosBoard.SetOrClear(this, true);

            if(tileBelow)
            {
                return false;
            }
            
        }

        return true;
    }

    // Instantly drop the active tetramino to the lowest position possible 
    public void HardDrop(InputAction.CallbackContext context)
    {
        // Drop the piece only when the input starts
        if(context.phase.ToString() == "Started") {
            Vector2Int down = position;

            this.tetraminosBoard.SetOrClear(this, false);
            // Calculate the lowest position possible
            while(this.tetraminosBoard.IsValidPosition(this, down)) {
                down.y += -1;
            }
            down.y += 1;
            // Move the piece down
            position = down;
            moveTime = Time.time + moveDelay;
            lockTime = 0f;
            this.Lock();
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        bool rotate;

        // Rotate only when the key is pressed (not release)
        if(context.phase.ToString() == "Canceled")
        {
            rotate = false;
        }
        else
        {
            rotate = true;
        }

        if(oneRotation && rotate)
        {
            oneRotation = false;
            // Save the cells
            for (int i = 0; i < cells.Length; i++) 
            {
                saveCells[i] = cells[i];
            }

            StartCoroutine(Wait(0.09f));
            rotationIndex++;
            if(rotationIndex > 3)
            {
                rotationIndex = 0;
            }
            //Debug.Log(rotationIndex);
            this.tetraminosBoard.SetOrClear(this, false);

            // Rotate all of the cells using the rotation matrix
            for (int i = 0; i < cells.Length; i++)
            {
                switch (tetraminoData.tetramino)
                {
                    case Tetramino.I:
                    case Tetramino.O:
                        if(rotationIndex %2 == 1)
                        {
                            cells[i] = tetraminoData.rotation[i];
                        }
                        else
                        {
                            cells[i] = (Vector2Int)tetraminoData.cells[i];
                        }
                        break;
                    case Tetramino.T:
                    case Tetramino.L:
                    case Tetramino.J:
                        int gap = 0;
                        if(tetraminoData.tetramino == Tetramino.T)
                        {
                            gap = 1;
                        }

                        if(rotationIndex %2 == 1)
                        {
                            cells[i] = tetraminoData.vertical[i] - new Vector2Int(gap, 0);
                        }
                        else
                        {
                            cells[i] = tetraminoData.horizontal[i] - new Vector2Int(gap, 0);
                        }

                        if(i == 3)
                        {
                            cells[i] = tetraminoData.rotation[rotationIndex];
                        }
                        break;
                    // S and Z
                    default:
                        if(rotationIndex %2 == 1)
                        {
                            cells[i].x = tetraminoData.rotation[i].x - (int)(Mathf.Floor(rotationIndex/2));
                            cells[i].y = tetraminoData.rotation[i].y;
                        }
                        else
                        {
                            cells[i].x = tetraminoData.cells[i].x;
                            cells[i].y = tetraminoData.cells[i].y - (rotationIndex/2);
                        }
                        break;
                }
            }

            //Debug.Log(tetraminosBoard.IsValidPosition(this, this.position));
            if(!tetraminosBoard.IsValidPosition(this, this.position))
            {
                for (int i = 0; i < cells.Length; i++) 
                {
                    cells[i] = saveCells[i];
                }
            }
        }

        this.tetraminosBoard.SetOrClear(this, true);

    }


    private void Step()
    {
        stepTime = Time.time + stepDelay;

        // Step down to the next row
        step = true;

        bool tileBelow = tetraminosBoard.IsTileBelow(this, this.position);

        Move();

        /*if(tileBelowEverywhere && one)
        {
            Debug.Log("COUCOU");
            lockTime = 0;
            one = true;
        }*/

        // Once the piece has been inactive for too long it becomes locked
        if (lockTime >= lockDelay && tileBelow)
        {
            Lock();
        }
    }

    private void Lock()
    {
        tetraminosBoard.SetOrClear(this, true);
        tetraminosBoard.ClearLines();
        tetraminosBoard.SpawnPiece();
    }

    private IEnumerator Wait(float seconde)
    {
        yield return new WaitForSeconds(seconde);
        oneRotation = true;
        oneMove = true;
    }
        

}
