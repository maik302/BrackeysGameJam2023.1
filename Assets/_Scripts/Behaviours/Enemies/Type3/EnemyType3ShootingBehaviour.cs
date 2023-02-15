using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3ShootingBehaviour : MonoBehaviour {

    [SerializeField]
    int _projectilesToShoot = 10;
    [SerializeField]
    float _shootFrequency = 3f;
    [SerializeField]
    [Range(0f, 1f)]
    float _singleShootFrequency = .1f;

    EnemyProjectilesPoolBehaviour _projectilesPool;
    Transform _shooterTransform;

    void Awake() {
        _shooterTransform = GetShooterTransform();
    }

    Transform GetShooterTransform() {
        foreach (Transform child in this.transform.GetComponentsInChildren<Transform>()) {
            if (child.CompareTag(GameTags.ShooterTag)) {
                return child;
            }
        }

        return null;
    }

    // Start is called before the first frame update
    void Start() {
        _projectilesPool = GameObject.FindWithTag(GameTags.EnemyProjectilesPoolTag)?.GetComponent<EnemyProjectilesPoolBehaviour>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    IEnumerator Shoot() {
        while (_shooterTransform != null) {
            StartCoroutine(ShootMultipleTimes());
            yield return new WaitForSeconds(_shootFrequency);
        }
    }

    IEnumerator ShootMultipleTimes() {
        for (int i = 0; i < _projectilesToShoot; i++) {
            _projectilesPool.GetProjectileInstance(_shooterTransform);
            yield return new WaitForSeconds(_singleShootFrequency);
        }
    }

    public void StartShooting() {
        StartCoroutine(Shoot());
    }
}
