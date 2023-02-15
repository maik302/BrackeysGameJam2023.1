using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupBehaviour : PickupBehaviour {

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    protected override void OnPickedUpByPlayer() {
        // TODO: Broadcast message to GameManager and Player tealling that a HealthPickup has been grabbed by the player
        Destroy(gameObject);
    }
}
