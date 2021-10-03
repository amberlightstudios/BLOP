using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject preview;
    public GameObject hold;
    CamController cam;

    public Text scoreLabel;
    public Text blocksNumLabel;

    [SerializeField]
    float rotateSpeed = 0.1f;

    public int blocksNum = 10;
    public float generateHeight2ScreenH = 0.7f;
    Vector3 blockGenerateLoc { get { return cam.CamRatioHeight(generateHeight2ScreenH); } }
    public BlockController[] blocks;

    [HideInInspector]
    public BlockController currentBlock;
    BlockController nextBlock;
    BlockController previewBlock;

    Timer timer;

    private void Awake() {
        cam = Camera.main.GetComponent<CamController>();
        timer = GameObject.Find("TimerComponent").GetComponent<Timer>();
    }

    private void Start() {
        SwitchToNextBlock();
        // TODO: add countdown before init timer
        timer.StartTimer();
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

        if (nextBlock == null && !currentBlock.isSuspending) timer.StopTimer();

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
        if (Input.GetKeyDown(KeyCode.I)) {
            Destroy(currentBlock.gameObject);
            currentBlock = Instantiate(blocks[0], blockGenerateLoc, Quaternion.identity);
        }
        #endif
    }

    private void FixedUpdate() {
        try {
            if (currentBlock.transform.position.y < cam.btmPos)   SwitchToNextBlock();
        } catch {}
    }

    int Score() {
        int score = 0;
        foreach (GameObject block in GameObject.FindGameObjectsWithTag("Block")) {
            if (block.transform.position.y < cam.btmPos) Destroy(block);

            if (!block.GetComponent<BlockController>().isSuspending) {
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
        blocksNumLabel.text = blocksNum.ToString();
    }

    // TODO: need to fix this bug before UI. maybe this should just be removed
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
