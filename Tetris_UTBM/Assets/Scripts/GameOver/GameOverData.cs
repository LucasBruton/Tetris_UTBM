using System.Collections.Generic;
using UnityEngine;

public static class GameOverData {
    public static string sceneName{ get; private set; }

    public static void setLevel(Scene scene) {
        sceneName = Data.SceneNames[scene];
    }
}