using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2MovementBehaviour : MonoBehaviour {

    [SerializeField]
    float _speed = 5f;
    [SerializeField]
    Boundaries _boundaries;
    [SerializeField]
    SpriteRenderer _playerSpriteRenderer;

    float _spriteMiddlePoint;
    float _movementChanges;
    Vector2 _leftBoundaryWithOffset, _rightBoundaryWithOffset;

    void Awake() {
        // Get the middle point of the square-shapped sprite of this GameObject
        _spriteMiddlePoint = _playerSpriteRenderer.bounds.size.x * .5f;
        _leftBoundaryWithOffset = new Vector2(_boundaries.LeftBoundary.position.x + _spriteMiddlePoint, transform.position.y);
        _rightBoundaryWithOffset = new Vector2(_boundaries.RightBoundary.position.x - _spriteMiddlePoint, transform.position.y);
    }

    // Start is called before the first frame update
    void Start() {
        // Starts the t-value for the movement LERP at this object's travelled distance from the left boundary
        _movementChanges = Vector2.Distance(_leftBoundaryWithOffset, transform.position) / Vector2.Distance(_leftBoundaryWithOffset, _rightBoundaryWithOffset);
    }

    // Update is called once per frame
    void Update() {
        Move();
    }

    void Move() {
        _movementChanges += _speed * Time.deltaTime;
        transform.position = Vector2.Lerp(
            _leftBoundaryWithOffset,
            _rightBoundaryWithOffset,
            Mathf.PingPong(_movementChanges, 1f)
        );
    }
}
