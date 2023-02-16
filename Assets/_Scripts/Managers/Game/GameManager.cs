using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Game State")]
    [SerializeField]
    GameState _initialGameState;
    [SerializeField]
    int _maxHealthAllowed;
    [SerializeField]
    int _maxPowerAllowed;

    [Header("Player")]
    [SerializeField]
    GameObject _player;

    GameState _currentGameState;

    void OnEnable() {
        Messenger.AddListener(GameEvents.MaxHealthPickupGrabbedEvent, IncreaseMaxHealthStatus);
        Messenger.AddListener(GameEvents.PowerUpPickupGrabbedEvent, IncreaseMaxPowerStatus);
        Messenger<int>.AddListener(GameEvents.EnemyDestroyedEvent, IncreaseScore);
    }

    void OnDisable() {
        Messenger.RemoveListener(GameEvents.MaxHealthPickupGrabbedEvent, IncreaseMaxHealthStatus);
        Messenger.RemoveListener(GameEvents.PowerUpPickupGrabbedEvent, IncreaseMaxPowerStatus);
        Messenger<int>.RemoveListener(GameEvents.EnemyDestroyedEvent, IncreaseScore);
    }

    void Awake() {
        // Only when starting anew, use the initial values set for the game state
        _currentGameState = _initialGameState;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void IncreaseMaxHealthStatus() {
        _currentGameState.PlayerMaxHealth += (_currentGameState.PlayerMaxHealth + 1) < _maxHealthAllowed ? 1 : 0;
        // TODO: Update UI showing a new max health pack
    }

    void IncreaseMaxPowerStatus() {
        _currentGameState.PlayerPowerLevel += (_currentGameState.PlayerPowerLevel + 1) < _maxPowerAllowed ? 1 : 0;
        // TODO: Update UI showing a new max power level
    }

    void IncreaseScore(int newScoredPoints) {
        _currentGameState.PlayerScore += newScoredPoints;
        // TODO: Update UI showing the new score
    }
}
