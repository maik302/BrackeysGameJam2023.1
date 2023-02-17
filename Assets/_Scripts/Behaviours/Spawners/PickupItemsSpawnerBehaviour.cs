using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PickupItemsSpawnerBehaviour : MonoBehaviour, ISpawner {

    [SerializeField]
    Transform _spawnerPosition;    

    [Header("Items Prefabs")]
    [SerializeField]
    GameObject _maxHealthItemPrefab;
    [SerializeField]
    GameObject _powerUpItemPrefab;
    [SerializeField]
    GameObject _healthPrefab;

    [Header("Boundaries")]
    [SerializeField]
    Transform _leftBoundaryTransform;
    [SerializeField]
    Transform _rightBoundaryTransform;
    [SerializeField]
    float _offestFromBoundaries = 1f;
    
    int _spawnedItems;
    float _previousItemSpawnedXPos;
    PickupItemsSpawnConfiguration _currentConfiguration;

    public void Init(PickupItemsSpawnConfiguration configuration) {
        _currentConfiguration = configuration;
        ClearSpawnedItemsCount();
    }

    public void StartSpawning() {
        if (_currentConfiguration == null) {
            throw new NullReferenceException("Current configuration for PickupItemsSpawner is empty");
        }

        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn() {
        // Returns an "Id" for the item to spawn, this Id corresponds to the available items to spawn definition order in the inspector
        int GetItemToSpawnId() {
            var itemProbabilityValue = UnityEngine.Random.Range(0f, 1f);
            var probabilityIndex = 0;
            var itemsAvailableToSpawn = _currentConfiguration.GetProbabilitiesAsList().Aggregate(new List<int>(), (acc, probability) => {
                if (itemProbabilityValue < probability) {
                    acc.Add(probabilityIndex);
                }
                probabilityIndex++;

                return acc;
            });
            var itemToSpawn = UnityEngine.Random.Range(0, itemsAvailableToSpawn.Count - 1);  

            return itemsAvailableToSpawn[itemToSpawn];
        }

        var secondsForNextSpawn = UnityEngine.Random.Range(_currentConfiguration.MinSpawnFrequency, _currentConfiguration.MaxSpawnFrequency);

        while (_spawnedItems < _currentConfiguration.MaxItemsToSpawn) {
            yield return new WaitForSeconds(secondsForNextSpawn);

            switch (GetItemToSpawnId()) {
                case 0:
                    InstantiateMaxHealthItem();
                    break;
                case 1:
                    InstantiatePowerUpItem();
                    break;
                case 2:
                    InstantiateHealthItem();
                    break;
            }
        }
    }

    public void InstantiateMaxHealthItem() => InstantiateAnItem(_maxHealthItemPrefab);

    public void InstantiatePowerUpItem() => InstantiateAnItem(_powerUpItemPrefab);

    public void InstantiateHealthItem() => InstantiateAnItem(_healthPrefab);

    void InstantiateAnItem(GameObject itemPrefab) {
        var spawnXPos = GetRandomBoundedSpawnPosX();
        Instantiate(itemPrefab, new Vector2(spawnXPos, _spawnerPosition.position.y), Quaternion.identity);
        _previousItemSpawnedXPos = spawnXPos;

        _spawnedItems++;
    }

    float GetRandomBoundedSpawnPosX() {
        float spawnXPos;
        
        do {
            spawnXPos = UnityEngine.Random.Range(_leftBoundaryTransform.position.x + _offestFromBoundaries, _rightBoundaryTransform.position.x - _offestFromBoundaries);
        } while (spawnXPos == _previousItemSpawnedXPos);
        
        _previousItemSpawnedXPos = spawnXPos;

        return spawnXPos;
    }

    public void ClearSpawnedItemsCount() {
        _spawnedItems = 0;
    }

    public void DestroyAllItems() {
        var activePickupItems = GameObject.FindGameObjectsWithTag(GameTags.PickupItemTag);
        if (activePickupItems != null) {
            foreach (var activePickupItem in activePickupItems) {
                Destroy(activePickupItem);
            }
        }

        ClearSpawnedItemsCount();
    }
}
