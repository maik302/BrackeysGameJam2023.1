using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

    [SerializeField]
    float _speed = 5f;

    Action<GameObject, Collider2D> _collisionAction;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        Move();
    }

    void Move() {
        var rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody != null) {
            rigidBody.AddForce(transform.up * _speed, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        _collisionAction(gameObject, collider);
    }

    public void Init(Action<GameObject, Collider2D> collisionAction) {
        _collisionAction = collisionAction;
    }

    public void ReleaseObject() {
        _collisionAction(gameObject, null);
    }
}
