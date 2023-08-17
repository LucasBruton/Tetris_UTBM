using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class ActiveTetraminoTC1 : MonoBehaviour
{
    private int pieceNumber;

    public Tilemap activePieceTilemap;
    public Tilemap tilemap;

    public List<Tile> activePiece = new List<Tile>();
    public List<Vector3Int> pos = new List<Vector3Int>();
    public List<int> id = new List<int>();
    public TetraminosBoardTC1 tetraminosBoard;
    /*public TetraminoData tetraminoData {get; private set;}
    public Vector2Int[] cells {get; private set;}
    public Vector2Int position {get; private set;}*/

    // Rotation
    private float cos;
    private float sin;
    private float[] matrix;

    private Vector2 saveMovement;
    private Vector2Int[] saveCells;

    private bool oneMove;
    public bool oneRotation;
    private bool downPressed;
    private bool one;
    private bool step = false;
    public int rotationIndex;
    private bool down;

    private float stepDelay = 1f;
    private float moveDelay = 0f;
    private float lockDelay = 0f;
    private float overDelay = 1f;

    private float stepTime;
    private float moveTime;
    private float lockTime;
    public float overTime;

    public string nextScene;

    private List<Vector3Int> allTiles = new List<Vector3Int>(); 

    private List<Vector3Int> allPurple = new List<Vector3Int>(); 
    private List<Vector3Int> allRed = new List<Vector3Int>(); 
    private List<Vector3Int> allGreen = new List<Vector3Int>();

    private Vector2Int boardSize = new Vector2Int(10, 20);
    private RectInt Bounds 
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    
    void Start()
    {
        int i = 0;
        RectInt boundsBoard = Bounds;
        NameNextScene();
        //from the bottom to the top
        for(int row = boundsBoard.yMin; row < boundsBoard.yMax; row++)
        {
            //from the left to the right
            for (int col = boundsBoard.xMin; col < boundsBoard.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);

                if(activePieceTilemap.HasTile(position))
                {
                    Tile tile = activePieceTilemap.GetTile<Tile>(position); 
                    activePieceTilemap.SetTile(position, null);
                    // To draw it on the tilemap
                    tilemap.SetTile(position, tile);
                    // Retrieve each tiles that compose the Active Piece
                    activePiece.Add(tile);
                    pos.Add(position);
                    id.Add(i);
                    i++;            
                }

                if(tilemap.HasTile(position))
                {
                    string tile = tilemap.GetTile(position).name;

                    allTiles.Add(position);

                    switch(tile)
                    {
                        case "Purple": case "Purple_LG": case "Purple_LG_special":
                            allPurple.Add(position);
                            break;

                        case "Red": case "Red_TM": case "Red_TM_special":
                            allRed.Add(position);
                            break;

                        case "Green": case "Green_CS": case "Green_CS_special":
                            allGreen.Add(position);
                            break;
                    }
                }
            }
        }

        /*Debug.Log("ACTIVE PIECE");
        Debug.Log(activePieceTilemap.origin);*/
        oneMove = true;
        oneRotation = true;
        downPressed = false;
        one = false;
        down = false;

        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;
        overTime = 0f;

        rotationIndex = 0;

        pieceNumber = 4;

        cos = Mathf.Cos(Mathf.PI / 2f);
        sin = Mathf.Sin(Mathf.PI / 2f);
        matrix = new float[] { cos, sin, -sin, cos };
    
    }  

    void FixedUpdate()
    {
        lockTime += Time.deltaTime;

        overTime += Time.deltaTime;

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

        if(down)
        {
            down = false;
            if(this.tetraminosBoard.Down(pos, overTime))
            {
                overTime = 0;
                List<Vector3Int> tileBelow = tetraminosBoard.IsTileBelow(pos);
                Lock(tileBelow);
            }
        }

        /*Debug.Log("JE VAIS AVOIR CE PUTAIN DE GAME OVER");
        Debug.Log(overTime);*/
        if(overTime > overDelay)
        {
            if(pos.Count == 0)
            {

                if(this.tetraminosBoard.IsLevelFinished())
                {
                    Debug.Log("GAGNEEEEEEEEEER");
                    SceneManager.LoadScene(nextScene);
                }
                else
                {
                    this.tetraminosBoard.GameOver();
                }
            }
        }
    }

    public virtual void NameNextScene()
    {
        nextScene = "TC1 - Niveau 2";
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

    public void Move()
    {
        //Debug.Log("I'M MOVING");
        if(oneMove)
        {
            oneMove = false;
            List<Vector3Int> tileBelow = new List<Vector3Int>();

            // Control the speed of the active piece
            StartCoroutine(Wait(0.07f));

            //Clear
            this.tetraminosBoard.SetOrClear(tilemap, activePiece, pos, false);

            List<Vector3Int> newPosition = new List<Vector3Int>();
            List<Vector3Int> savePosition = new List<Vector3Int>();

            // We move each tiles and not it's position;
            for(int i = 0; i < activePiece.Count; i++)
            {
                newPosition.Add(pos[i]);
                savePosition.Add(pos[i]);

                if(step)
                {
                    newPosition[i] = new Vector3Int(savePosition[i].x, savePosition[i].y - 1, 0);
                }
                else
                {
                    newPosition[i] = new Vector3Int(savePosition[i].x + (int)saveMovement.x, savePosition[i].y+(int) saveMovement.y, 0);
                }
            
            }
            
            step = false;

            bool valid = tetraminosBoard.IsValidPosition(newPosition);
            // Only save the movement if the new position is valid

            if (valid)
            {
                if(pos.Count != 0)
                    overTime = 0;
                pos = newPosition;
                moveTime = Time.time + moveDelay;

                tileBelow = tetraminosBoard.IsTileBelow(newPosition);

                if(tileBelow == null)
                {
                    lockTime = 0f;
                }
            }

            /*if(tileBelow != null)
            {
                return false;
            }*/

            this.tetraminosBoard.SetOrClear(tilemap, activePiece, pos, true);

        }

        //return true;
    }

    // Instantly drop the active tetramino to the lowest position possible 
    /*public void HardDrop(InputAction.CallbackContext context)
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
    }*/

    public virtual void Rotate(InputAction.CallbackContext context)
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

        if(oneRotation && rotate && pos.Count > 1)
        {
            List<Vector3Int> newPos = new List<Vector3Int>();

            oneRotation = false;

            int oldX, oldY;

            rotationIndex++;
            if(rotationIndex > 3)
            {
                rotationIndex = 0;
            }

            StartCoroutine(Wait(0.09f));
            //Debug.Log(rotationIndex);
            this.tetraminosBoard.SetOrClear(tilemap, activePiece, pos, false);

            int x = -10;
            int y = -10;
            int comx = -10;
            int comy = -10;
            
            /*Debug.Log("ROTATION INDEX");
            Debug.Log(rotationIndex);*/

            int diff = 0;

            /*if(pos.Count > 2)
            {
                diff = pieceNumber - pos.Count;
            }
            else
            {
                diff = 1;
            }*/

            switch (rotationIndex)
            {
                case 0:
                    x = -1;
                    y = 2 - diff;
                    comx = 1;
                    comy = -1;
                    break;
                case 1:
                    x = 1;
                    y = 1;
                    comx = -1;
                    comy = -1;
                    break;
                case 2:
                    x = 2 - diff;
                    y = -1;
                    comx = -1;
                    comy = 1;
                    break;
                case 3:
                    x = -2 + diff;
                    y = -2 + diff;
                    comx = 1;
                    comy = 1;
                    break;
            }

            oldX = x;
            oldY = y;

            for(int i = 0; i < pos.Count; i++)
            {
                x= oldX + comx * id[i];
                y= oldY + comy * id[i];
                newPos.Add(new Vector3Int(pos[i].x+x, pos[i].y+y, 0));
                Debug.Log("ID ATTENDUS");
                Debug.Log(id[i]);
                Debug.Log("RESULTATS ATTENDUS");
                Debug.Log(x + " + " + y);
            }
            

            //Debug.Log(tetraminosBoard.IsValidPosition(this, this.position));
            if(tetraminosBoard.IsValidPosition(newPos))
            {
                for (int j = 0; j < pos.Count; j++) 
                {
                    pos[j] = newPos[j];
                    Debug.Log(pos[j]);
                }
                if(pos.Count != 0)
                    overTime = 0;
            }
            else
            {
                rotationIndex--;
            }
        }

        this.tetraminosBoard.SetOrClear(tilemap, activePiece, pos, true);



    }

    private void Step()
    {
        //Debug.Log("STEP");

        stepTime = Time.time + stepDelay;

        // Step down to the next row
        step = true;

        List<Vector3Int> tileBelow = tetraminosBoard.IsTileBelow(pos);

        List<Vector3Int> suite = new List<Vector3Int>();

        Move();

        if(pos.Count != 0)
            overTime = 0;

        // Once the piece has been inactive for too long it becomes locked
        if (lockTime >= lockDelay && (tileBelow != null))
        {
            Debug.Log("LOCK AFTER STEP");
            Lock(tileBelow);
        }
    }

    private void Lock(List<Vector3Int> tileBelow)
    {
        Debug.Log("LOCK");

        List<Vector3Int> copyTile = new List<Vector3Int>(pos);

        if(copyTile.Count >= 1 && tileBelow != null)
        {
            foreach(Vector3Int item in pos)
            {
                Debug.Log("POSITIIIIIIION");
                Debug.Log(item);
            }

            Debug.Log("JE RENTRE DANS LE IF");
            for(int i = 0; i < copyTile.Count; i++)
            {
                for(int j = 0; j < tileBelow.Count; j++)
                {
                    //tile le bas possible
                    if(copyTile[i] == tileBelow[j])
                    {
                        Debug.Log("TILE BELOOOOOOOOOOOOOOOOOOOOOOOOOOW");
                        Debug.Log(tileBelow.Count);
                        for(int k = 1; k < copyTile.Count; k++)
                        {
                            Vector3Int tile = new Vector3Int(tileBelow[j].x, tileBelow[j].y + k, 0);
                            if(tilemap.HasTile(tile))
                            {
                                /*Debug.Log("JE REMOVE QUELLE TILE ?");
                                Debug.Log(pos.IndexOf(tile));*/
                                id.Remove(pos.IndexOf(tile));
                                Debug.Log("IL Y A UNE TILE AU DESSUS");
                                Debug.Log(pos.IndexOf(tile));
                                pos.Remove(tile);
                                activePiece.Remove(tilemap.GetTile<Tile>(tile));
                            }
                        }
                        Debug.Log("JE REMOVE QUELLE TILE PAS AU DESSUS ?");
                        Debug.Log(copyTile.IndexOf(tileBelow[j]));
                        id.Remove(copyTile.IndexOf(tileBelow[j]));
                        pos.Remove(tileBelow[j]);
                        activePiece.Remove(tilemap.GetTile<Tile>(tileBelow[j]));
                    }
                }
            }
        }

        bool valid = this.tetraminosBoard.ClearLines(activePiece, pos, id, allTiles);

        if(valid)
        {
            Debug.Log("IT'S VALIIIIIIIIIIIIIIIIIIIIIIIID");
            //stepDelay = 0.5f;
            overTime = 0;
            StartCoroutine(WaitDown());
        }
        
    }

    protected IEnumerator Wait(float seconde)
    {
        yield return new WaitForSeconds(seconde);
        oneRotation = true;
        oneMove = true;
    }

    protected IEnumerator WaitDown()
    {
        overTime = 0;
        yield return new WaitForSeconds(0.6f);
        overTime = 0;
        down = true;
    }
        

}
