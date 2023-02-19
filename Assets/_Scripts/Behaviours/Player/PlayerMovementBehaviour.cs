using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBehaviour : MonoBehaviour {

    [SerializeField]
    float _speed = 5f;
    [SerializeField]
    SpriteRenderer _playerSpriteRenderer;

    Transform _topBoundary;
    Transform _bottomBoundary;
    Transform _leftBoundary;
    Transform _rightBoundary;
    Vector2 _movementDirection;
    float _spriteMiddlePoint;

    public void InitMovementBoundaries(Transform topBoundary, Transform bottomBoundary, Transform leftBoundary, Transform rightBoundary) {
        _topBoundary = topBoundary;
        _bottomBoundary = bottomBoundary;
        _leftBoundary = leftBoundary;
        _rightBoundary = rightBoundary;
    }

    void Awake() {
        _movementDirection = new Vector2();
        // Get the middle point of the square-shapped sprite of this GameObject
        _spriteMiddlePoint = _playerSpriteRenderer.bounds.size.x * .5f;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Move(_movementDirection);
    }

    void Move(Vector2 direction) {
        var movementDirection = direction * _speed * Time.deltaTime;
        var boundedMovementDirection = new Vector2(
            Mathf.Clamp(transform.position.x + movementDirection.x, _leftBoundary.position.x + _spriteMiddlePoint, _rightBoundary.position.x - _spriteMiddlePoint),
            Mathf.Clamp(transform.position.y + movementDirection.y, _bottomBoundary.position.y + _spriteMiddlePoint, _topBoundary.position.y - _spriteMiddlePoint)
        );

        transform.position = boundedMovementDirection;
    }

    void OnMove(InputValue value) {
        _movementDirection = value.Get<Vector2>().normalized;
    }
}
