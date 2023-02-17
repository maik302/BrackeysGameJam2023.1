using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawnerBehaviour : MonoBehaviour {

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

    // Start is called before the first frame update
    void Start() {
        InstantiateEnemyType2();
        InstantiateEnemyType2();
        InstantiateEnemyType2();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void InstantiateEnemyType1() {
        var spawnXPos = GetRandomBoundedSpawnPosX();
        
        var enemyInstance = Instantiate(_enemyType1Prefab, new Vector2(spawnXPos, _topSpawner.position.y), Quaternion.Euler(0f, 0f, 180f));
        var enemyType1MovementBehaviour = enemyInstance.transform.GetComponent<EnemyType1MovementBehaviour>();
        if (enemyType1MovementBehaviour != null) {
            enemyType1MovementBehaviour.Init(_enemyType1BottomMovementBoundary);
        }
    }

    float GetRandomBoundedSpawnPosX() {
        return Random.Range((int) (_leftBoundaryTransform.position.x + _offsetFromBoundaries), (int) (_rightBoundaryTransform.position.x - _offsetFromBoundaries) + 1);
    }

    void InstantiateEnemyType2() {
        float GetRandomBoundedSpawnPosY() {
            return Random.Range((int) (_enemyType2BottomSpawnPointBoundary.position.y + _offsetFromBoundaries), (int) (_topBoundaryTransform.position.y - _offsetFromBoundaries) + 1);
        }

        Transform GetRandomLateralSpawner() {
            var randomBinary = Random.Range(0,2);
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
}
