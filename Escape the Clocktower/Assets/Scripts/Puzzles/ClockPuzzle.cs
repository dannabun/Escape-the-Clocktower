using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Room 1: turn the grandfather clock's hands to the right time and the case opens.
// E moves the hands forward 15 minutes, Q moves them back.
public class ClockPuzzle : Interactable
{
    public Transform hourHand;
    public Transform minuteHand;
    // the grandfather clock model's hand bones spin around their local Y.
    // Y = 0 points the hand at 12, positive Y goes counter clockwise
    public Vector3 zeroRotation = new Vector3(90f, 0f, 0f);
    public float direction = -1f;               // flip to 1 if the hands go the wrong way

    public int startMinutes = 0;     // what the clock shows at the start (0 = 12:00)
    public int targetMinutes = 105;  // 1:45
    public int step = 15;

    public GameObject[] revealOnSolve;
    public string solvedMessage = "The clock chimes and the case springs open. Something clatters to the floor.";

    int current;
    bool solved;
    bool turning;

    void Start()
    {
        current = startMinutes;
        SetHands(current);
        foreach (var g in revealOnSolve)
            if (g != null) g.SetActive(false);
    }

    public override string GetPrompt(PlayerInteract player)
    {
        if (solved) return "";
        return "Turn the hands forward    [Q] back";
    }

    public override void Interact(PlayerInteract player)
    {
        Turn(step);
    }

    void Update()
    {
        if (solved || turning || !GameManager.CanPlay) return;
        if (PlayerInteract.Current == this && Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
            Turn(-step);
    }

    void Turn(int delta)
    {
        if (turning || solved) return;
        StartCoroutine(TurnHands(delta));
    }

    IEnumerator TurnHands(int delta)
    {
        turning = true;
        SFX.Play("tick", transform.position, 0.8f);

        float from = current;
        float to = current + delta;
        float t = 0;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            SetHands(Mathf.Lerp(from, to, t / 0.25f));
            yield return null;
        }

        current = (((int)to % 720) + 720) % 720;
        SetHands(current);
        turning = false;

        if (current == targetMinutes) StartCoroutine(Solve());
        else HUD.Instance.ShowMessage("The clock reads " + Format(current) + ".", 1.5f);
    }

    // minutes past 12:00 -> hand angles
    void SetHands(float minutes)
    {
        minuteHand.localRotation = Quaternion.Euler(zeroRotation.x, zeroRotation.y + direction * minutes * 6f, zeroRotation.z);
        hourHand.localRotation = Quaternion.Euler(zeroRotation.x, zeroRotation.y + direction * minutes * 0.5f, zeroRotation.z);
    }

    IEnumerator Solve()
    {
        solved = true;
        yield return new WaitForSeconds(0.3f);
        SFX.Play("chime", transform.position);
        yield return new WaitForSeconds(1.2f);
        SFX.Play("unlock", transform.position);
        foreach (var g in revealOnSolve)
            if (g != null) g.SetActive(true);
        HUD.Instance.ShowMessage(solvedMessage, 4f);
    }

    static string Format(int minutes)
    {
        int h = (minutes / 60) % 12;
        if (h == 0) h = 12;
        return h + ":" + (minutes % 60).ToString("00");
    }
}
