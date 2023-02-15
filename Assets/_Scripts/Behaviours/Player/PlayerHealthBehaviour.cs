using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour {

    [SerializeField]
    int _maxHealthPoints = 3;
    [SerializeField]
    int _healthPoints = 3;

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
        _healthPoints += ((_healthPoints + 1) >= _maxHealthPoints) ? 1 : 0;
    }

    public void IncreaseMaxHealthPoints() {
        _maxHealthPoints++;
    }

}
