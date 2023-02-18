using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyRowSpawnConfiguration {

    [SerializeField]
    public EnemyTypes EnemyType;
    [SerializeField]
    public int EnemyHealth;
    [SerializeField]
    public int MaxEnemiesToSpawn;

    public EnemyRowSpawnConfiguration(EnemyTypes enemyType, int enemyHealth, int maxEnemiesToSpawn) {
        EnemyType = enemyType;
        EnemyHealth = enemyHealth;
        MaxEnemiesToSpawn = maxEnemiesToSpawn;
    }

    public EnemyRowSpawnConfiguration GetCopy() {
        return new EnemyRowSpawnConfiguration(
            this.EnemyType,
            this.EnemyHealth,
            this.MaxEnemiesToSpawn
        );
    }
}
