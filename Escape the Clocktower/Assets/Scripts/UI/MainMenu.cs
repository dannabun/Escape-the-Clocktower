using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainGroup;
    public GameObject instructionsPanel;
    public string gameScene = "Main";

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShowInstructions(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void OpenInstructions() { ShowInstructions(true); }
    public void CloseInstructions() { ShowInstructions(false); }

    void ShowInstructions(bool show)
    {
        if (instructionsPanel != null) instructionsPanel.SetActive(show);
        if (mainGroup != null) mainGroup.SetActive(!show);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
