using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField]
    Sprite blockSprite;
    [SerializeField]
    GameManager gm;
    
    Rigidbody2D rb;

    private void Awake() {
        foreach (Transform child in transform)
        {
            if (blockSprite != null)
                child.GetComponent<SpriteRenderer>().sprite = blockSprite;
        }
        rb = GetComponent<Rigidbody2D>();
        Suspend();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        gm.SwitchToNextBlock();
    }

    [HideInInspector]
    public bool isSuspending = true;
    void Suspend() 
    {
        isSuspending = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    public void Release() 
    {
        isSuspending = false;
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
