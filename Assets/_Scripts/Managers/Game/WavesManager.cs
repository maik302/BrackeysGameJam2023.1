using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour {

    [Header("Items Waves configuration")]
    [SerializeField]
    PickupItemsSpawnerBehaviour _pickupItemsSpawner;
    [SerializeField]
    List<PickupItemsSpawnConfiguration> _pickupItemsWavesConfiguration;

    // Start is called before the first frame update
    void Start() {
        StartPickupItemsSpawner();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void StartPickupItemsSpawner() {
        // TEST PURPOSES ONLY
        
        _pickupItemsSpawner.Init(_pickupItemsWavesConfiguration[0]);
        _pickupItemsSpawner.StartSpawning();
    }
}
