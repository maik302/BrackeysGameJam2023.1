using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilesPoolBehaviour : MonoBehaviour {

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

    void ReleaseProjectileToPool(GameObject projectile) {
        _projectilesPool.Release(projectile);
    }

    public GameObject GetProjectileInstance(Transform shooterTransform) {
        var projectileInstance = _projectilesPool.Get();
        projectileInstance.transform.position = shooterTransform.position;
        projectileInstance.transform.rotation = shooterTransform.rotation;
        
        return projectileInstance;
    }
}
