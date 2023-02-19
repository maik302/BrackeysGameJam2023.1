using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickupBehaviour : PickupBehaviour {

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    protected override void OnPickedUpByPlayer() {
        AudioManager.Instance.Play(AudioNames.PickPowerUpSFX);
        Messenger.Broadcast(GameEvents.PowerUpPickupGrabbedEvent);
        Destroy(gameObject);
    }
}
