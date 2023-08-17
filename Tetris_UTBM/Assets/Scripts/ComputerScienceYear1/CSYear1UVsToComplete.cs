using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Represents the objectives of the CSYear1 level (how many UVs need to be compeleted)
public class CSYear1UVsToComplete : MonoBehaviour {

    // UI elements to change depending on how many UVs have been completed
    public TextMeshProUGUI CSTextUI;
    public TextMeshProUGUI TCSTextUI;
    public TextMeshProUGUI APTextUI;
    public TextMeshProUGUI NTextUI;
    public TextMeshProUGUI DTextUI;
    public TextMeshProUGUI HSTextUI;
    public TextMeshProUGUI T2STextUI;
    public TextMeshProUGUI LanguageTextUI;

    // UI elements that show the number of UVs to complete for the level
    public TextMeshProUGUI CSToCompleteTextUI;
    public TextMeshProUGUI T2SToCompleteTextUI;
    public TextMeshProUGUI LanguageToCompleteTextUI;

    // Number of UVs completed per type
    private Dictionary<CSYear1UVType, int> UVsCompleted = new Dictionary<CSYear1UVType, int>() {
        {CSYear1UVType.CS, 0},
        {CSYear1UVType.TheoreticalComputerScience, 0},
        {CSYear1UVType.AlgoAndPrograming, 0},
        {CSYear1UVType.Network, 0},
        {CSYear1UVType.Database, 0},
        {CSYear1UVType.HardwareAndSystems, 0},
        {CSYear1UVType.T2S, 0},
        {CSYear1UVType.Language, 0}
    };

    // Number of UVs to complete per type
    private readonly Dictionary<CSYear1UVType, int> numberOfUVsToComplete = new Dictionary<CSYear1UVType, int>() {
        {CSYear1UVType.CS, 4},
        {CSYear1UVType.T2S, 1},
        {CSYear1UVType.Language, 1}
    };

    // Number of UVs to complete
    private int numberOfUVs = 0;

    // Initialises the UI of the number of UVs to complete and the number of UVs to complete
    private void Awake() {
        CSTextUI.text = UVsCompleted[CSYear1UVType.CS].ToString();
        TCSTextUI.text = UVsCompleted[CSYear1UVType.TheoreticalComputerScience].ToString();
        APTextUI.text = UVsCompleted[CSYear1UVType.AlgoAndPrograming].ToString();
        NTextUI.text = UVsCompleted[CSYear1UVType.Network].ToString();
        DTextUI.text = UVsCompleted[CSYear1UVType.Database].ToString();
        HSTextUI.text = UVsCompleted[CSYear1UVType.HardwareAndSystems].ToString();
        T2STextUI.text = UVsCompleted[CSYear1UVType.T2S].ToString();
        LanguageTextUI.text = UVsCompleted[CSYear1UVType.Language].ToString();

        CSToCompleteTextUI.text = "("+numberOfUVsToComplete[CSYear1UVType.CS].ToString()+")";
        T2SToCompleteTextUI.text = "("+numberOfUVsToComplete[CSYear1UVType.T2S].ToString()+")";
        LanguageToCompleteTextUI.text = "("+numberOfUVsToComplete[CSYear1UVType.Language].ToString()+")";
        foreach(KeyValuePair<CSYear1UVType, int> kvp in numberOfUVsToComplete) {
            numberOfUVs += kvp.Value;
        }
    }

    // An UV has been completed
    public void CompleteUV(CSYear1UVType uv) {
        if(uv == CSYear1UVType.TheoreticalComputerScience || uv == CSYear1UVType.AlgoAndPrograming || uv == CSYear1UVType.Network || uv == CSYear1UVType.Database || uv == CSYear1UVType.HardwareAndSystems) {
            CompletedCS(uv);
        } else {
            CompletedHumanityUV(uv);
        }
    }

    // a CS UV has been completed
    private void CompletedCS(CSYear1UVType uv) {
        ++UVsCompleted[uv];
        if((++UVsCompleted[CSYear1UVType.CS]) <= numberOfUVsToComplete[CSYear1UVType.CS]) {
            --numberOfUVs;
            checkEndLevel();
            updateUI(uv);
        } else {
            GameOver();
        }
    }

    // A humanity UV (T2S or language UV) has been completed
    private void CompletedHumanityUV(CSYear1UVType uv) {
        if((++UVsCompleted[uv]) <= numberOfUVsToComplete[uv]) {
            --numberOfUVs;
            updateUI(uv);
            checkEndLevel();
        }else {
            GameOver();
        }
    }

    // Updates the UI of the completed type of UV
    private void updateUI(CSYear1UVType uv) {
        switch(uv) {
            case CSYear1UVType.TheoreticalComputerScience:
                TCSTextUI.text = UVsCompleted[uv].ToString();
                CSTextUI.text = UVsCompleted[CSYear1UVType.CS].ToString();
                break;
            case CSYear1UVType.AlgoAndPrograming:
                APTextUI.text = UVsCompleted[uv].ToString();
                CSTextUI.text = UVsCompleted[CSYear1UVType.CS].ToString();
                break;
            case CSYear1UVType.Network:
                NTextUI.text = UVsCompleted[uv].ToString();
                CSTextUI.text = UVsCompleted[CSYear1UVType.CS].ToString();
                break;
            case CSYear1UVType.Database:
                DTextUI.text = UVsCompleted[uv].ToString();
                CSTextUI.text = UVsCompleted[CSYear1UVType.CS].ToString();
                break;
            case CSYear1UVType.HardwareAndSystems:
                HSTextUI.text = UVsCompleted[uv].ToString();
                CSTextUI.text = UVsCompleted[CSYear1UVType.CS].ToString();
                break;
            case CSYear1UVType.T2S:
                T2STextUI.text = UVsCompleted[uv].ToString();
                break;
            case CSYear1UVType.Language:
                LanguageTextUI.text = UVsCompleted[uv].ToString();      
                break;
        }
    }

    // GameOver function
    private void GameOver() {
        GameOverData.setLevel(Scene.ComputerScienceYear1);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }

    // Checks if the level has been finished or not
    private void checkEndLevel() {
        if((numberOfUVs) == 0) {
            SectorData.CompletedCS(CSYear1UVType.AlgoAndPrograming, UVsCompleted[CSYear1UVType.AlgoAndPrograming]);
            SectorData.CompletedCS(CSYear1UVType.Network, UVsCompleted[CSYear1UVType.Network]);
            SectorData.CompletedCS(CSYear1UVType.HardwareAndSystems, UVsCompleted[CSYear1UVType.HardwareAndSystems]);
            SectorData.CompletedCS(CSYear1UVType.Database, UVsCompleted[CSYear1UVType.Database]);
            SectorData.CompletedCS(CSYear1UVType.TheoreticalComputerScience, UVsCompleted[CSYear1UVType.TheoreticalComputerScience]);
            SceneManager.LoadScene(Data.SceneNames[Scene.SectorRules]);
        }
    }
}