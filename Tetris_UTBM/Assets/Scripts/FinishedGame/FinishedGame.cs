using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Class used by the FinishedScene scene
public class FinishedGame : MonoBehaviour
{
    // UI of the scene
    public GameObject UICanvas;

    // Text to display the completed sectors done by the player
    public TextMeshProUGUI CompletedSectorsText;

    GraphicRaycaster UIRaycaster;
 
    PointerEventData clickData;
    List<RaycastResult> clickResults;
 
    void Start()
    {
        UIRaycaster = UICanvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
        CompletedSectorsText.text = "- " + Data.SectorNames[SectorData.firstSector] + "\n\n- " + Data.SectorNames[SectorData.secondSector];
    }
 
    void Update()
    {
        // Checks if a left click has been done
        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            GetUiElementsClicked();
        }
    }
 
    // Method that looks at what UI element has been clicked
    private void GetUiElementsClicked()
    {
        // Get all the UI elements clicked, using the current mouse position and raycasting.
        clickData.position = Mouse.current.position.ReadValue();
        clickResults.Clear();
 
        UIRaycaster.Raycast(clickData, clickResults);
 
        foreach(RaycastResult result in clickResults)
        {
            GameObject UIElement = result.gameObject;
 
            switch(UIElement.name) {
                case "MainMenu":
                    StartMainMenu();
                    break;
            }
        }
    }

    // Start the MainMenu Scene
    public void StartMainMenu() {
        SectorData.ResetSectorData();
        SceneManager.LoadScene(Data.SceneNames[Scene.MainMenu]);
    }
}