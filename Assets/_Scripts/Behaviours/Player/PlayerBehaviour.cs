using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour {

    [SerializeField]
    float _speed = 5f;
    [SerializeField]
    Boundaries _boundaries;
    [SerializeField]
    SpriteRenderer _playerSpriteRenderer;

    Vector2 _movementDirection;
    float _spriteMiddlePoint;

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
            Mathf.Clamp(transform.position.x + movementDirection.x, _boundaries.LeftBoundary.position.x + _spriteMiddlePoint, _boundaries.RightBoundary.position.x - _spriteMiddlePoint),
            Mathf.Clamp(transform.position.y + movementDirection.y, _boundaries.DownBoundary.position.y + _spriteMiddlePoint, _boundaries.UpBoundary.position.y - _spriteMiddlePoint)
        );

        transform.position = boundedMovementDirection;
    }

    void OnMove(InputValue value) {
        _movementDirection = value.Get<Vector2>().normalized;
    }
}
