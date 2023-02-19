using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour {

    [Header("Game Configurations")]
    [SerializeField]
    public GameState _initialGameState;
    [SerializeField]
    [Range(3, 5)]
    int _maxHealthAllowed;
    [SerializeField]
    [Range(3, 5)]
    int _maxPowerAllowed;
    [SerializeField]
    WavesManager _wavesManager;

    [Header("Player")]
    [SerializeField]
    GameObject _playerPrefab;
    [SerializeField]
    Transform _topBoundary;
    [SerializeField]
    Transform _bottomBoundary;
    [SerializeField]
    Transform _leftBoundary;
    [SerializeField]
    Transform _rightBoundary;

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
    [Header("Game Over results screen")]
    [SerializeField]
    GameObject _gameOverScreen;

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

        // Restart game related events
        Messenger.AddListener(GameEvents.RestartGameWithPreviousMaxHealthEvent, RestartGameWithPreviousMaxHealth);
        Messenger.AddListener(GameEvents.RestartGameWithPreviousMaxPowerEvent, RestartGameWithPreviousMaxPower);
        Messenger.AddListener(GameEvents.RestartGameWithPreviousScoreEvent, RestartGameWithPreviousScore);
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
        Messenger<int>.RemoveListener(GameEvents.NewWaveStartedEvent, StartNewWave);

        // Restart game related events
        Messenger.RemoveListener(GameEvents.RestartGameWithPreviousMaxHealthEvent, RestartGameWithPreviousMaxHealth);
        Messenger.RemoveListener(GameEvents.RestartGameWithPreviousMaxPowerEvent, RestartGameWithPreviousMaxPower);
        Messenger.RemoveListener(GameEvents.RestartGameWithPreviousScoreEvent, RestartGameWithPreviousScore);
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
        var randomBgmAudioIndex = UnityEngine.Random.Range(0, 2);
        AudioManager.Instance.Play((randomBgmAudioIndex == 0) ? AudioNames.Bgm0 : AudioNames.Bgm1);
        InitGame();
    }

    void InitGame() {
        void InitPlayer() {
            var playerObject = Instantiate(_playerPrefab, new Vector3(0, 0), Quaternion.identity);

            var playerMovementBehaviour = playerObject.transform.GetComponent<PlayerMovementBehaviour>();
            if (playerMovementBehaviour != null) {
                playerMovementBehaviour.InitMovementBoundaries(_topBoundary, _bottomBoundary, _leftBoundary, _rightBoundary);
            }

            var playerHealthBehaviour = playerObject.transform.GetComponent<PlayerHealthBehaviour>();
            if (playerHealthBehaviour != null) {
                playerHealthBehaviour.InitHealthPoints(_currentGameState.PlayerMaxHealth, _maxHealthAllowed);
            }

            var playerShootingBehaviour = playerObject.transform.GetComponent<PlayerShootingBehaviour>();
            if (playerShootingBehaviour != null) {
                playerShootingBehaviour.InitPowerPoints(_currentGameState.PlayerMaxPower, _maxPowerAllowed);
            }
        }

        void InitUI() {
            _scoreCounterText.text = GameTexts.ScoreText + _currentGameState.PlayerScore.ToString("D12");
            
            _healthPointsUIController.SetMaxHealthPoints(_currentGameState.PlayerMaxHealth);
            IncreaseCurrentHealth(_currentGameState.PlayerMaxHealth);

            _powerPointsUIController.SetMaxPowerPointsAllowed(_maxPowerAllowed);
            _powerPointsUIController.SetPowerPoints(_currentGameState.PlayerMaxPower);
        }

        Time.timeScale = 1f;
        InitPlayer();
        InitUI();
        _wavesManager.StartGame();
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

        void ShowGameOverScreen() {
            _gameOverScreen.SetActive(true);

            var gameOverScreenController = _gameOverScreen.transform.GetComponent<GameOverScreenController>();
            if (gameOverScreenController != null) {
                gameOverScreenController.SetFinalResults(_currentGameState);
            }
        }

        void DeleteAllElementsOnScreen() {
            var enemyObjects = GameObject.FindGameObjectsWithTag(GameTags.EnemyTag);
            if (enemyObjects != null) {
                foreach (var enemy in enemyObjects) {
                    Destroy(enemy);
                }
            }

            var pickupItemObjects = GameObject.FindGameObjectsWithTag(GameTags.PickupItemTag);
            if (pickupItemObjects != null) {
                foreach (var pickupItem in pickupItemObjects) {
                    Destroy(pickupItem);
                }
            }

            var enemyProjectileObjectsProjectileBehaviours = GameObject.FindGameObjectsWithTag(GameTags.EnemyProjectileTag)?.Select(projectile => projectile.transform.GetComponent<ProjectileBehaviour>());
            if (enemyProjectileObjectsProjectileBehaviours != null) {
                foreach (var projectileBehaviour in enemyProjectileObjectsProjectileBehaviours) {
                    projectileBehaviour.ReleaseObject();
                }
            }

            var playerProjectileObjectsProjectileBehaviours = GameObject.FindGameObjectsWithTag(GameTags.PlayerProjectileTag)?.Select(projectile => projectile.transform.GetComponent<ProjectileBehaviour>());
            if (playerProjectileObjectsProjectileBehaviours != null) {
                foreach (var projectileBehaviour in playerProjectileObjectsProjectileBehaviours) {
                    projectileBehaviour.ReleaseObject();
                }
            }
        }

        SaveCurrentStateToPlayerPrefs();
        DeleteAllElementsOnScreen();

        // Pause the game before showing the Game Over screen
        Time.timeScale = 0f;
        ShowGameOverScreen();
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
        ResetGame();
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
        ResetGame();
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
        ResetGame();
    }

    void StartNewWave(int waveNumber) {
        _currentGameState.WaveReached = waveNumber;
        _waveCounterText.text = GameTexts.WaveText + (waveNumber + 1).ToString("D2");
    }

    void ResetGame() {
        _gameOverScreen.SetActive(false);        
        InitGame();
    }
}
