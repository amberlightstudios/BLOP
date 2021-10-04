using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 1;

    public GameObject preview;
// TODO: create levels for demo (BACKLOGGED to post-jam plans)
    public Text levelLabel;
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

    CamController cam;
    Timer timer;

    private void Awake() {
        cam = Camera.main.GetComponent<CamController>();
        timer = GameObject.Find("TimerComponent").GetComponent<Timer>();
    }

    private void Start() {
        SwitchToNextBlock();
        // (BACKLOG for post jam): add countdown before init timer 
        // TODO: create blit tutorial before starting
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
        if (Input.GetKeyDown(KeyCode.I)) {
            Destroy(currentBlock.gameObject);
            currentBlock = Instantiate(blocks[0], blockGenerateLoc, Quaternion.identity);
        }
        #endif
    }

    private void FixedUpdate() {
        try {
            if (currentBlock.transform.position.y < cam.btmPos) SwitchToNextBlock();
        } catch {}
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

        UpdateBlocksUI();
    }

    // TODO: make scoring system juicier - refer to notes (level)
    int score = 0;
    int Score() {
        int scoreCur = 0;
        foreach (GameObject block in GameObject.FindGameObjectsWithTag("Block")) {
            if (block.transform.position.y < cam.btmPos) Destroy(block);

            if (!block.GetComponent<BlockController>().isSuspending) {
                scoreCur += 100;
            }
        }
        if (score < scoreCur) {
            score = scoreCur;
            scoreLabel.text = score.ToString();
        }
        return score;
    }

    public void ChangeScore(int changedAmount) {
        score += changedAmount;
        scoreLabel.text = score.ToString();
    }

    // TODO: figure out a fair amount to increase the new level by each turn and time   
    public void UpdateLevelSettings()
    {
        currentLevel++;

        RefillBlocks(5 * currentLevel);
        timer.AddTime(30 * currentLevel);
        levelLabel.text = "Level " + currentLevel;
    }

    public void UpdateBlocksUI()
    {
        if (previewBlock != null)  previewBlock.DestroySelf();
        if (nextBlock != null) {
            previewBlock = Instantiate(nextBlock, preview.transform.position, Quaternion.identity);
            previewBlock.ActivatePreview();
        }
        blocksNumLabel.text = blocksNum.ToString();
    }

    // TODO: game over when blocks fall off (height of blocks lower than last checkpoint)- report results to player
    // TODO: implement transition to game results screen with option to replay
    public void EndGame()
    {
        Time.timeScale = 0;
        Debug.Log("Message! : Your total score was ");
    }

    public void RefillBlocks(int refillNum) {
        blocksNum += refillNum;
    }
}
