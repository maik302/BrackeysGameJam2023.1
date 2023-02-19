using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreRowController : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI _scoreText;
    [SerializeField]
    TextMeshProUGUI _maxHealthModifierCountText;
    [SerializeField]
    TextMeshProUGUI _powerModifierCountText;
    [SerializeField]
    TextMeshProUGUI _scoreModifierText;
    [SerializeField]
    TextMeshProUGUI _wavesCounterText;

    public void SetUpScoreRow(GameState scoredGameState) {
        _scoreText.text = scoredGameState.PlayerScore.ToString("D12");
        _maxHealthModifierCountText.text = scoredGameState.MaxHealthModifiersCount.ToString("D2");
        _powerModifierCountText.text = scoredGameState.MaxPowerModifiersCount.ToString("D2");
        _scoreModifierText.text = scoredGameState.ScoreModifiersCount.ToString("D2");
        _wavesCounterText.text = GameTexts.HighScoresWavesIdentifiarText + scoredGameState.WaveReached.ToString("D2");
    }
}
