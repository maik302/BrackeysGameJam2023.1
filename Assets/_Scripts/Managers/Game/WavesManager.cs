using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour {

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

    // Start is called before the first frame update
    void Start() {
        StartEnemiesSpawner();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void StartPickupItemsSpawner() {
        // TEST PURPOSES ONLY
        
        _pickupItemsSpawner.Init(_pickupItemsWavesConfiguration[0]);
        _pickupItemsSpawner.StartSpawning();
    }

    void StartEnemiesSpawner() {
        // TEST PURPOSES ONLY
        
        _enemiesSpawner.Init(_enemyWavesConfiguration[0]);
        _enemiesSpawner.StartSpawning();
    }
}
