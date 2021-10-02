using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public GameManager gm;
    public int blockLayer = 6;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == blockLayer) 
            gm.SwitchToNextBlock();
    }
}
