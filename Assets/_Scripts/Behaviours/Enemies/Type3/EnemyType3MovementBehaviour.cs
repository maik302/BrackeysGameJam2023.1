using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3MovementBehaviour : MonoBehaviour {

    [SerializeField]
    float _movementSpeed = 5f;
    [SerializeField]
    float _rotationSpeed = 5f;
    [SerializeField]
    Boundaries _boundaries;
    [SerializeField]
    SpriteRenderer _playerSpriteRenderer;

    float _spriteMiddlePoint;
    bool _hasStoppedMoving;
    bool _hasStartedShooting;
    EnemyType3ShootingBehaviour _shootingBehaviour;

    void Awake() {
        // Get the middle point of the square-shapped sprite of this GameObject
        _spriteMiddlePoint = _playerSpriteRenderer.bounds.size.x * .5f;
        _hasStoppedMoving = false;
        _hasStartedShooting = false;
        _shootingBehaviour = transform.GetComponent<EnemyType3ShootingBehaviour>();
    }


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (!_hasStoppedMoving) {
            Move(Vector2.down);
        } else {
            Rotate();

            if (!_hasStartedShooting && _shootingBehaviour != null) {
                _shootingBehaviour.StartShooting();
                _hasStartedShooting = true;
            }
        }
    }

    void Move(Vector2 direction) {
        var movementDirection = direction * _movementSpeed * Time.deltaTime;
        var newYPosition = Mathf.Clamp(transform.position.y + movementDirection.y, _boundaries.DownBoundary.position.y + _spriteMiddlePoint, _boundaries.UpBoundary.position.y - _spriteMiddlePoint); 
        var boundedMovementDirection = new Vector2(
            transform.position.x + movementDirection.x,
            newYPosition
        );

        transform.position = boundedMovementDirection;
        _hasStoppedMoving = newYPosition == (_boundaries.DownBoundary.position.y + _spriteMiddlePoint);
    }

    void Rotate() {
        transform.Rotate(Vector3.forward * _rotationSpeed);
    }
}
