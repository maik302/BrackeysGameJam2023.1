using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour {

    [Header("General configuration")]
    [SerializeField]
    int _waveToStartHardMode;

    [Header("Items Waves configuration")]
    [SerializeField]
    PickupItemsSpawnerBehaviour _pickupItemsSpawner;
    [SerializeField]
    List<PickupItemsSpawnConfiguration> _pickupItemsWavesConfiguration;

    [Header("Enemy Waves configuration")]
    [SerializeField]
    EnemiesSpawnerBehaviour _enemiesSpawner;
    [SerializeField]
    List<EnemiesSpawnConfiguration> _enemyWavesConfiguration;

    int _waveCounter;
    int _currentWaveEnemiesCount;
    bool _isHardModeActive;

    void OnEnable() {
        Messenger<int>.AddListener(GameEvents.EnemyDestroyedEvent, OnEnemyKilled);
    }

    void OnDisable() {
        Messenger<int>.RemoveListener(GameEvents.EnemyDestroyedEvent, OnEnemyKilled);
    }

    void OnEnemyKilled(int scoredPoints) {
        _currentWaveEnemiesCount--;
        if (_currentWaveEnemiesCount <= 0) {
            _waveCounter++;
            StartWave();
        }
    }

    void Awake() {
        _waveCounter = 0;
        _isHardModeActive = false;
    }

    // Start is called before the first frame update
    void Start() {
        StartWave();
    }

    void StartWave() {
        Messenger<int>.Broadcast(GameEvents.NewWaveStartedEvent, _waveCounter);

        _isHardModeActive = _waveCounter >= _waveToStartHardMode;

        var currentPickupItemssWaveConfigurationIndex = _waveCounter % _pickupItemsWavesConfiguration.Count;
        StartPickupItemsSpawner(currentPickupItemssWaveConfigurationIndex);

        var currentEnemiesWaveConfigurationIndex = _waveCounter % _enemyWavesConfiguration.Count;
        StartEnemiesSpawner(currentEnemiesWaveConfigurationIndex);
    }

    void StartPickupItemsSpawner(int waveIndex) {
        void ActivateHardMode(PickupItemsSpawnConfiguration configuration) {
            configuration.MaxItemsToSpawn /= 2;
            configuration.MinSpawnFrequency += configuration.MinSpawnFrequency * .75f;
            configuration.MaxSpawnFrequency += configuration.MaxSpawnFrequency * .75f;

            configuration.ProbabilityForMaxHealth /= 3f;
            configuration.ProbabilityForPowerUp /= 3f;
            configuration.ProbabilityForHealth /= 2f;
        }

        var newWaveConfiguration = _pickupItemsWavesConfiguration[waveIndex].GetCopy();
        if (_isHardModeActive) {
            ActivateHardMode(newWaveConfiguration);
        }

        _pickupItemsSpawner.Init(newWaveConfiguration);
        _pickupItemsSpawner.StartSpawning();
    }

    void StartEnemiesSpawner(int waveIndex) {
        void ActivateHardMode(EnemiesSpawnConfiguration configuration) {
            configuration.EnemiesInGameThreshold += (configuration.EnemiesInGameThreshold / 2);

            configuration.MinSpawnFrequency -= configuration.MinSpawnFrequency * .75f;
            configuration.MaxSpawnFrequency -= configuration.MaxSpawnFrequency *.25f;

            configuration.MinEnemyRowsSpawnFrequency -= configuration.MinEnemyRowsSpawnFrequency * .75f;
            configuration.MaxEnemyRowsSpawnFrequency -= configuration.MaxEnemyRowsSpawnFrequency * .25f;

            configuration.EnemyRowsConfigurations.ForEach(rowConfiguration => {
                rowConfiguration.EnemyHealth *= 2;
                rowConfiguration.MaxEnemiesToSpawn *= 2;
            });
        }

        var newWaveConfiguration = _enemyWavesConfiguration[waveIndex].GetCopy();
        if (_isHardModeActive) {
            ActivateHardMode(newWaveConfiguration);
        }

        _currentWaveEnemiesCount = newWaveConfiguration.GetTotalEnemies(); 
        _enemiesSpawner.Init(newWaveConfiguration);
        _enemiesSpawner.StartSpawning();
    }
}
