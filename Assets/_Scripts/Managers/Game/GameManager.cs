using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    [Header("Game State")]
    [SerializeField]
    public GameState _initialGameState;
    [SerializeField]
    int _maxHealthAllowed;
    [SerializeField]
    int _maxPowerAllowed;

    [Header("Player")]
    [SerializeField]
    GameObject _player;

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
    }

    void Awake() {
        // Only when starting anew, use the initial values set for the game state
        _currentGameState = new GameState(
            _initialGameState.PlayerScore,
            _initialGameState.PlayerMaxHealth,
            _initialGameState.PlayerCurrentHealth,
            _initialGameState.PlayerMaxPower,
            _initialGameState.PlayerCurrentPower,
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

        InitPlayer();
        // TODO: Init UI with current game state
    }

    // Update is called once per frame
    void Update() {
        
    }

    void IncreaseMaxHealthStatus() {
        _currentGameState.PlayerMaxHealth += (_currentGameState.PlayerMaxHealth + 1) <= _maxHealthAllowed ? 1 : 0;
        // TODO: Update UI showing a new max health pack
    }

    void IncreaseMaxPowerStatus() {
        _currentGameState.PlayerMaxPower += (_currentGameState.PlayerMaxPower + 1) <= _maxPowerAllowed ? 1 : 0;
        // TODO: Update UI showing a new max power level
    }

    void IncreaseScore(int newScoredPoints) {
        _currentGameState.PlayerScore += newScoredPoints;
        // TODO: Update UI showing the new score
    }

    void IncreaseCurrentHealth(int healthPoints) {
        _currentGameState.PlayerCurrentHealth += (healthPoints + healthPoints) <= _currentGameState.PlayerMaxHealth ? healthPoints : 0;
        // TODO: Update UI health points
    }

    void ReduceCurrentHealth() {
        _currentGameState.PlayerCurrentHealth = (_currentGameState.PlayerCurrentHealth - 1) <= 0 ? 0 : _currentGameState.PlayerCurrentHealth - 1;
        // TODO: Update UI health points
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
            _initialGameState.PlayerCurrentPower,
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
            _initialGameState.PlayerCurrentPower,
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
            _initialGameState.PlayerCurrentPower,
            _initialGameState.WaveReached,
            _currentGameState.MaxHealthModifiersCount,
            _currentGameState.MaxPowerModifiersCount,
            _currentGameState.ScoreModifiersCount + 1
        );
        // TODO: Destroy all enemies
        // TODO: Restart WaveManager and Spawners
    }
}
