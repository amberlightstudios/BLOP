using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    
    [SerializeField]
    float rotateSpeed = 0.1f;

    public int blocksNum = 10;
    public Vector3 blockGenerateLoc = new Vector3(0, 3, 0);
    public BlockController[] blocks;
    [HideInInspector]
    public BlockController currentBlock;

    // Start is called before the first frame update
    void Awake()
    {
        SwitchToNextBlock();
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

        #if DEBUG
        if (Input.GetKeyDown(KeyCode.C)) {
            foreach(GameObject block in GameObject.FindGameObjectsWithTag("Block")) {
                Destroy(block);
            }
            currentBlock = null;
            SwitchToNextBlock();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            blocksNum += 10;
            SwitchToNextBlock();
        }
        #endif
    }

    int lastIndex;
    public void SwitchToNextBlock() {
        if (blocksNum <= 0)
            return;

        if (currentBlock == null || !currentBlock.isSuspending) {
            int curBlockIndex = Random.Range(0, blocks.Length - 1);
            if (curBlockIndex == lastIndex)
                curBlockIndex = Random.Range(0, blocks.Length - 1);
            lastIndex = curBlockIndex;
            currentBlock = Instantiate(blocks[curBlockIndex], blockGenerateLoc, Quaternion.identity);
            blocksNum--;
        }
    }
}
