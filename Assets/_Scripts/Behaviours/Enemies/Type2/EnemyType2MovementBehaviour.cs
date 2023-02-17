using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2MovementBehaviour : MonoBehaviour {
    private const float InBoundSpeedModifier = .25f;

    [SerializeField]
    float _speed = 5f;
    [SerializeField]
    SpriteRenderer _playerSpriteRenderer;

    float _spriteMiddlePoint;
    float _movementChanges;
    Transform _leftBoundary;
    Transform _rightBoundary;
    Vector2 _leftBoundaryWithOffset;
    Vector2 _rightBoundaryWithOffset;
    Transform _playerTransform;
    Vector2 _initialPosition;

    public void Init(Transform leftBoundary, Transform rightBoundary) {
        _leftBoundary = leftBoundary;
        _rightBoundary = rightBoundary;
        _leftBoundaryWithOffset = new Vector2(_leftBoundary.position.x + _spriteMiddlePoint, transform.position.y);
        _rightBoundaryWithOffset = new Vector2(_rightBoundary.position.x - _spriteMiddlePoint, transform.position.y);
        
        _initialPosition = new Vector2(transform.position.x, transform.position.y);
    }

    void Awake() {
        // Get the middle point of the square-shapped sprite of this GameObject
        _spriteMiddlePoint = _playerSpriteRenderer.bounds.size.x * .5f;
        _movementChanges = 0f;
    }

    // Start is called before the first frame update
    void Start() {
        _playerTransform = GameObject.FindWithTag(GameTags.PlayerTag)?.transform;
    }

    // Update is called once per frame
    void Update() {
        Move();
        LookAtPlayer();
    }

    void Move() {
        void MoveIntoBoundaries() {
           transform.position = ((Vector2) transform.position) + (((_initialPosition.x <= 0) ? Vector2.right : Vector2.left) * _speed * Time.deltaTime); 
        }

        void MoveInsideBoundaries() {
            // Sets the initial value for the movement changes t-value when entering the game's boundaries
            if (_movementChanges == 0f) {
                // Starts the t-value for the movement LERP at this object's travelled distance from the left boundary
                _movementChanges = Vector2.Distance(_leftBoundaryWithOffset, transform.position) / Vector2.Distance(_leftBoundaryWithOffset, _rightBoundaryWithOffset);
            }

            _movementChanges += _speed * Time.deltaTime * InBoundSpeedModifier;
            transform.position = Vector2.Lerp(
                _leftBoundaryWithOffset,
                _rightBoundaryWithOffset,
                Mathf.PingPong(_movementChanges, 1f)
            );
        }

        if ((_initialPosition.x <= 0 && transform.position.x < _leftBoundaryWithOffset.x) ||
            (_initialPosition.x > 0 && transform.position.x > _rightBoundaryWithOffset.x)) {
            MoveIntoBoundaries();
        } else {
            MoveInsideBoundaries();
        }
    }

    void LookAtPlayer() {
        if (_playerTransform != null) {
            transform.up = _playerTransform.position - transform.position;
        }
    }
}
