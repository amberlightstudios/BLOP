using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject preview;
    public Text scoreLabel;
    public GameObject hold;

    [SerializeField]
    float rotateSpeed = 0.1f;

    public int blocksNum = 10;
    public Vector3 blockGenerateLoc = new Vector3(0, 3, 0);
    public BlockController[] blocks;

    [HideInInspector]
    public BlockController currentBlock;
    BlockController nextBlock;
    BlockController previewBlock;

    CamController cam;

    void Awake()
    {
        SwitchToNextBlock();
    }

    private void Start() {
        cam = Camera.main.GetComponent<CamController>();
        InvokeRepeating(nameof(Score), 0.5f, 1f);
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
        if (Input.GetKeyDown(KeyCode.S)) {
            SwitchCurrentAndNext();
        }
        #endif
    }

    int Score() {
        int score = 0;
        foreach (GameObject block in GameObject.FindGameObjectsWithTag("Block")) {
            if (block.transform.position.y < cam.btmPos) 
                Destroy(block);
            
            if (block.GetComponent<Rigidbody2D>().velocity.magnitude < 0.2f && !block.GetComponent<BlockController>().isSuspending) {
                score++;
            }
        }
        scoreLabel.text = score.ToString();
        return score;
    }

    int currentIndex, nextBlockIndex;
    public void SwitchToNextBlock() {
        if (blocksNum <= 0) return;

        if (currentBlock == null) {
            currentIndex = Random.Range(0, blocks.Length - 1);
            currentBlock = Instantiate(blocks[currentIndex], blockGenerateLoc, Quaternion.identity);
            if (blocksNum >= 2) {
                nextBlockIndex = Random.Range(0, blocks.Length - 1);
                if (nextBlockIndex == currentIndex)
                    nextBlockIndex = Random.Range(0, blocks.Length - 1);
                nextBlock = blocks[nextBlockIndex];
            }
            blocksNum--;
        }

        else if (nextBlock != null && !currentBlock.isSuspending) {
            nextBlockIndex = Random.Range(0, blocks.Length - 1);
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
        if (previewBlock != null)  previewBlock.DestroySelf();
        if (nextBlock != null) {
            previewBlock = Instantiate(nextBlock, preview.transform.position, Quaternion.identity);
            previewBlock.ActivatePreview();
        }
    }

    public void SwitchCurrentAndNext() {
        if (nextBlock == null) return;
        int temp = currentIndex;
        Transform transformCur = currentBlock.transform;
        Destroy(currentBlock.gameObject);
        currentBlock = Instantiate(nextBlock, blockGenerateLoc, Quaternion.identity);
        nextBlock = blocks[currentIndex];
        currentIndex = nextBlockIndex;
        nextBlockIndex = temp;
    }
}
