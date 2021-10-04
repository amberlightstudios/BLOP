using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public int blocksLayer;
    Collider2D coll;
    private void Start() {
        coll = GetComponent<Collider2D>();
    }

    private void Update() {
        #if DEBUG
        if (Input.GetKeyDown(KeyCode.T)) {
            Camera.main.GetComponent<CamController>().IncreaseCamSize();
        }
        #endif
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == blocksLayer) {
            float speed = other.GetComponent<Rigidbody2D>().velocity.magnitude;
            if (speed < 0.05f && !other.GetComponent<BlockController>().isSuspending) {
                Camera.main.GetComponent<CamController>().IncreaseCamSize();
            }
        }
    }
}
