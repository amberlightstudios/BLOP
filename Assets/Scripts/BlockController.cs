using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Sprite blockSprite;

    private void Awake() {
        foreach (Transform child in transform)
        {
            if (blockSprite != null)
                child.GetComponent<SpriteRenderer>().sprite = blockSprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
