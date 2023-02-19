using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPickupBehaviour : PickupBehaviour {

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    protected override void OnPickedUpByPlayer() {
        AudioManager.Instance.Play(AudioNames.PickMaxHealthSFX);
        Messenger.Broadcast(GameEvents.MaxHealthPickupGrabbedEvent);
        Destroy(gameObject);
    }
}
