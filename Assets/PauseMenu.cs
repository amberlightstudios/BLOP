using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Slider volSlider;
    GameManager gm;

    private void Awake() {
        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        } 
    }

    public void Resume() {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        gm.endMenu.SetActive(false);
        gameIsPaused = false;
    }

    void Pause() {
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }

    public void RestartLevel()
    {
        Resume();
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void ChangeVolume() {
        AudioListener.volume = volSlider.value;
    }
}
