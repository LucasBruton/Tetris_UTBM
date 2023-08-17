using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ActiveTetraminoTC12 : ActiveTetraminoTC1
{

    public override void NameNextScene()
    {
        nextScene = "TC2 - règles";
    }

    public override void Rotate(InputAction.CallbackContext context)
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
            List<Vector3Int> newPos = new List<Vector3Int>(pos);

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

            switch (rotationIndex)
            {
                case 0:
                    for(int i = 0; i < pos.Count; i++)
                    {
                        Debug.Log("ID ATTENDU POUR 0");
                        if(id[i] == 0)
                            newPos[i] = new Vector3Int(pos[i].x-2, pos[i].y, 0);
                        if(id[i] == 2)
                            newPos[i] = new Vector3Int(pos[i].x+1, pos[i].y-1, 0);
                        if(id[i] == 3)
                            newPos[i] = new Vector3Int(pos[i].x, pos[i].y+2, 0);
                        Debug.Log(id[i]);
                        Debug.Log(newPos[i]);
                    }
                    break;
                case 1:
                    for(int i = 0; i < pos.Count; i++)
                    {
                        Debug.Log("ID ATTENDU POUR 1");
                        if(id[i] == 0)
                            newPos[i] = new Vector3Int(pos[i].x, pos[i].y+2, 0);
                        if(id[i] == 2)
                            newPos[i] = new Vector3Int(pos[i].x-1, pos[i].y-1, 0);
                        if(id[i] == 3)
                            newPos[i] = new Vector3Int(pos[i].x+2, pos[i].y, 0);
                        Debug.Log(id[i]);
                        Debug.Log(newPos[i]);
                    }
                    break;
                case 2:
                    for(int i = 0; i < pos.Count; i++)
                    {
                        Debug.Log("ID ATTENDU POUR 2");
                        if(id[i] == 0)
                            newPos[i] = new Vector3Int(pos[i].x+2, pos[i].y, 0);
                        if(id[i] == 2)
                            newPos[i] = new Vector3Int(pos[i].x-1, pos[i].y+1, 0);
                        if(id[i] == 3)
                            newPos[i] = new Vector3Int(pos[i].x, pos[i].y-2, 0);
                        Debug.Log(id[i]);
                        Debug.Log(newPos[i]);
                    }
                    break;
                case 3:
                    for(int i = 0; i < pos.Count; i++)
                    {
                        if(id[i] == 0)
                            newPos[i] = new Vector3Int(pos[i].x, pos[i].y-2, 0);
                        if(id[i] == 2)
                            newPos[i] = new Vector3Int(pos[i].x+1, pos[i].y+1, 0);
                        if(id[i] == 3)
                            newPos[i] = new Vector3Int(pos[i].x-2, pos[i].y, 0);
                    }
                    break;

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

}
