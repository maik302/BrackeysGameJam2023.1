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

    void Awake() {
        ResetSpawning();
    }

    // Start is called before the first frame update
    void Start() {
        InstantiateMaxHealthItem();
    }

    // Update is called once per frame
    void Update() {
        
    }

    // TODO: This needs to be in a coroutine
    public void InstantiateMaxHealthItem() {
        var spawnXPos = Random.Range(_leftBoundaryTransform.position.x + _offestFromBoundaries, _rightBoundaryTransform.position.x - _offestFromBoundaries);
        Instantiate(_maxHealthItemPrefab, new Vector2(spawnXPos, _spawnerPosition.position.y), Quaternion.identity);
    }

    public void ResetSpawning() {
        _spawnedItems = 0;
    }
}
