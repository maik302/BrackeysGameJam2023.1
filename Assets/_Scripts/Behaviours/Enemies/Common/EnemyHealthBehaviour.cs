using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBehaviour : MonoBehaviour {

    // For testing purposes only. This configuration will be made by the Enemies spawner
    [SerializeField]
    int _healthPoints = 3;
    [SerializeField]
    int _scoreMultiplier = 100;

    int _baseHealthPoints;
    Vector2 _spawnedPositionDescriptor;

    public void Init(int healthPoints, Vector2 spawnPositionDescriptor) {
        _healthPoints = healthPoints;
        _spawnedPositionDescriptor = spawnPositionDescriptor;
    }

    void Start() {
        _baseHealthPoints = _healthPoints;
    }

    public void TakeDamage(int damagePoints) {
        _healthPoints -= damagePoints;
        if (_healthPoints <= 0) {
            Die();
        }
    }

    void Die() {
        Messenger<int>.Broadcast(GameEvents.EnemyDestroyedEvent, _baseHealthPoints * _scoreMultiplier);
        Messenger<Vector2>.Broadcast(GameEvents.EnemySpawningPositionFreedEvent, _spawnedPositionDescriptor);
        Destroy(gameObject);
    }
}
