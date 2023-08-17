using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Represents the objectives of the level (how many UVs need to be compeleted)
public class TC2UVsToComplete : MonoBehaviour {

    // UI elements to change depending on how many UVs have been completed
    public TextMeshProUGUI CSTextUI;
    public TextMeshProUGUI T2STextUI;
    public TextMeshProUGUI LanguageTextUI;

    // UI elements that show the number of UVs to complete for the level
    public TextMeshProUGUI CSToCompleteTextUI;
    public TextMeshProUGUI T2SToCompleteTextUI;
    public TextMeshProUGUI LanguageToCompleteTextUI;

    // Number of UVs completed per type
    private Dictionary<TC2UVType, int> UVsCompleted = new Dictionary<TC2UVType, int>() {
        {TC2UVType.CS, 0},
        {TC2UVType.T2S, 0},
        {TC2UVType.Language, 0}
    };

    // Number of UVs to complete per type
    private readonly Dictionary<TC2UVType, int> numberOfUVsToComplete = new Dictionary<TC2UVType, int>() {
        {TC2UVType.CS, 4},
        {TC2UVType.T2S, 1},
        {TC2UVType.Language, 1}
    };

    // Number of UVs to complete
    private int numberOfUVs = 0;

    // Initialises the UI of the number of UVs to complete and the number of UVs to complete
    private void Awake() {
        CSTextUI.text = UVsCompleted[TC2UVType.CS].ToString();
        T2STextUI.text = UVsCompleted[TC2UVType.T2S].ToString();
        LanguageTextUI.text = UVsCompleted[TC2UVType.Language].ToString();

        CSToCompleteTextUI.text = "("+numberOfUVsToComplete[TC2UVType.CS].ToString()+")";
        T2SToCompleteTextUI.text = "("+numberOfUVsToComplete[TC2UVType.T2S].ToString()+")";
        LanguageToCompleteTextUI.text = "("+numberOfUVsToComplete[TC2UVType.Language].ToString()+")";
        foreach(KeyValuePair<TC2UVType, int> kvp in numberOfUVsToComplete) {
            numberOfUVs += kvp.Value;
        }
    }

    // An UV has been completed
    public void CompleteUV(TC2UVType uv) {
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
    private void updateUI(TC2UVType uv) {
        switch(uv) {
            case TC2UVType.CS:
                CSTextUI.text = UVsCompleted[uv].ToString();
                break;
            case TC2UVType.T2S:
                T2STextUI.text = UVsCompleted[uv].ToString();
                break;
            case TC2UVType.Language:
                LanguageTextUI.text = UVsCompleted[uv].ToString();      
                break;
        }
    }

    // GameOver function
    private void GameOver() {
        GameOverData.setLevel(Scene.TC2);
        SceneManager.LoadScene(Data.SceneNames[Scene.GameOver]);
    }

    // Checks if the level has been finished or not
    private void checkEndLevel() {
        if((numberOfUVs) == 0) {
            SceneManager.LoadScene(Data.SceneNames[Scene.ComputerScienceYear1Rules]);
        }
    }
}