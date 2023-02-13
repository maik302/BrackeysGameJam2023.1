using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public void GoToMainGameScene() {
        SceneManager.LoadScene("MainGame");
    }

    public void GoToHighScoresScene() {
        SceneManager.LoadScene("HighScores");
    }
}
