using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupItemsSpawnConfiguration {

    [Header("Spawn configuration")]
    [SerializeField]
    public int MaxItemsToSpawn;
    [SerializeField]
    [Tooltip("Min frequency in seconds that an item can be spawned")]
    public float MinSpawnFrequency;
    [SerializeField]
    [Tooltip("Max frequency in seconds that an item can be spawned")]
    public float MaxSpawnFrequency;

    [Header("Items' probability of spawning")]
    [SerializeField]
    [Range(0f, 1f)]
    public float ProbabilityForMaxHealth;
    [SerializeField]
    [Range(0f, 1f)]
    public float ProbabilityForPowerUp;
    [SerializeField]
    [Range(0f, 1f)]
    public float ProbabilityForHealth;

    public PickupItemsSpawnConfiguration(int maxItemsToSpawn, float minSpawnFrequency, float maxSpawnFrequency, float probabilityForMaxHealth, float probabilityForPowerUp, float probabilityForHealth)
    {
        MaxItemsToSpawn = maxItemsToSpawn;
        MinSpawnFrequency = minSpawnFrequency;
        MaxSpawnFrequency = maxSpawnFrequency;
        ProbabilityForMaxHealth = probabilityForMaxHealth;
        ProbabilityForPowerUp = probabilityForPowerUp;
        ProbabilityForHealth = probabilityForHealth;
    }

    public List<float> GetProbabilitiesAsList() {
        var probabilitiesList = new List<float>();
        probabilitiesList.Add(ProbabilityForMaxHealth);
        probabilitiesList.Add(ProbabilityForPowerUp);
        probabilitiesList.Add(ProbabilityForHealth);

        return probabilitiesList;
    }

    public PickupItemsSpawnConfiguration GetCopy() {
        return new PickupItemsSpawnConfiguration(
            this.MaxItemsToSpawn,
            this.MinSpawnFrequency,
            this.MaxSpawnFrequency,
            this.ProbabilityForMaxHealth,
            this.ProbabilityForPowerUp,
            this.ProbabilityForHealth
        );
    }

}
