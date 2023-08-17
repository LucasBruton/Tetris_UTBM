using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Represents the objectives of the level (how many UVs need to be compeleted)
public class SectorUVsToComplete : MonoBehaviour {

    // UI elements to change depending on how many UVs have been completed
    public TextMeshProUGUI TMTextUI;
    public TextMeshProUGUI T2STextUI;
    public TextMeshProUGUI LanguageTextUI;

    // UI elements that show the number of UVs to complete for the level
    public TextMeshProUGUI TMToCompleteTextUI;
    public TextMeshProUGUI T2SToCompleteTextUI;
    public TextMeshProUGUI LanguageToCompleteTextUI;

    // Number of UVs completed per type
    private Dictionary<SectorUVType, int> UVsCompleted = new Dictionary<SectorUVType, int>() {
        {SectorUVType.TM, 0},
        {SectorUVType.T2S, 0},
        {SectorUVType.Language, 0}
    };

    // Number of UVs to complete per type
    private readonly Dictionary<SectorUVType, int> numberOfUVsToComplete = new Dictionary<SectorUVType, int>() {
        {SectorUVType.TM, 4},
        {SectorUVType.T2S, 1},
        {SectorUVType.Language, 1}
    };

    // Number of UVs to complete
    private int numberOfUVs = 0;

    // Initialises the UI of the number of UVs to complete and the number of UVs to complete
    private void Awake() {
        TMTextUI.text = UVsCompleted[SectorUVType.TM].ToString();
        T2STextUI.text = UVsCompleted[SectorUVType.T2S].ToString();
        LanguageTextUI.text = UVsCompleted[SectorUVType.Language].ToString();

        TMToCompleteTextUI.text = "("+numberOfUVsToComplete[SectorUVType.TM].ToString()+")";
        T2SToCompleteTextUI.text = "("+numberOfUVsToComplete[SectorUVType.T2S].ToString()+")";
        LanguageToCompleteTextUI.text = "("+numberOfUVsToComplete[SectorUVType.Language].ToString()+")";
        foreach(KeyValuePair<SectorUVType, int> kvp in numberOfUVsToComplete) {
            numberOfUVs += kvp.Value;
        }
    }

    // An UV has been completed
    public void CompleteUV(SectorUVType uv) {
        // If too many UVs of a certain type have been completed, then the player loses
        if((++UVsCompleted[uv]) <= numberOfUVsToComplete[uv]) {
            --numberOfUVs;
            updateUI(uv);
            checkEndLevel();
        }else {
            GameOver();
        }
    }

    // Updates the UI of a certain type of UV
    private void updateUI(SectorUVType uv) {
        switch(uv) {
            case SectorUVType.TM:
                TMTextUI.text = UVsCompleted[uv].ToString();
                break;
            case SectorUVType.T2S:
                T2STextUI.text = UVsCompleted[uv].ToString();
                break;
            case SectorUVType.Language:
                LanguageTextUI.text = UVsCompleted[uv].ToString();      
                break;
        }
    }

    // GameOver function
    private void GameOver() {
        GameOverData.setLevel(Scene.Sector);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }

    // Checks if the level has been finished or not
    private void checkEndLevel() {
        if((numberOfUVs) == 0) {
            if(SectorData.nextSector == 1) {
                SceneManager.LoadScene(Data.SceneNames[Scene.Sector2ndChoice]); 
            } else {
                SceneManager.LoadScene(Data.SceneNames[Scene.FinishedGame]);
            }
        }
    }
}