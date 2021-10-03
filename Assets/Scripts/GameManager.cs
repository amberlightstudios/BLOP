using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    public GameObject preview;

    [SerializeField]
    float rotateSpeed = 0.1f;

    public int blocksNum = 10;
    public Vector3 blockGenerateLoc = new Vector3(0, 3, 0);
    public BlockController[] blocks;
    [HideInInspector]
    public BlockController currentBlock;
    BlockController nextBlock;
    BlockController previewBlock;

    void Awake()
    {
        SwitchToNextBlock();
    }

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

        #if DEBUG
        if (Input.GetKeyDown(KeyCode.C)) {
            foreach(GameObject block in GameObject.FindGameObjectsWithTag("Block")) {
                Destroy(block);
            }
            currentBlock = null;
            if (blocksNum == 0) blocksNum++;
            SwitchToNextBlock();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            blocksNum += 10;
            SwitchToNextBlock();
        }
        #endif
    }

    int currentIndex;
    public void SwitchToNextBlock() {
        if (blocksNum <= 0) return;

        if (currentBlock == null) {
            currentIndex = Random.Range(0, blocks.Length - 1);
            currentBlock = Instantiate(blocks[currentIndex], blockGenerateLoc, Quaternion.identity);
            if (blocksNum >= 2) {
                int nextBlockIndex = Random.Range(0, blocks.Length - 1);
                if (nextBlockIndex == currentIndex)
                    nextBlockIndex = Random.Range(0, blocks.Length - 1);
                nextBlock = blocks[nextBlockIndex];
            }
            blocksNum--;
        }

        else if (nextBlock != null && !currentBlock.isSuspending) {
            int nextBlockIndex = Random.Range(0, blocks.Length - 1);
            if (nextBlockIndex == currentIndex)
                nextBlockIndex = Random.Range(0, blocks.Length - 1);
            currentBlock = Instantiate(nextBlock, blockGenerateLoc, Quaternion.identity);
            currentIndex = nextBlockIndex;
            if (blocksNum > 1) {
                nextBlock = blocks[nextBlockIndex];
            } else {
                nextBlock = null;
            }
            blocksNum--;
        }

        UpdatePreview();
    }

    public void UpdatePreview()
    {
        if (nextBlock != null) {
            if (previewBlock != null) {
                previewBlock.DestroySelf();
            }
            previewBlock = Instantiate(nextBlock, preview.transform.position, Quaternion.identity);
            previewBlock.ActivatePreview();
        }
    }
}
