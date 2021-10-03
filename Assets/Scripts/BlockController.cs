using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float moveLeftDist = 3f;
    public float moveRightDist = 3f; 
    public float moveSpeed = 0.5f;   
    Vector3 dstLeft, dstRight;

    [SerializeField]
    Sprite blockSprite;
    GameManager gm;
    Rigidbody2D rb;
    Collider2D coll;

    private void Awake() {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        foreach (Transform child in transform)
        {
            if (blockSprite != null)
                child.GetComponent<SpriteRenderer>().sprite = blockSprite;
        }
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<PolygonCollider2D>();
        Suspend();
    }

    private void Start() {
        dstLeft = transform.position - new Vector3(moveLeftDist, 0, 0);
        dstRight = transform.position + new Vector3(moveRightDist, 0, 0);
    }
    
    private void Update() {
        if ((transform.position.x > dstRight.x && rb.velocity.x > 0)
        || (transform.position.x < dstLeft.x && rb.velocity.x < 0)) {
            moveSpeed *= -1;
        }
        if (isSuspending)
            rb.velocity = new Vector2(moveSpeed, 0);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (this.Equals(gm.currentBlock))
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
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void ActivatePreview()
    {
        // Freeze, shrink, and disable collider on block
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
        coll.enabled = false;
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos() {
        try {
            Gizmos.DrawLine(dstLeft, dstRight);
        } catch {
            Vector2 dstLeft = transform.position - new Vector3(moveLeftDist, 0, 0);
            Vector2 dstRight = transform.position + new Vector3(moveRightDist, 0, 0);
            Gizmos.DrawLine(dstLeft, dstRight);
        }
    }
}
