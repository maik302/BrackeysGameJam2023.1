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
        AudioManager.Instance.Play(AudioNames.PickHealthSFX);
        Messenger.Broadcast(GameEvents.HealthPickupGrabbedEvent);
        Destroy(gameObject);
    }
}
