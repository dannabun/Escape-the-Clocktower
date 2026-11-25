using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    public Text timerText;
    public Text timeLeftText;
    public Text promptText;
    public Text messageText;
    public Text inventoryText;

    public GameObject notePanel;
    public Text noteTitle;
    public Text noteBody;

    bool noteOpen;
    int closedFrame = -1;
    int openedFrame = -1;
    float messageTimer;

    // true while reading a note (and the frame it closes) so E/Esc don't do two things at once
    public bool BlockingInput { get { return noteOpen || closedFrame == Time.frameCount; } }

    void Awake()
    {
        Instance = this;
        if (notePanel != null) notePanel.SetActive(false);
        SetPrompt("");
        SetInventory("");
        if (messageText != null) messageText.text = "";
    }

    void Update()
    {
        if (noteOpen && openedFrame != Time.frameCount)
        {
            var kb = Keyboard.current;
            if (kb != null && (kb.eKey.wasPressedThisFrame || kb.escapeKey.wasPressedThisFrame))
                CloseNote();
        }

        if (messageTimer > 0)
        {
            messageTimer -= Time.unscaledDeltaTime;
            var c = messageText.color;
            c.a = Mathf.Clamp01(messageTimer);
            messageText.color = c;
        }
    }

    public void SetPrompt(string p)
    {
        if (promptText == null) return;
        promptText.text = p == "" ? "" : "[E] " + p;
    }

    public void SetInventory(string s)
    {
        if (inventoryText != null) inventoryText.text = s;
    }

    public void ShowMessage(string msg, float time = 3f)
    {
        if (messageText == null) return;
        messageText.text = msg;
        messageTimer = time;
        var c = messageText.color;
        c.a = 1;
        messageText.color = c;
    }

    public void ShowNote(string title, string body)
    {
        noteOpen = true;
        openedFrame = Time.frameCount;
        noteTitle.text = title;
        noteBody.text = body;
        notePanel.SetActive(true);
        SetPrompt("");
        if (GameManager.Instance != null && GameManager.Instance.player != null)
            GameManager.Instance.player.canMove = false;
    }

    public void CloseNote()
    {
        noteOpen = false;
        closedFrame = Time.frameCount;
        notePanel.SetActive(false);
        if (GameManager.Instance != null && GameManager.Instance.player != null)
            SFX.Play("note_close", GameManager.Instance.player.transform.position, 0.6f);
        if (GameManager.Instance != null && GameManager.Instance.player != null && GameManager.CanPlay)
            GameManager.Instance.player.canMove = true;
    }

    public void SetTime(float elapsed, float left)
    {
        if (timerText != null)
        {
            timerText.text = FormatClock(elapsed);
            // goes red for the last 2 minutes
            timerText.color = left < 120f ? new Color(1f, 0.3f, 0.25f) : new Color(0.95f, 0.85f, 0.6f);
        }
        if (timeLeftText != null)
            timeLeftText.text = FormatLeft(left) + " until midnight";
    }

    // seconds since 11:45 PM -> "11:52 PM"
    public static string FormatClock(float elapsed)
    {
        int total = 23 * 3600 + 45 * 60 + Mathf.FloorToInt(elapsed);
        int h = (total / 3600) % 24;
        int m = (total / 60) % 60;
        string ampm = h >= 12 ? "PM" : "AM";
        int h12 = h % 12;
        if (h12 == 0) h12 = 12;
        if (h == 0) ampm = "AM";
        return h12 + ":" + m.ToString("00") + " " + ampm;
    }

    public static string FormatLeft(float left)
    {
        int s = Mathf.CeilToInt(Mathf.Max(0, left));
        return (s / 60).ToString("00") + ":" + (s % 60).ToString("00");
    }
}
