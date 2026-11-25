using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// runs the 15 minute countdown and handles winning / losing
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum State { Playing, Paused, Won, Lost }
    public State state = State.Playing;

    public float timeLimit = 900f;     // 11:45 -> 12:00
    public float hazardPenalty = 30f;

    public PlayerController player;
    public Transform respawnPoint;

    public GameObject winPanel;
    public Text winText;
    public GameObject losePanel;

    public string menuScene = "MainMenu";

    [Header("Story")]
    public string introTitle = "11:45 PM  -  St. Jude Municipal Clocktower";
    [TextArea(4, 14)] public string introText =
        "My name is Arthur Vance.\n\n" +
        "Three days ago an unsigned letter landed on my desk. Five words: Julian Cross never left the tower. " +
        "Cross built this clock in 1996 and vanished the night it was finished.\n\n" +
        "I broke in tonight to see for myself. I got as far as the study before someone hit me from behind.\n\n" +
        "The door is locked from the outside. My watch says 11:45. Somewhere above me, something enormous is ticking.\n\n" +
        "Whoever did this is counting on the tower to finish the job at midnight.";
    [TextArea(3, 10)] public string endingText =
        "You haul on the bell rope at {0}.\n\n" +
        "The great bell of St. Jude rings out fifteen minutes early. Across town, porch lights flicker on. " +
        "The Midnight Protocol never gets its twelfth chime.\n\n" +
        "Wedged behind the bell frame you find a leather satchel. Inside: blueprints, a council seal, and a ticket stub " +
        "for the 1:45 to Ashford. Julian Cross never left the tower. Now everyone will know why.";

    float timeLeft;
    bool warned;
    bool introDone;
    public float TimeLeft { get { return timeLeft; } }
    public float Elapsed { get { return timeLimit - timeLeft; } }

    public static bool CanPlay { get { return Instance != null && Instance.state == State.Playing; } }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        timeLeft = timeLimit;
    }

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        SetPlayerControl(true);

        // the opening page of Arthur's notebook, shown once at the start
        if (HUD.Instance != null && introText != "")
            HUD.Instance.ShowNote(introTitle, introText);
    }

    void Update()
    {
        if (state != State.Playing) return;

        // the clock doesn't start until you've read the opening page
        if (!introDone)
        {
            if (HUD.Instance != null && HUD.Instance.BlockingInput) return;
            introDone = true;
        }

        timeLeft -= Time.deltaTime;

        // the bell strikes once with five minutes left
        if (!warned && timeLeft <= 300f)
        {
            warned = true;
            if (player != null) SFX.Play("chime", player.transform.position);
            PlayerController.Shake(0.1f, 1.2f);
            if (HUD.Instance != null) HUD.Instance.ShowMessage("Somewhere above you the great bell tolls once. 11:55.", 4f);
        }

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            Lose();
        }

        if (HUD.Instance != null) HUD.Instance.SetTime(Elapsed, timeLeft);
    }

    public void SetPlayerControl(bool on)
    {
        if (player != null) player.canMove = on;
        Cursor.lockState = on ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !on;
    }

    public void HitHazard()
    {
        HitHazard(hazardPenalty, null, "");
    }

    public void HitHazard(float penalty, Transform sendTo, string message)
    {
        if (state != State.Playing) return;

        timeLeft -= penalty;
        if (player != null) SFX.Play("hit", player.transform.position);
        PlayerController.Shake(0.12f, 0.35f);

        if (HUD.Instance != null)
        {
            if (message == "") message = "The blade knocks you back!";
            if (penalty > 0) message += " -" + penalty + " seconds";
            HUD.Instance.ShowMessage(message, 3f);
        }

        Transform target = sendTo != null ? sendTo : respawnPoint;
        if (target != null && player != null)
            player.Teleport(target.position, target.eulerAngles.y);
    }

    public void Win()
    {
        if (state == State.Won || state == State.Lost) return;
        state = State.Won;
        SetPlayerControl(false);
        if (HUD.Instance != null) HUD.Instance.SetPrompt("");

        if (winText != null)
            winText.text = string.Format(endingText, HUD.FormatClock(Elapsed)) +
                           "\n\n" + HUD.FormatLeft(timeLeft) + " left before midnight.";
        if (winPanel != null) winPanel.SetActive(true);
    }

    public void Lose()
    {
        if (state == State.Won || state == State.Lost) return;
        state = State.Lost;
        SetPlayerControl(false);
        if (HUD.Instance != null) HUD.Instance.SetPrompt("");
        if (losePanel != null) losePanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
