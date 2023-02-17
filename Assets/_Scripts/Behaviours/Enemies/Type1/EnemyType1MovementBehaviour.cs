using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1MovementBehaviour : MonoBehaviour {

    [SerializeField]
    float _speed = 5f;
    [SerializeField]
    SpriteRenderer _playerSpriteRenderer;

    Transform _bottomBoundary;
    Vector2 _initialPosition;

    float _spriteMiddlePoint;

    public void Init(Transform bottomBoundary) {
        _bottomBoundary = bottomBoundary;
        _initialPosition = new Vector2(transform.position.x, transform.position.y);
    }

    void Awake() {
        // Get the middle point of the square-shapped sprite of this GameObject
        _spriteMiddlePoint = _playerSpriteRenderer.bounds.size.x * .5f;
    }

    // Update is called once per frame
    void Update() {
        Move(Vector2.down);
    }

    void Move(Vector2 direction) {
        var movementDirection = direction * _speed * Time.deltaTime;
        var boundedMovementDirection = new Vector2(
            transform.position.x,
            Mathf.Clamp(transform.position.y + movementDirection.y, _bottomBoundary.position.y + _spriteMiddlePoint, _initialPosition.y)
        );

        transform.position = boundedMovementDirection;
    }
}
