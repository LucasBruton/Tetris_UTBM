using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TetraminosBoardTC12 : TetraminosBoardTC1
{

    public override void GameOver() 
    {
        GameOverData.setLevel(Scene.TC12);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }

}
