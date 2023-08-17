using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Class used by the 2nd sector choice scene
public class Sector2ndChoice : MonoBehaviour
{
    // UI of the scene
    public GameObject UICanvas;
    // Button to choose the advanced IT Development sector 
    public Button AdvancedITDevelopmentButton;
    // Button to choose the Cybersecurity sector 
    public Button CybersecurityButton;
    // Button to choose the Artificiel intelligence sector 
    public Button AIButton;
    // Button to choose the artificiel vision sector 
    public Button ArtificialVisionButton;

    GraphicRaycaster UIRaycaster;
 
    PointerEventData clickData;
    List<RaycastResult> clickResults;
 
    void Start()
    {
        UIRaycaster = UICanvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
        EventSystem eventSystem = EventSystem.current;

        // Deactivate buttons depending on the CSs and the sector completed previously
        if(SectorData.firstSector != Sector.SoftwareDeployment && SectorData.firstSector != Sector.DataScience) {
            AdvancedITDevelopmentButton.interactable = false;
        }
        if(SectorData.firstSector != Sector.Networks && SectorData.firstSector !=  Sector.EmbeddedSystems) {
            CybersecurityButton.interactable = false;
        }
        if(SectorData.firstSector != Sector.VirtualWorlds) {
            ArtificialVisionButton.interactable = false;
        }
        if(SectorData.NumberOfCSsCompleted[CSYear1UVType.TheoreticalComputerScience] < 2) {
            AIButton.interactable = false;
        }

        // Thre following code is used to find a interactable button to select
        List<Button> buttons = new List<Button>();
        buttons.Add(AdvancedITDevelopmentButton);
        buttons.Add(CybersecurityButton);
        buttons.Add(AIButton);
        buttons.Add(ArtificialVisionButton);

        foreach(Button button in buttons) {
            if(button.interactable) {
                button.Select();
                break;
            }
        }
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
 
            // Choose a sector if the button is interactable
            switch(UIElement.name) {
                case "AdvancedITDevelopment":
                    if(AdvancedITDevelopmentButton.interactable){
                        Start2ndSector(Sector.AdvancedITDevelopment);
                    }
                    break;
                case "Cybersecurity":
                    if(CybersecurityButton.interactable){
                        Start2ndSector(Sector.Cybersecurity);
                    }
                    break;
                case "AI":
                    if(AIButton.interactable){
                        Start2ndSector(Sector.AI);
                    }
                    break;
                case "ArtificialVision":
                    if(ArtificialVisionButton.interactable){
                        Start2ndSector(Sector.ArtificialVision);
                    }
                    break;
            }
        }
    }

    // Start the second sector level
    private void Start2ndSector(Sector sector) {
        SectorData.SetSecondSector(sector);
        SceneManager.LoadScene(Data.SceneNames[Scene.Sector]);
    }

    // Start the advanced IT development sector
    public void StartAdvancedITDevelopmentSector() {
        Start2ndSector(Sector.AdvancedITDevelopment);
    }

    // Start the cybersecurity sector
    public void StartCybersecuritySector() {
        Start2ndSector(Sector.Cybersecurity);
    }

    // Start the AI sector
    public void StartAISector() {
        Start2ndSector(Sector.AI);
    }

    // Start the artificial vision sector
    public void StartArtificialVisionSector() {
        Start2ndSector(Sector.ArtificialVision);
    }
}
