using System.Collections;
using UnityEngine;

// The tower's clockwork. Anything that should only move once the player fixes the jam
// reads ClockMechanism.Power (0 = stopped, 1 = fully running).
public class ClockMechanism : MonoBehaviour
{
    public static float Power = 1f;   // stays 1 in scenes without a mechanism
    public static ClockMechanism Instance;

    public bool startRunning = false;
    public float spinUpTime = 5f;
    public AudioSource runningLoop;   // optional machine hum, fades in with power

    bool running;
    public bool IsRunning { get { return running; } }

    void Awake()
    {
        Instance = this;
        running = startRunning;
        Power = startRunning ? 1f : 0f;
        if (runningLoop != null) runningLoop.volume = 0f;
    }

    void OnDestroy()
    {
        if (Instance == this) Power = 1f;
    }

    public void StartMechanism()
    {
        if (running) return;
        running = true;
        StartCoroutine(SpinUp());
    }

    IEnumerator SpinUp()
    {
        SFX.Play("unlock", transform.position);
        PlayerController.Shake(0.15f, 1.5f);
        if (runningLoop != null && !runningLoop.isPlaying) runningLoop.Play();

        float t = 0;
        while (t < spinUpTime)
        {
            t += Time.deltaTime;
            Power = Mathf.SmoothStep(0f, 1f, t / spinUpTime);
            if (runningLoop != null) runningLoop.volume = Power * 0.6f;
            yield return null;
        }
        Power = 1f;
        SFX.Play("chime", transform.position, 0.8f);
    }
}
