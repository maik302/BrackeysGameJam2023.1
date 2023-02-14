using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingBehaviour : MonoBehaviour {

    [SerializeField]
    GameObject _projectile;

    Transform _shooterTransform;

    void Awake() {
        _shooterTransform = GetShooter();
    }

    Transform GetShooter() {
        foreach (Transform child in this.transform.GetComponentsInChildren<Transform>()) {
            if (child.CompareTag(GameTags.ShooterTag)) {
                return child;
            }
        }

        return null;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnFire() {
        if (_shooterTransform != null) {
            Instantiate(_projectile, _shooterTransform.position, Quaternion.identity);
        }
    }
}
