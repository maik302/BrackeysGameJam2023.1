using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public void GoToMainGameScene() {
        // TODO: Navigate to main game Scene
    }

    public void GoToHighScoresScene() {
        SceneManager.LoadScene("HighScores");
    }
}
