using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TetraminosBoardTC1 : MonoBehaviour
{
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Tilemap tilemap;
    public Tilemap borderTilemap;
    public Tilemap activePiece { get; protected set; }
    public Vector2Int spawnPosition = new Vector2Int(-1, 8);

    private Vector3Int purpleSpecial;
    private Vector3Int redSpecial;
    private Vector3Int greenSpecial;
    private RectInt bounds;

    private bool clear;
    private bool finish;
    private bool wait;

    public RectInt Bounds 
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        bounds = Bounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        purpleSpecial = new Vector3Int(-1, -1, -1);
        redSpecial = new Vector3Int(-1, -1, -1);
        greenSpecial = new Vector3Int(-1, -1, -1);

    }

    void Update()
    {

    }

    public virtual void SetOrClear(Tilemap tilemap, List<Tile> piece, List<Vector3Int> pos, bool set)
    {
        // Set the tile
        if(set)
        {
            for(int i=0; i < piece.Count; i++)
            {
                tilemap.SetTile(pos[i], piece[i]);
            }
        }
        // Clear the tile
        else
        {
            for(int i=0; i < piece.Count; i++)
            {
                tilemap.SetTile(pos[i], null);
            }
        }

    }

    public bool IsValidPosition(List<Vector3Int> newPos)
    {
        // The position is only valid if every cell is valid
        for (int i = 0; i < newPos.Count; i++)
        {

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(newPos[i])) 
            {
                return false;
            }

            // The piece go to the border of the board
            if (borderTilemap.HasTile(newPos[i]))
            {
                return false;
            }

        }

        return true;
    }

    public List<Vector3Int> IsTileBelow(List<Vector3Int> pos)
    {
        List<Vector3Int> saveTiles = new List<Vector3Int>();

        for (int i = 0; i < pos.Count; i++)
        {
            Vector3Int tile = new Vector3Int(pos[i].x, pos[i].y-1, 0);

            bool below = false;

            // Save only the cells that can have a tile below
            for(int j = 0; j < pos.Count; j++)
            {

                if(i != j)
                {
                    if(tile.x == pos[j].x && tile.y == pos[j].y)
                    {
                        below = true;
                    }

                }
            }
            
            if(!below)
                saveTiles.Add(pos[i]);
        }

        /*Debug.Log("SAVE TILES");
        Debug.Log(saveTiles.Count);*/

        // Research if there is a tile below each save cells

        bool belowTile = false;

        List<Vector3Int> copySave = new List<Vector3Int>();

        for(int i = 0; i < saveTiles.Count; i++)
        {
            copySave.Add(saveTiles[i]);
        }

        for(int i = 0; i < copySave.Count; i++)
        {
            Vector3Int tile = new Vector3Int(copySave[i].x, copySave[i].y-1, 0);
            //if there is no tile below
            if(!tilemap.HasTile(new Vector3Int (tile.x, tile.y, 0)) && !borderTilemap.HasTile(new Vector3Int (tile.x, tile.y, 0))) 
            {
                saveTiles.Remove(copySave[i]);
            }
            else
            {
                belowTile = true;
            }

        }

        /*Debug.Log("NOMBRE DE TILES");
        Debug.Log(saveTiles.Count);*/

        // if there is au moins one tile below
        if(belowTile)
        {
            return saveTiles;
        }
        else
        {
            return null;
        }
    
    }
    

    public bool ClearLines(List<Tile> piece, List<Vector3Int> pos, List<int> id, List<Vector3Int> allTiles)
    {
        List<Vector3Int> purple = new List<Vector3Int>(); 
        List<Vector3Int> red = new List<Vector3Int>(); 
        List<Vector3Int> green = new List<Vector3Int>();

        //Debug.Log("CLEAR LINES");

        rowTiles(purple, red, green);
        colTiles(purple, red, green);

        clear = false;

        // PURPLE
        for(int i = 0; i < purple.Count; i++)
        {
            for(int j = 0; j < pos.Count; j++)
            {
                if(purple[i] == pos[j])
                {
                    pos.RemoveAt(j);
                    piece.RemoveAt(j);
                    id.RemoveAt(j);
                }
            }

            if(purple[i] == purpleSpecial)
            {
                Debug.Log("LE SPECIAL PURPLE VA SE FAIRE DEGOMMER");
                purpleSpecial = Vector3Int.one;
            }

            StartCoroutine(animateTiles(purple[i], allTiles));
        }

        // RED
        for(int i = 0; i < red.Count; i++)
        {
            for(int j = 0; j < pos.Count; j++)
            {
                if(red[i] == pos[j])
                {
                    pos.RemoveAt(j);
                    piece.RemoveAt(j);
                    id.RemoveAt(j);
                }
            }

            if(red[i] == redSpecial)
            {
                redSpecial = Vector3Int.one;
            }

            StartCoroutine(animateTiles(red[i], allTiles));
        }

        // GREEN
        for(int i = 0; i < green.Count; i++)
        {
            for(int j = 0; j < pos.Count; j++)
            {
                if(green[i] == pos[j])
                {
                    pos.RemoveAt(j);
                    piece.RemoveAt(j);
                    id.RemoveAt(j);
                }
            }

            /*for(int j = 0; j < allGreen.Count; j++)
            {
                if(green[i] == allGreen[j])
                    allGreen.RemoveAt(j);
            }*/

            if(green[i] == greenSpecial)
            {
                greenSpecial = Vector3Int.one;
            }

            StartCoroutine(animateTiles(green[i], allTiles));
        }

        if(purpleSpecial == Vector3Int.one)
        {
            List<Vector3Int>allPurple = AllTiles("purple");
            Debug.Log("LE NOMBRE DE VIOLET DANS ALL PURPLE");
            Debug.Log(allPurple.Count);

            foreach(Vector3Int item in allPurple)
            {
                Debug.Log("POSITION DU VIOLET DE MERDE");
                Debug.Log(item);
                StartCoroutine(animateTiles(item, allTiles));
            }

            purpleSpecial = Vector3Int.zero;
        }

        if(redSpecial == Vector3Int.one)
        {
            List<Vector3Int>allRed = AllTiles("red");
            Debug.Log("LE NOMBRE DE ROUGE DANS ALL RED");
            Debug.Log(allRed.Count);

            foreach(Vector3Int item in allRed)
            {
                StartCoroutine(animateTiles(item, allTiles));
            }

            redSpecial = Vector3Int.zero;
        }

        if(greenSpecial == Vector3Int.one)
        {
            List<Vector3Int>allGreen = AllTiles("green");

            foreach(Vector3Int item in allGreen)
            {
                StartCoroutine(animateTiles(item, allTiles));
            }

            greenSpecial = Vector3Int.zero;
        }

        wait = false;

        if(clear)
        {
            Debug.Log("IT'S TRUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUE");
            return true;    
        }
        
        return false;
        
    
    }

    public void rowTiles(List<Vector3Int> purple, List<Vector3Int> red, List<Vector3Int> green)
    {
        //Debug.Log("ROW TILES");
        int purpleSuite;
        int redSuite;
        int greenSuite;

        //Go from bottom to up, to retrieve the line
        for(int row = bounds.yMin; row < bounds.yMax; row++)
        {
            //Debug.Log("ROW");
            // initialize at each beginning of a row
            purpleSuite = 0;
            redSuite = 0;
            greenSuite = 0;

            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                //Debug.Log("COL");
                Vector3Int position = new Vector3Int(col, row, 0);

                if(tilemap.HasTile(position))
                {
                    string tile = tilemap.GetTile(position).name;

                    Vector3Int tileAfterPosition;
                    string tileAfter = null;

                    if(col != bounds.xMax)
                    {   
                        tileAfterPosition = new Vector3Int(col+1, row, 0);
                        if(tilemap.HasTile(tileAfterPosition))
                        {
                            tileAfter = tilemap.GetTile(tileAfterPosition).name;
                        }
                    }

                    switch(tile)
                    {
                        case "Purple": case "Purple_LG": case "Purple_LG_special":
                            //Debug.Log(position);
                            // if it's the special tile
                            if(tile == "Purple_LG_special")
                            {
                                purpleSpecial = position;
                            }

                            purpleSuite++;
                            purple.Add(position);

                            //if the tile after is not red
                            if(tileAfter != "Purple" && tileAfter != "Purple_LG" && tileAfter != "Purple_LG_special")
                            {
                                //Debug.Log("REMOVE FUNCTION");
                                // Need to be 3 tiles at the minimum to clear
                                if(purpleSuite < 3)
                                {
                                    /*Debug.Log("REMOVE");*/
                                    purple.RemoveRange(purple.Count-purpleSuite, purpleSuite);
                                }

                                purpleSuite = 0;
                            }
                                
                            break;

                        case "Red": case "Red_TM": case "Red_TM_special":
                            //Debug.Log(position);
                            // if it's the special tile
                            if(tile == "Red_TM_special")
                            {
                                redSpecial = position;
                            }

                            redSuite++;
                            red.Add(position);

                            //if the tile after is not red
                            if(tileAfter != "Red" && tileAfter != "Red_TM" && tileAfter != "Red_TM_special")
                            {
                                //Debug.Log("REMOVE FUNCTION");
                                // Need to be 3 tiles at the minimum to clear
                                if(redSuite < 3)
                                {
                                    /*Debug.Log("REMOVE");
                                    Debug.Log(red.Count-redSuite);*/
                                    red.RemoveRange(red.Count-redSuite, redSuite);
                                }

                                redSuite = 0;
                            }
                                
                            break;

                        case "Green": case "Green_CS": case "Green_CS_special":
                            //Debug.Log(position);
                            // if it's the special tile
                            if(tile == "Green_CS_special")
                            {
                                greenSpecial = position;
                            }

                            greenSuite++;
                            green.Add(position);

                            //if the tile after is not red
                            if(tileAfter != "Green" && tileAfter != "Green_CS" && tileAfter != "Green_CS_special")
                            {
                                //Debug.Log("REMOVE FUNCTION");
                                // Need to be 3 tiles at the minimum to clear
                                if(greenSuite < 3)
                                {
                                    /*Debug.Log("REMOVE");*/
                                    green.RemoveRange(green.Count-greenSuite, greenSuite); 
                                }

                                greenSuite = 0;
                            }
                                
                            break;
                    }
                }
            }
        }
    }

    public void colTiles(List<Vector3Int> purple, List<Vector3Int> red, List<Vector3Int> green)
    {
        //Debug.Log("ROW TILES");
        int purpleSuite;
        int redSuite;
        int greenSuite;

        //Go from bottom to up, to retrieve the line
        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            //Debug.Log("ROW");
            // initialize at each beginning of a row
            purpleSuite = 0;
            redSuite = 0;
            greenSuite = 0;

            for (int row = bounds.yMin; row < bounds.yMax; row++)
            {
                //Debug.Log("COL");
                Vector3Int position = new Vector3Int(col, row, 0);

                if(tilemap.HasTile(position))
                {
                    string tile = tilemap.GetTile(position).name;

                    Vector3Int tileAfterPosition;
                    string tileAfter = null;

                    if(row != bounds.yMax)
                    {   
                        tileAfterPosition = new Vector3Int(col, row+1, 0);
                        if(tilemap.HasTile(tileAfterPosition))
                        {
                            tileAfter = tilemap.GetTile(tileAfterPosition).name;
                        }
                    }

                    switch(tile)
                    {
                        case "Purple": case "Purple_LG": case "Purple_LG_special":
                            //Debug.Log(position);
                            // if it's the special tile
                            if(tile == "Purple_LG_special")
                            {
                                purpleSpecial = position;
                            }

                            purpleSuite++;
                            purple.Add(position);

                            //if the tile after is not red
                            if(tileAfter != "Purple" && tileAfter != "Purple_LG" && tileAfter != "Purple_LG_special")
                            {
                                //Debug.Log("REMOVE FUNCTION");
                                // Need to be 3 tiles at the minimum to clear
                                if(purpleSuite < 3)
                                {
                                    /*Debug.Log("REMOVE");*/
                                    purple.RemoveRange(purple.Count-purpleSuite, purpleSuite);
                                }

                                purpleSuite = 0;
                            }
                                
                            break;

                        case "Red": case "Red_TM": case "Red_TM_special":
                            //Debug.Log(position);
                            // if it's the special tile
                            if(tile == "Red_TM_special")
                            {
                                redSpecial = position;
                            }

                            redSuite++;
                            red.Add(position);

                            //if the tile after is not red
                            if(tileAfter != "Red" && tileAfter != "Red_TM" && tileAfter != "Red_TM_special")
                            {
                                //Debug.Log("REMOVE FUNCTION");
                                // Need to be 3 tiles at the minimum to clear
                                if(redSuite < 3)
                                {
                                    /*Debug.Log("REMOVE");
                                    Debug.Log(red.Count-redSuite);*/
                                    red.RemoveRange(red.Count-redSuite, redSuite);
                                }

                                redSuite = 0;
                            }
                                
                            break;

                        case "Green": case "Green_CS": case "Green_CS_special":
                            //Debug.Log(position);
                            // if it's the special tile
                            if(tile == "Green_CS_special")
                            {
                                greenSpecial = position;
                            }

                            greenSuite++;
                            green.Add(position);

                            //if the tile after is not red
                            if(tileAfter != "Green" && tileAfter != "Green_CS" && tileAfter != "Green_CS_special")
                            {
                                //Debug.Log("REMOVE FUNCTION");
                                // Need to be 3 tiles at the minimum to clear
                                if(greenSuite < 3)
                                {
                                    /*Debug.Log("REMOVE");*/
                                    green.RemoveRange(green.Count-greenSuite, greenSuite);
                                }

                                greenSuite = 0;
                            }
                                
                            break;
                    }
                }
            }
        }
    }

    public List<Vector3Int> AllTiles(string name)
    {
        List<Vector3Int> tiles = new List<Vector3Int>();

        for(int row = bounds.yMin; row < bounds.yMax; row++)
        {
            for(int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);

                switch(name)
                {
                    case "purple":
                        if(tilemap.HasTile(position))
                        {
                            string tile = tilemap.GetTile<Tile>(position).name;
                            if(tile == "Purple" || tile == "Purple_LG" || tile == "Purple_LG_special")
                                tiles.Add(position);
                        }
                        break;
                    case "red":
                        if(tilemap.HasTile(position))
                        {
                            string tile = tilemap.GetTile<Tile>(position).name;
                            if(tile == "Red" || tile == "Red_TM" || tile == "Red_TM_special")
                                tiles.Add(position);
                        }
                        break;
                    case "green":
                        if(tilemap.HasTile(position))
                        {
                            string tile = tilemap.GetTile<Tile>(position).name;
                            if(tile == "Green" || tile == "Green_CS" || tile == "Green_CS_special")
                                tiles.Add(position);
                        }
                        break;
                    
                }
            }
        }

        return tiles;
    }

    public bool Down(List<Vector3Int> posPiece, float overTime)
    {
        //Debug.Log("ALL TILES");
        //Debug.Log(allTiles.Count);

        Debug.Log("DOOOOOOOOOOOOOOOOOOOOWN");

        RectInt bounds = Bounds;

        bool lockDown = false;


        // Clear all tiles in the row
        for(int row = bounds.yMin; row < bounds.yMax; row++)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                Vector3Int below = new Vector3Int(col, row - 1, 0);
                bool one = false;
                bool notPiece = false;
                    // if there is a tile
                if(tilemap.HasTile(position))
                {
                    //Debug.Log("THERE IS A TILE");
                    //Debug.Log(position);

                    // not tile below and no border below
                    if(!tilemap.HasTile(new Vector3Int(position.x, position.y-1, 0)) && !borderTilemap.HasTile(new Vector3Int(position.x, position.y-1, 0)))
                    {
                        overTime = 0;
                        Debug.Log("JE DESCEEEEEEEEEEEEEEEEEEEEEEEEEEENDS");
                        lockDown = true;
                        List<Vector3Int> copyPiece = new List<Vector3Int>(posPiece);
                        Tile tile = tilemap.GetTile<Tile>(position);

                        for(int k = 0; k < copyPiece.Count; k++)
                        {
                            if(position == copyPiece[k])
                                //posPiece[k] = new Vector3Int(position.x, position.y-1, 0);
                                notPiece = true;
                        }

                        if(!notPiece)
                        {
                            while(!tilemap.HasTile(new Vector3Int(position.x, position.y-1, 0)) && !borderTilemap.HasTile(new Vector3Int(position.x, position.y-1, 0)))
                            {
                                tilemap.SetTile(position, null);
                                position = new Vector3Int(position.x, position.y-1, 0);
                                tilemap.SetTile(position, tile);
                            }
                        }

                        overTime = 0;
                    }
                }
            }
        }

        if(lockDown)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    protected IEnumerator animateTiles(Vector3Int position, List<Vector3Int> allTiles)
    {
        clear = true;

        allTiles.Remove(position);

        Matrix4x4 newMatrix = Matrix4x4.Scale(new Vector3((float)0.8, (float)0.8, (float)0.8));
        yield return new WaitForSeconds(0.1f);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetTransformMatrix(position, newMatrix);

        newMatrix = Matrix4x4.Scale(new Vector3((float)0.6, (float)0.6, (float)0.6));
        yield return new WaitForSeconds(0.1f);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetTransformMatrix(position, newMatrix);

        newMatrix = Matrix4x4.Scale(new Vector3((float)0.4, (float)0.4, (float)0.4));
        yield return new WaitForSeconds(0.1f);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetTransformMatrix(position, newMatrix);

        newMatrix = Matrix4x4.Scale(new Vector3((float)0.2, (float)0.2, (float)0.2));
        yield return new WaitForSeconds(0.1f);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetTransformMatrix(position, newMatrix);

        yield return new WaitForSeconds(0.1f);

        tilemap.SetTile(position, null);
    }

    protected IEnumerator WaitDown(Vector3Int pos)
    {
        //List<Vector3Int> copy = new List<Vector3Int>(posPiece);
        finish = false;
        Tile tile = tilemap.GetTile<Tile>(pos);
        tilemap.SetTile(pos, null);


        /*for(int i = 0; i < copy.Count; i++)
        {

            if(copy[i] == pos)
                posPiece[i] = new Vector3Int(pos.x, pos.y-1, 0);
        }*/

        pos = new Vector3Int(pos.x, pos.y-1, 0);
        tilemap.SetTile(pos, tile);
        yield return new WaitForSeconds(0);
        finish = true;
    }

    protected IEnumerator Wait(float s)
    {
        yield return new WaitForSeconds(s);
        wait = true;
    }

    public bool IsLevelFinished()
    {
        for(int row = bounds.yMin; row < bounds.yMax; row++)
        {
            for(int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);

                if(tilemap.HasTile(position))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public virtual void GameOver() 
    {
        GameOverData.setLevel(Scene.TC11);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }

}
