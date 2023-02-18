using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour {

    [Header("Game State")]
    [SerializeField]
    public GameState _initialGameState;
    [SerializeField]
    [Range(3, 5)]
    int _maxHealthAllowed;
    [SerializeField]
    [Range(3, 5)]
    int _maxPowerAllowed;

    [Header("Player")]
    [SerializeField]
    GameObject _player;

    [Header("HUD")]
    [Header("In Game UI")]
    [SerializeField]
    TextMeshProUGUI _scoreCounterText;
    [SerializeField]
    TextMeshProUGUI _waveCounterText;
    [SerializeField]
    HealthPointsController _healthPointsUIController;
    [SerializeField]
    PowerPointsController _powerPointsUIController;

    GameState _currentGameState;

    void OnEnable() {
        // Pickup items related events
        Messenger.AddListener(GameEvents.MaxHealthPickupGrabbedEvent, IncreaseMaxHealthStatus);
        Messenger.AddListener(GameEvents.PowerUpPickupGrabbedEvent, IncreaseMaxPowerStatus);

        // Enemies related events
        Messenger<int>.AddListener(GameEvents.EnemyDestroyedEvent, IncreaseScore);

        // Player related events
        Messenger<int>.AddListener(GameEvents.PlayerHealthIncreasedEvent, IncreaseCurrentHealth);
        Messenger.AddListener(GameEvents.PlayerTookDamageEvent, ReduceCurrentHealth);
        Messenger.AddListener(GameEvents.PlayerDiedEvent, EndCurrentGame);

        // Waves related events
        Messenger<int>.AddListener(GameEvents.NewWaveStartedEvent, StartNewWave);
    }

    void OnDisable() {
        // Pickup items related events
        Messenger.RemoveListener(GameEvents.MaxHealthPickupGrabbedEvent, IncreaseMaxHealthStatus);
        Messenger.RemoveListener(GameEvents.PowerUpPickupGrabbedEvent, IncreaseMaxPowerStatus);

        // Enemies related events
        Messenger<int>.RemoveListener(GameEvents.EnemyDestroyedEvent, IncreaseScore);

        // Player related events
        Messenger<int>.RemoveListener(GameEvents.PlayerHealthIncreasedEvent, IncreaseCurrentHealth);
        Messenger.RemoveListener(GameEvents.PlayerTookDamageEvent, ReduceCurrentHealth);
        Messenger.RemoveListener(GameEvents.PlayerDiedEvent, EndCurrentGame);

        // Waves related events
        Messenger<int>.AddListener(GameEvents.NewWaveStartedEvent, StartNewWave);
    }

    void Awake() {
        // Only when starting anew, use the initial values set for the game state
        _currentGameState = new GameState(
            _initialGameState.PlayerScore,
            _initialGameState.PlayerMaxHealth,
            _initialGameState.PlayerCurrentHealth,
            _initialGameState.PlayerMaxPower,
            _initialGameState.WaveReached,
            0,
            0,
            0
        );
    }

    // Start is called before the first frame update
    void Start() {
        InitGame();
    }

    void InitGame() {
        void InitPlayer() {
            var playerHealthBehaviour = _player.transform.GetComponent<PlayerHealthBehaviour>();
            if (playerHealthBehaviour != null) {
                playerHealthBehaviour.InitHealthPoints(_currentGameState.PlayerMaxHealth, _maxHealthAllowed);
            }

            var playerShootingBehaviour = _player.transform.GetComponent<PlayerShootingBehaviour>();
            if (playerShootingBehaviour != null) {
                playerShootingBehaviour.InitPowerPoints(_currentGameState.PlayerMaxPower, _maxPowerAllowed);
            }
        }

        void InitUI() {
            _scoreCounterText.text = GameTexts.ScoreText + 0.ToString("D12");
            
            _healthPointsUIController.SetMaxHealthPoints(_currentGameState.PlayerMaxHealth);
            IncreaseCurrentHealth(_currentGameState.PlayerMaxHealth);

            _powerPointsUIController.SetMaxPowerPointsAllowed(_maxPowerAllowed);
            _powerPointsUIController.SetPowerPoints(_currentGameState.PlayerMaxPower);
        }

        InitPlayer();
        InitUI();
    }

    void IncreaseMaxHealthStatus() {
        _currentGameState.PlayerMaxHealth += (_currentGameState.PlayerMaxHealth + 1) <= _maxHealthAllowed ? 1 : 0;
        _healthPointsUIController.SetMaxHealthPoints(_currentGameState.PlayerMaxHealth);
        IncreaseCurrentHealth(_currentGameState.PlayerMaxHealth);
    }

    void IncreaseMaxPowerStatus() {
        _currentGameState.PlayerMaxPower += (_currentGameState.PlayerMaxPower + 1) <= _maxPowerAllowed ? 1 : 0;
        _powerPointsUIController.SetPowerPoints(_currentGameState.PlayerMaxPower);
    }

    void IncreaseScore(int newScoredPoints) {
        _currentGameState.PlayerScore += newScoredPoints;
        _scoreCounterText.text = GameTexts.ScoreText + _currentGameState.PlayerScore.ToString("D12");
    }

    void IncreaseCurrentHealth(int healthPoints) {
        if (_currentGameState.PlayerCurrentHealth + healthPoints >= _currentGameState.PlayerMaxHealth) {
            _currentGameState.PlayerCurrentHealth = _currentGameState.PlayerMaxHealth;
        } else {
            _currentGameState.PlayerCurrentHealth += healthPoints;
        }
        _healthPointsUIController.SetHealthPoints(_currentGameState.PlayerCurrentHealth);
    }

    void ReduceCurrentHealth() {
        _currentGameState.PlayerCurrentHealth -= (_currentGameState.PlayerCurrentHealth - 1) < 0 ? 0 : 1;
        _healthPointsUIController.SetHealthPoints(_currentGameState.PlayerCurrentHealth);
    }

    void EndCurrentGame() {
        void SaveCurrentStateToPlayerPrefs() {
            // Get the current high scores to save only the GameValues.MaxHighScoresToSave 
            var highScoresHolder = PlayerPrefsUtils.GetHighScoresFromPlayerPrefs();
            if (highScoresHolder == null) {
                highScoresHolder = new HighScoresHolder();
            }
            highScoresHolder.Add(_currentGameState);
            highScoresHolder.SortDescending();
            var updatedHighScoresHolder = new HighScoresHolder(highScoresHolder.Take(GameValues.MaxHighScoresToSave));

            PlayerPrefsUtils.SaveHighScoresToPlayerPrefs(updatedHighScoresHolder);
        }

        SaveCurrentStateToPlayerPrefs();
        // TODO: Show UI for restarting a game with modifiers or go back to main menu
    }

    void RestartGameWithPreviousMaxHealth() {
        _currentGameState = new GameState(
            _initialGameState.PlayerScore,
            _currentGameState.PlayerMaxHealth,
            _initialGameState.PlayerCurrentHealth,
            _initialGameState.PlayerMaxPower,
            _initialGameState.WaveReached,
            _currentGameState.MaxHealthModifiersCount + 1,
            _currentGameState.MaxPowerModifiersCount,
            _currentGameState.ScoreModifiersCount
        );
        // TODO: Destroy all enemies
        // TODO: Restart WaveManager and Spawners
    }

    void RestartGameWithPreviousMaxPower() {
        _currentGameState = new GameState(
            _initialGameState.PlayerScore,
            _initialGameState.PlayerMaxHealth,
            _initialGameState.PlayerCurrentHealth,
            _currentGameState.PlayerMaxPower,
            _initialGameState.WaveReached,
            _currentGameState.MaxHealthModifiersCount,
            _currentGameState.MaxPowerModifiersCount + 1,
            _currentGameState.ScoreModifiersCount
        );
        // TODO: Destroy all enemies
        // TODO: Restart WaveManager and Spawners
    }

    void RestartGameWithPreviousScore() {
        _currentGameState = new GameState(
            _currentGameState.PlayerScore,
            _initialGameState.PlayerMaxHealth,
            _initialGameState.PlayerCurrentHealth,
            _initialGameState.PlayerMaxPower,
            _initialGameState.WaveReached,
            _currentGameState.MaxHealthModifiersCount,
            _currentGameState.MaxPowerModifiersCount,
            _currentGameState.ScoreModifiersCount + 1
        );
        // TODO: Destroy all enemies
        // TODO: Restart WaveManager and Spawners
    }

    void StartNewWave(int waveNumber) {
        _waveCounterText.text = GameTexts.WaveText + (waveNumber + 1).ToString("D2");
    }
}
