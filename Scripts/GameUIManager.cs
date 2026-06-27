using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    public GameObject pausePanel;
    public GameObject respawnPanel;
    public GameObject gameOverPanel;

    private PlayerHealth playerHealth;

    private bool paused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Don't allow pause while another menu is open
            if ((respawnPanel != null && respawnPanel.activeSelf) ||
                (gameOverPanel != null && gameOverPanel.activeSelf))
                return;

            if (paused)
                Resume();
            else
                Pause();
        }
    }

    public void SetPlayer(PlayerHealth player)
    {
        playerHealth = player;
    }

    void Pause()
    {
        paused = true;

        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void Resume()
    {
        paused = false;

        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void ShowRespawn()
    {
        Time.timeScale = 0f;

        if (respawnPanel != null)
            respawnPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void Respawn()
    {
        Time.timeScale = 1f;

        if (respawnPanel != null)
            respawnPanel.SetActive(false);

        if (playerHealth != null)
            playerHealth.RespawnFromCheckpoint();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        paused = false;

        SeedManager.worldLevel = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}