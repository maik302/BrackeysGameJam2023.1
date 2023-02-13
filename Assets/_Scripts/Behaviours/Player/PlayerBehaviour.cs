using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour {

    [SerializeField]
    private float _speed = 5f;

    Vector2 _movementDirection;

    void Awake() {
        _movementDirection = new Vector2();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Move(_movementDirection);
    }

    void Move(Vector2 movementDirection) {
        transform.Translate(movementDirection * _speed * Time.deltaTime);
    }

    void OnMove(InputValue value) {
        _movementDirection = value.Get<Vector2>();
    }
}
