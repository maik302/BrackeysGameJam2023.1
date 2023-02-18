using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoresController : MonoBehaviour {

    public void GoToMainMenu() {
        SceneManager.LoadScene(ScenesNames.MainMenuScene);
    }
}
