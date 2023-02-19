using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyType3MovementBehaviour : MonoBehaviour {

    [SerializeField]
    float _movementSpeed = 5f;
    [SerializeField]
    float _rotationSpeed = 5f;
    [SerializeField]
    SpriteRenderer _playerSpriteRenderer;

    Transform _bottomBoundary;
    Vector2 _initialPosition;

    float _spriteMiddlePoint;
    bool _hasStoppedMoving;
    bool _hasStartedShooting;
    EnemyType3ShootingBehaviour _shootingBehaviour;

    public void Init(Transform bottomBoundary) {
        _bottomBoundary = bottomBoundary;
        _initialPosition = new Vector2(transform.position.x, transform.position.y);
    }

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
        var newYPosition = Mathf.Clamp(transform.position.y + movementDirection.y, _bottomBoundary.position.y + _spriteMiddlePoint, _initialPosition.y); 
        var boundedMovementDirection = new Vector2(
            transform.position.x + movementDirection.x,
            newYPosition
        );

        transform.position = boundedMovementDirection;
        _hasStoppedMoving =  Math.Round(newYPosition, 2) == Math.Round((_bottomBoundary.position.y + _spriteMiddlePoint), 2);
    }

    void Rotate() {
        transform.Rotate(Vector3.forward * _rotationSpeed);
    }
}
