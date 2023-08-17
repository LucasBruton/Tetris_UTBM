using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Class used by the SectorRules scene
public class SectorRules : MonoBehaviour
{
    public GameObject UICanvas;
    GraphicRaycaster UIRaycaster;
 
    PointerEventData clickData;
    List<RaycastResult> clickResults;
 
    void Start()
    {
        UIRaycaster = UICanvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
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
                case "FirstSectorChoice":
                    StartFirstSectorChoice();
                    break;
            }
        }
    }

    // Start the Sector 1st Choice scene
    public void StartFirstSectorChoice() {
        SceneManager.LoadScene(Data.SceneNames[Scene.Sector1stChoice]);
    }
}