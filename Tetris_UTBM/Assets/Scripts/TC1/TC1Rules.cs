using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Class used by the MainMenu scene
public class TC1Rules : TC2Rules
{

    // Start the game
    protected override void StartGame() 
    {
        SceneManager.LoadScene(Data.SceneNames[Scene.TC11]);
    }
}