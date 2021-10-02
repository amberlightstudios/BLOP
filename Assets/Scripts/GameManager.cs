using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    
    [SerializeField]
    float rotateSpeed = 0.1f;

    public BlockController[] blocks;
    BlockController currentBlock;
    int curBlockIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (BlockController block in blocks) {
            block.gameObject.SetActive(false);
        }
        currentBlock = blocks[curBlockIndex];
        currentBlock.gameObject.SetActive(true);
    }

    Vector2 mouseStart;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currentBlock.Release();
        }
        
        else if (currentBlock.isSuspending) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                currentBlock.transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
            } else if (Input.GetKey(KeyCode.RightArrow)) {
                currentBlock.transform.Rotate(0, 0, -Time.deltaTime * rotateSpeed);
            }
        }

        
    }

    public void SwitchToNextBlock() {
        if (curBlockIndex + 1 < blocks.Length && !currentBlock.isSuspending) {
            currentBlock = blocks[++curBlockIndex];
            currentBlock.gameObject.SetActive(true);
        }
    }
}
