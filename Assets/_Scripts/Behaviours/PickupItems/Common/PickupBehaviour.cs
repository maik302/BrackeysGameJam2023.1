using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupBehaviour : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(GameTags.PlayerTag)) {
            OnPickedUpByPlayer();
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(GameTags.DownBoundaryTag)) {
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickedUpByPlayer();
}
