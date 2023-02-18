using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemiesSpawnerBehaviour : MonoBehaviour, ISpawner {

    private const float InvalidSpawnDescriptorIndex =-1f;

    [Header("Spawners transfroms")]
    [SerializeField]
    Transform _topSpawner;
    [SerializeField]
    Transform _leftSpawner;
    [SerializeField]
    Transform _rightSpawner;

    [Header("Enemies configurations")]
    [Header("Enemy Type 1")]
    [SerializeField]
    GameObject _enemyType1Prefab;
    [SerializeField]
    Transform _enemyType1BottomMovementBoundary;
    [Header("Enemy Type 2")]
    [SerializeField]
    GameObject _enemyType2Prefab;
    [SerializeField]
    Transform _enemyType2BottomSpawnPointBoundary;
    [Header("Enemy Type 3")]
    [SerializeField]
    GameObject _enemyType3Prefab;
    [SerializeField]
    Transform _enemyType3BottomMovementBoundary;

    [Header("Boundaries")]
    [SerializeField]
    Transform _topBoundaryTransform;
    [SerializeField]
    Transform _bottomBoundaryTransform;
    [SerializeField]
    Transform _leftBoundaryTransform;
    [SerializeField]
    Transform _rightBoundaryTransform;
    [SerializeField]
    float _offsetFromBoundaries = 1f;

    EnemiesSpawnConfiguration _currentConfiguration;
    bool[] _occupiedColumns;
    bool[] _occupiedRows;
    int _currentEnemyRowSpawnedEnemiesCount;

    void OnEnable() {
        Messenger<Vector2>.AddListener(GameEvents.EnemySpawningPositionFreedEvent, FreeEnemySpawnPosition);
    }

    void OnDisable() {
        Messenger<Vector2>.RemoveListener(GameEvents.EnemySpawningPositionFreedEvent, FreeEnemySpawnPosition);
    }

    void FreeEnemySpawnPosition(Vector2 spawnPositionDescriptor) {
        var releasedRowIndex = (int) spawnPositionDescriptor.x;
        var releasedColumnIndex = (int) spawnPositionDescriptor.y;

        // A row was released
        if (releasedRowIndex != InvalidSpawnDescriptorIndex) {
            _occupiedRows[releasedRowIndex] = false;
        }
        // A column was released
        else if (releasedColumnIndex != InvalidSpawnDescriptorIndex) {
            _occupiedColumns[releasedColumnIndex] = false;
        }
    }

    public void Init(EnemiesSpawnConfiguration configuration) {
        void InitAvailableSpawningSpaces() {
            var availableSpawningColumns = (int) (Vector2.Distance(_leftBoundaryTransform.position, _rightBoundaryTransform.position) - _offsetFromBoundaries);
            var availableSpawningRows = (int) (Vector2.Distance(_topBoundaryTransform.position, _enemyType2BottomSpawnPointBoundary.position) - _offsetFromBoundaries);

            _occupiedColumns = new bool[availableSpawningColumns];
            _occupiedRows = new bool[availableSpawningRows];
        }

        _currentConfiguration = configuration;
        InitAvailableSpawningSpaces();
    }

    public void StartSpawning() {
        if (_currentConfiguration == null) {
            throw new NullReferenceException("Current configuration for EnemiesSpawner is empty");
        }

        StartCoroutine(Spawn());
    }

    // For spawnning rows of enemies
    IEnumerator Spawn() {
        var enemyRowsConfigurations = _currentConfiguration.EnemyRowsConfigurations;
        while (enemyRowsConfigurations.Count > 0) {
            var currentEnemyRowsConfig = enemyRowsConfigurations[0];
            
            // Spawn enemies
            _currentEnemyRowSpawnedEnemiesCount = 0;
            while (_currentEnemyRowSpawnedEnemiesCount < currentEnemyRowsConfig.MaxEnemiesToSpawn) {
                var secondsForNextEnemySpawn = GetSecondsForNextEnemySpawn(_currentConfiguration);
                yield return new WaitForSeconds(secondsForNextEnemySpawn);

                switch (currentEnemyRowsConfig.EnemyType) {
                    case EnemyTypes.Type1:
                        TryInstantiateEnemyType1(currentEnemyRowsConfig.EnemyHealth);
                        break;
                    case EnemyTypes.Type2:
                        InstantiateEnemyType2();
                        break;
                    case EnemyTypes.Type3:
                        InstantiateEnemyType3();
                        break;
                }
            }
            enemyRowsConfigurations.RemoveAt(0);
        }
    }

    float GetSecondsForNextEnemySpawn(EnemiesSpawnConfiguration configuration) {
        return UnityEngine.Random.Range(configuration.MinSpawnFrequency, configuration.MaxSpawnFrequency);
    }

    void TryInstantiateEnemyType1(int healthPoints) {
        var (spawnColumn, spawnXPos) = GetRandomBoundedUnoccupiedSpawnXAxisSpace();

        // Only spawns an enemy if theres an unoccupied column to do so
        if (spawnColumn >= 0) {
            _occupiedColumns[spawnColumn] = true;

            var enemyInstance = Instantiate(_enemyType1Prefab, new Vector2(spawnXPos, _topSpawner.position.y), Quaternion.Euler(0f, 0f, 180f));
            var enemyType1MovementBehaviour = enemyInstance.transform.GetComponent<EnemyType1MovementBehaviour>();
            var enemyHealtBehaviour = enemyInstance.transform.GetComponent<EnemyHealthBehaviour>();
            if (enemyType1MovementBehaviour != null && enemyHealtBehaviour != null) {
                var randomStopYPos = UnityEngine.Random.Range(_enemyType1BottomMovementBoundary.position.y, _topBoundaryTransform.position.y - _offsetFromBoundaries);
                enemyType1MovementBehaviour.Init(randomStopYPos);

                enemyHealtBehaviour.Init(healthPoints, new Vector2(InvalidSpawnDescriptorIndex, spawnColumn));
            }

            _currentEnemyRowSpawnedEnemiesCount++;
        }
    }

    // Returns a pair that reads as (selected now-occupied column to spawn an enemy in, spawn world position)
    (int, float) GetRandomBoundedUnoccupiedSpawnXAxisSpace() {
        var columnIndex = 0;
        var unoccupiedColumns = _occupiedColumns.Aggregate(new List<int>(), (acc, isColumnOccupied) => {
            if (!isColumnOccupied) {
                acc.Add(columnIndex);
            }
            columnIndex++;

            return acc;
        });

        if (unoccupiedColumns.Count == 0) {
            return (-1, -1f);
        } else {
            var columnToSpawnIn = UnityEngine.Random.Range(0, unoccupiedColumns.Count);
            // Displaces (or translates) the column index to the actual world position
            var columnToSpawnInXPos = unoccupiedColumns[columnToSpawnIn] - (_rightBoundaryTransform.position.x) + _offsetFromBoundaries;

            return (unoccupiedColumns[columnToSpawnIn], columnToSpawnInXPos);
        }
    }

    void InstantiateEnemyType2() {
        float GetRandomBoundedSpawnPosY() {
            return UnityEngine.Random.Range((int) (_enemyType2BottomSpawnPointBoundary.position.y + _offsetFromBoundaries), (int) (_topBoundaryTransform.position.y - _offsetFromBoundaries) + 1);
        }

        Transform GetRandomLateralSpawner() {
            var randomBinary = UnityEngine.Random.Range(0,2);
            return (randomBinary == 0) ? _leftSpawner : _rightSpawner;
        }

        var spawner = GetRandomLateralSpawner();
        var spawnYPos = GetRandomBoundedSpawnPosY();

        var enemyInstance = Instantiate(
            _enemyType2Prefab,
            new Vector2(spawner.position.x, spawnYPos),
            Quaternion.Euler(0f, 0f, (spawner.position.x <= 0) ? 90f : -90f)
        );
        var enemyType2MovementBehaviour = enemyInstance.transform.GetComponent<EnemyType2MovementBehaviour>();
        if (enemyType2MovementBehaviour != null) {
            enemyType2MovementBehaviour.Init(_leftBoundaryTransform, _rightBoundaryTransform);
        }
    }

    void InstantiateEnemyType3() {
        // var spawnXPos = GetRandomBoundedUnoccupiedSpawnXAxisSpace();

        // var enemyInstance = Instantiate(_enemyType3Prefab, new Vector2(spawnXPos, _topSpawner.position.y), Quaternion.identity);
        // var enemyType6MovementBehaviour = enemyInstance.transform.GetComponent<EnemyType3MovementBehaviour>();
        // if (enemyType6MovementBehaviour != null) {
        //     enemyType6MovementBehaviour.Init(_enemyType3BottomMovementBoundary);
        // }
    }
}
