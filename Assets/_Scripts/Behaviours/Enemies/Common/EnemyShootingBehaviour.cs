using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingBehaviour : MonoBehaviour {

    [SerializeField]
    float _shootFrequency = 1f;

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
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator Shoot() {
        while (_shooterTransform != null) {
            _projectilesPool.GetProjectileInstance(_shooterTransform);
            yield return new WaitForSeconds(_shootFrequency);
        }
    }
}
