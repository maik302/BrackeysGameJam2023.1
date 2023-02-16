using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour {

    // For testing purposes only. This configuration will be made by the GameManager
    [SerializeField]
    int _maxHealthPoints = 3;
    [SerializeField]
    int _healthPoints = 3;

    int _maxHealthAllowed;

    void OnEnable() {
        Messenger.AddListener(GameEvents.HealthPickupGrabbedEvent, RestoreHealth);
    }

    void OnDisable() {
        Messenger.RemoveListener(GameEvents.HealthPickupGrabbedEvent, RestoreHealth);
    }

    public void TakeDamage() {
        _healthPoints--;
        if (_healthPoints <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
        // TODO Report to GameManager that the current round has finished!
    }

    public void RestoreHealth() {
        _healthPoints += ((_healthPoints + 1) < _maxHealthPoints) ? 1 : 0;
    }

    public void IncreaseMaxHealthPoints() {
        _maxHealthPoints += ((_maxHealthPoints + 1) < _maxHealthAllowed) ? 1 : 0;
    }

    public void InitHealthPoints(int maxHealthPoints, int maxHealthAllowed) {
        _maxHealthPoints = maxHealthPoints;
        _healthPoints = maxHealthPoints;
        _maxHealthAllowed = maxHealthAllowed;
    }

}
