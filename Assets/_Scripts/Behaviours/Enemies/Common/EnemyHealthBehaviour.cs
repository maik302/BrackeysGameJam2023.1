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
        AudioManager.Instance.Play(AudioNames.EnemyDamageSFX);
        _healthPoints -= damagePoints;
        if (_healthPoints <= 0) {
            Die();
        } else {
            StartCoroutine(ShowFlickerEffect(.1f, 3));
        }
    }

    IEnumerator ShowFlickerEffect(float timeInSeconds, int repeatTimes) {
        var spriteRenderer = gameObject.transform.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null) {
            var repetition = 0;
            while (repetition < repeatTimes) {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(timeInSeconds);
                
                repetition++;
            }
            
            spriteRenderer.enabled = true;
        }
    }

    void Die() {
        AudioManager.Instance.Play(AudioNames.EnemyDeathSFX);
        Messenger<int>.Broadcast(GameEvents.EnemyDestroyedEvent, _baseHealthPoints * _scoreMultiplier);
        Messenger<Vector2>.Broadcast(GameEvents.EnemySpawningPositionFreedEvent, _spawnedPositionDescriptor);
        Destroy(gameObject);
    }
}
