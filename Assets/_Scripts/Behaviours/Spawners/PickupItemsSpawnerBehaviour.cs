using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemsSpawnerBehaviour : MonoBehaviour {

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

    [HideInInspector]
    public int MaxItemsToSpawn;
    
    int _spawnedItems;
    float _previousItemSpawnedXPos;

    void Awake() {
        ResetSpawning();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    // TODO: This needs to be in a coroutine
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
            spawnXPos = Random.Range(_leftBoundaryTransform.position.x + _offestFromBoundaries, _rightBoundaryTransform.position.x - _offestFromBoundaries);
        } while (spawnXPos == _previousItemSpawnedXPos);
        
        _previousItemSpawnedXPos = spawnXPos;

        return spawnXPos;
    }

    public void ResetSpawning() {
        _spawnedItems = 0;
    }
}
