using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class EnemiesSpawnConfiguration {

    [Header("Spawn configuration")]
    [SerializeField]
    public int EnemiesInGameThreshold;
    [SerializeField]
    [Tooltip("Min frequency in seconds that an enemy can be spawned")]
    public float MinSpawnFrequency;
    [SerializeField]
    [Tooltip("Max frequency in seconds that an enemy can be spawned")]
    public float MaxSpawnFrequency;
    [SerializeField]
    [Tooltip("Min frequency in seconds in which each enemies row will spawned")]
    public float MinEnemyRowsSpawnFrequency;
    [SerializeField]
    [Tooltip("Max frequency in seconds in which each enemies row will spawned")]
    public float MaxEnemyRowsSpawnFrequency;
    [SerializeField]
    public List<EnemyRowSpawnConfiguration> EnemyRowsConfigurations;

    public EnemiesSpawnConfiguration(int enemiesInGameThreshold, float minSpawnFrequency, float maxSpawnFrequency, float minEnemyRowsSpawnFrequency, float maxEnemyRowsSpawnFrequency, List<EnemyRowSpawnConfiguration> enemyRowsConfigurations)
    {
        EnemiesInGameThreshold = enemiesInGameThreshold;
        MinSpawnFrequency = minSpawnFrequency;
        MaxSpawnFrequency = maxSpawnFrequency;
        MinEnemyRowsSpawnFrequency = minEnemyRowsSpawnFrequency;
        MaxEnemyRowsSpawnFrequency = maxEnemyRowsSpawnFrequency;
        EnemyRowsConfigurations = enemyRowsConfigurations;
    }

    public int GetTotalEnemies() {
        return EnemyRowsConfigurations.Select(row => row.MaxEnemiesToSpawn).Sum();
    }

    public EnemiesSpawnConfiguration GetCopy() {
        var enemyRowsConfigurationsCopy = new List<EnemyRowSpawnConfiguration>();
        this.EnemyRowsConfigurations.ForEach(configuration => {
            enemyRowsConfigurationsCopy.Add(configuration.GetCopy());
        });

        return new EnemiesSpawnConfiguration(
            this.EnemiesInGameThreshold,
            this.MinSpawnFrequency,
            this.MaxSpawnFrequency,
            this.MinEnemyRowsSpawnFrequency,
            this.MaxEnemyRowsSpawnFrequency,
            enemyRowsConfigurationsCopy
        );
    }
}
