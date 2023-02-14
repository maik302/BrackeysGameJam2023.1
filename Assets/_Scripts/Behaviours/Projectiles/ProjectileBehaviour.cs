using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

    [SerializeField]
    float _speed = 5f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        MoveForward();
    }

    void MoveForward() {
        var rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody != null) {
            rigidBody.AddForce(transform.up * _speed, ForceMode2D.Impulse);
        }
    }
}
