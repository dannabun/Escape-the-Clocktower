using UnityEngine;
using UnityEngine.InputSystem;

// also holds the button functions for the win/lose screens
public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    bool paused;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.escapeKey.wasPressedThisFrame || kb.pKey.wasPressedThisFrame)
        {
            if (HUD.Instance != null && HUD.Instance.BlockingInput) return;

            if (paused) Resume();
            else if (GameManager.CanPlay) Pause();
        }
    }

    public void Pause()
    {
        paused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        GameManager.Instance.state = GameManager.State.Paused;
        GameManager.Instance.SetPlayerControl(false);
    }

    public void Resume()
    {
        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.state = GameManager.State.Playing;
        GameManager.Instance.SetPlayerControl(true);
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }

    public void MainMenu()
    {
        GameManager.Instance.GoToMenu();
    }
}
