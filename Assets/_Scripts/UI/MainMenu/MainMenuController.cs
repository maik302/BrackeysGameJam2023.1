using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    void Start() {
        AudioManager.Instance.PlayBgmWithoutInterruption(AudioNames.MenuMusic);
    }

    public void GoToMainGameScene() {
        SceneManager.LoadScene(ScenesNames.MainGameScene);
    }

    public void GoToHighScoresScene() {
        SceneManager.LoadScene(ScenesNames.HighScoresScene);
    }
}
