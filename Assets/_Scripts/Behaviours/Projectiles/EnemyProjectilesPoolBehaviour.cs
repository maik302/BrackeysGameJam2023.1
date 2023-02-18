using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyProjectilesPoolBehaviour : MonoBehaviour {

    [SerializeField]
    GameObject _projectilePrefab;

    ObjectPool<GameObject> _projectilesPool;

    void Awake() {
        InitProjectilePool();
    }

    void InitProjectilePool() {
        _projectilesPool = new ObjectPool<GameObject>(
            () => {
                var projectile = Instantiate(_projectilePrefab);
                var projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
                if (projectileBehaviour != null) {
                    projectileBehaviour.Init(ReleaseProjectileToPool);
                }

                return projectile;
            },
            projectile => {
                projectile.SetActive(true);
            },
            projectile => {
                projectile.SetActive(false);
            },
            projectile => {
                Destroy(projectile);
            },
            false, 20, 50
        );
    }

    void ReleaseProjectileToPool(GameObject projectile, Collider2D collider) {
        void HandleCollision(Collider2D collider) {
            if (collider.gameObject.CompareTag(GameTags.PlayerTag)) {
                var playerHealthBehaviour = collider.transform.GetComponent<PlayerHealthBehaviour>();
                if (playerHealthBehaviour != null) {
                    playerHealthBehaviour.TakeDamage();
                }
                _projectilesPool.Release(projectile);
            } else if (!collider.gameObject.CompareTag(GameTags.PickupItemTag) && !collider.gameObject.CompareTag(GameTags.EnemyTag)) {
                _projectilesPool.Release(projectile);
            }
        }

        // Only when a collider is met, handle their collision. Otherwise, release this object to its pool
        if (collider != null) {
            HandleCollision(collider);
        } else {
            _projectilesPool.Release(projectile);
        }
    }

    public GameObject GetProjectileInstance(Transform shooterTransform) {
        var projectileInstance = _projectilesPool.Get();
        projectileInstance.transform.position = shooterTransform.position;
        projectileInstance.transform.rotation = shooterTransform.rotation;
        
        return projectileInstance;
    }
}
