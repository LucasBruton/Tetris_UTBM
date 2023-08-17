using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Class used by the 1st sector choice scene
public class Sector1stChoice : MonoBehaviour
{
    // UI of the scene
    public GameObject UICanvas;
    // Button to choose the data science sector 
    public Button DataScienceButton;
    // Button to choose the software deployment sector 
    public Button SoftwareDeploymentButton;
    // Button to choose the virtual world sector 
    public Button VirtualWorldsButton;
    // Button to choose embedded systems sector 
    public Button EmbeddedSystemsButton;
    // Button to choose the network sector 
    public Button NetworksButton;

    GraphicRaycaster UIRaycaster;
 
    PointerEventData clickData;
    List<RaycastResult> clickResults;

 
    void Start()
    {
        UIRaycaster = UICanvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
        EventSystem eventSystem = EventSystem.current;

        // Deactivate buttons depending on the CSs completed during the first year of computer science level
        if(SectorData.NumberOfCSsCompleted[CSYear1UVType.Database] < 1) {
            DataScienceButton.interactable = false;
        }
        if(SectorData.NumberOfCSsCompleted[CSYear1UVType.HardwareAndSystems] < 1) {
            EmbeddedSystemsButton.interactable = false;
        }
        if(SectorData.NumberOfCSsCompleted[CSYear1UVType.AlgoAndPrograming] < 1) {
            SoftwareDeploymentButton.interactable = false;
        }
        if(SectorData.NumberOfCSsCompleted[CSYear1UVType.Network] < 1) {
            NetworksButton.interactable = false;
        }
        if(SectorData.NumberOfCSsCompleted[CSYear1UVType.TheoreticalComputerScience] < 1) {
            VirtualWorldsButton.interactable = false;
        }

        // Thre following code is used to find a interactable button to select
        List<Button> buttons = new List<Button>();
        buttons.Add(DataScienceButton);
        buttons.Add(EmbeddedSystemsButton);
        buttons.Add(SoftwareDeploymentButton);
        buttons.Add(NetworksButton);
        buttons.Add(VirtualWorldsButton);

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
                case "DataScience":
                    if(DataScienceButton.interactable){
                        Start1stSector(Sector.DataScience);
                    }
                    break;
                case "SoftwareDeployment":
                    if(SoftwareDeploymentButton.interactable){
                        Start1stSector(Sector.SoftwareDeployment);
                    }
                    break;
                case "VirtualWorlds":
                    if(VirtualWorldsButton.interactable){
                        Start1stSector(Sector.VirtualWorlds);
                    }
                    break;
                case "EmbeddedSystems":
                    if(EmbeddedSystemsButton.interactable){
                        Start1stSector(Sector.EmbeddedSystems);
                    }
                    break;
                case "Networks":
                    if(NetworksButton.interactable){
                        Start1stSector(Sector.Networks);
                    }
                    break;
            }
        }
    }

    // Start the first sector level
    public void Start1stSector(Sector sector) {
        SectorData.SetFirstSector(sector);
        SceneManager.LoadScene(Data.SceneNames[Scene.Sector]);
    }

    // Start the data science sector
    public void StartDataScienceSector() {
        Start1stSector(Sector.DataScience);
    }

    // Start the software deployment sector
    public void StartSoftwareDeploymentSector() {
        Start1stSector(Sector.SoftwareDeployment);
    }

    // Start the virutual world sector
    public void StartVirtualWorldsSector() {
        Start1stSector(Sector.VirtualWorlds);
    }

    // Start the embedded sytems sector
    public void StartEmbeddedSystemseSector() {
        Start1stSector(Sector.EmbeddedSystems);
    }

    // Start the networks sector
    public void StartNetworksSector() {
        Start1stSector(Sector.Networks);
    }
}
