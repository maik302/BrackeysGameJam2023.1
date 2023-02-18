using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreenController : MonoBehaviour {
    [SerializeField]
    TextMeshProUGUI _finalScoreText;
    [SerializeField]
    TextMeshProUGUI _finalWavesText;
    [SerializeField]
    TextMeshProUGUI _maxHealthModifiersCountText;
    [SerializeField]
    TextMeshProUGUI _powerModifiersCountText;
    [SerializeField]
    TextMeshProUGUI _scoreModifiersCountText;

    public void SetFinalResults(GameState finalGameState) {
        _finalScoreText.text = GameTexts.ScoreText + finalGameState.PlayerScore.ToString("D12");
        _finalWavesText.text = GameTexts.WavesClearedText + finalGameState.WaveReached.ToString("D2");
        _maxHealthModifiersCountText.text = finalGameState.MaxHealthModifiersCount.ToString("D2");
        _powerModifiersCountText.text = finalGameState.MaxPowerModifiersCount.ToString("D2");
        _scoreModifiersCountText.text = finalGameState.ScoreModifiersCount.ToString("D2");
    }

    public void RestartGameWithPreviousMaxHealth() {
        Messenger.Broadcast(GameEvents.RestartGameWithPreviousMaxHealthEvent);
    }

    public void RestartGameWithPreviousMaxPower() {
        Messenger.Broadcast(GameEvents.RestartGameWithPreviousMaxPowerEvent);
    }

    public void RestartGameWithPreviousScore() {
        Messenger.Broadcast(GameEvents.RestartGameWithPreviousScoreEvent);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene(ScenesNames.MainMenuScene);
    }
}
