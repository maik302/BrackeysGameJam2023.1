using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PickupItemsSpawnerBehaviour : MonoBehaviour, ISpawner {

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
    float _offsetFromBoundaries = 1f;
    
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

            if (itemsAvailableToSpawn.Count > 0) {
                var itemToSpawn = UnityEngine.Random.Range(0, itemsAvailableToSpawn.Count);  
                return itemsAvailableToSpawn[itemToSpawn];
            } else {
                return -1;
            }
        }

        while (_spawnedItems < _currentConfiguration.MaxItemsToSpawn) {
            var secondsForNextSpawn = UnityEngine.Random.Range(_currentConfiguration.MinSpawnFrequency, _currentConfiguration.MaxSpawnFrequency);
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
                default:
                    break;
            }
        }
    }

    void InstantiateMaxHealthItem() => InstantiateAnItem(_maxHealthItemPrefab);

    void InstantiatePowerUpItem() => InstantiateAnItem(_powerUpItemPrefab);

    void InstantiateHealthItem() => InstantiateAnItem(_healthPrefab);

    void InstantiateAnItem(GameObject itemPrefab) {
        var spawnXPos = GetRandomBoundedSpawnPosX();
        Instantiate(itemPrefab, new Vector2(spawnXPos, transform.position.y), Quaternion.identity);
        _previousItemSpawnedXPos = spawnXPos;

        _spawnedItems++;
    }

    float GetRandomBoundedSpawnPosX() {
        float spawnXPos;
        
        do {
            spawnXPos = UnityEngine.Random.Range(_leftBoundaryTransform.position.x + _offsetFromBoundaries, _rightBoundaryTransform.position.x - _offsetFromBoundaries);
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
