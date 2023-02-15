using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerShootingBehaviour : MonoBehaviour {

    [SerializeField]
    GameObject _projectilePrefab;
    [SerializeField]
    int _projectilePower = 1;

    Transform _shooterTransform;
    ObjectPool<GameObject> _projectilesPool;

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
        InitProjectilePool();
    }

    void InitProjectilePool() {
        _projectilesPool = new ObjectPool<GameObject>(
            () => {
                if (_shooterTransform != null) {
                    var projectile = Instantiate(_projectilePrefab, _shooterTransform.position, Quaternion.identity);
                    var projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
                    if (projectileBehaviour != null) {
                        projectileBehaviour.Init(ReleaseProjectileToPool);
                        projectileBehaviour.SetProjectilePower(_projectilePower);
                    }

                    return projectile;
                }
                return null;
            },
            projectile => {
                projectile.SetActive(true);
                projectile.transform.position = _shooterTransform.position;
            },
            projectile => {
                projectile.SetActive(false);
            },
            projectile => {
                Destroy(projectile);
            },
            false, 10, 20
        );
    }

    void ReleaseProjectileToPool(GameObject projectile, Collider2D collider) {
        void HandleCollision(Collider2D collider) {
            if (collider.gameObject.CompareTag(GameTags.EnemyTag)) {
                var enemyHealthBehaviour = collider.transform.GetComponent<EnemyHealthBehaviour>();
                if (enemyHealthBehaviour != null) {
                    enemyHealthBehaviour.TakeDamage(_projectilePower);
                }
                _projectilesPool.Release(projectile);
            } else if (!collider.gameObject.CompareTag(GameTags.PickupItemTag) && !collider.gameObject.CompareTag(GameTags.PlayerTag)) {
                _projectilesPool.Release(projectile);
            }
        }

        HandleCollision(collider);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnFire() {
        _projectilesPool.Get();
    }

    public void IncreaseShootingPower() {
        _projectilePower++;
    }
}
