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
        Messenger.AddListener(GameEvents.MaxHealthPickupGrabbedEvent, IncreaseMaxHealthPoints);
    }

    void OnDisable() {
        Messenger.RemoveListener(GameEvents.HealthPickupGrabbedEvent, RestoreHealth);
        Messenger.RemoveListener(GameEvents.MaxHealthPickupGrabbedEvent, IncreaseMaxHealthPoints);
    }

    void Start() {
        _healthPoints = (_healthPoints > _maxHealthPoints) ? _maxHealthPoints : _healthPoints;
    }

    public void TakeDamage() {
        _healthPoints = ((_healthPoints - 1) <= 0) ? 0 : _healthPoints - 1;
        Messenger.Broadcast(GameEvents.PlayerTookDamageEvent);
        if (_healthPoints == 0) {
            Die();
        }
    }

    void Die() {
        Messenger.Broadcast(GameEvents.PlayerDiedEvent);
        Destroy(gameObject);
    }

    public void RestoreHealth() {
        if (_healthPoints + 1 < _maxHealthPoints) {
            _healthPoints++;
            Messenger<int>.Broadcast(GameEvents.PlayerHealthIncreasedEvent, 1);
        }
    }

    public void IncreaseMaxHealthPoints() {
        _maxHealthPoints += ((_maxHealthPoints + 1) <= _maxHealthAllowed) ? 1 : 0;
        // Every time a Max Health pickup is grabbed by the player, their health will match the new current max health
        if (_healthPoints < _maxHealthPoints) {
            Messenger<int>.Broadcast(GameEvents.PlayerHealthIncreasedEvent, _maxHealthPoints - _healthPoints);
            _healthPoints = _maxHealthPoints;
        }
    }

    public void InitHealthPoints(int maxHealthPoints, int maxHealthAllowed) {
        _maxHealthPoints = maxHealthPoints;
        _healthPoints = maxHealthPoints;
        _maxHealthAllowed = maxHealthAllowed;
    }

}
