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
}
