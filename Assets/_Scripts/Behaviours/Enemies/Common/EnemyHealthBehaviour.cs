using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBehaviour : MonoBehaviour {

    [SerializeField]
    int _healthPoints = 3;

    public void TakeDamage(int damagePoints) {
        _healthPoints -= damagePoints;
        if (_healthPoints <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
        // TODO Report that 1 instance of this enemy type has been destroyed
    }

    public void SetHealthPoints(int healthPoints) {
        _healthPoints = healthPoints;
    }
}
