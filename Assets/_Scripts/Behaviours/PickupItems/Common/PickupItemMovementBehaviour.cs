using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemMovementBehaviour : MonoBehaviour {

    [SerializeField]
    float _speed = 1f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Move(Vector2.down);
    }

    void Move(Vector2 direction) {
        transform.position = (Vector2) transform.position + (direction * _speed * Time.deltaTime);
    }
}
