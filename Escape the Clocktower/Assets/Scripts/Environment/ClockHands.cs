using UnityEngine;

// Rotates the hands on a clock face. Each hand needs its pivot at the center of the clock
// (put the hand mesh inside an empty object sitting on the center).
// Hands spin around their local Z axis, and the front of the clock is the -Z side.
public class ClockHands : MonoBehaviour
{
    public Transform hourHand;
    public Transform minuteHand;
    public Transform secondHand;

    // game starts at 11:45 PM
    public int startHour = 11;
    public int startMinute = 45;

    // 1 = real time, 0 = stopped clock. bump it up for the menu clock if you want
    public float timeScale = 1f;

    // if true the clock shows the game timer instead (so penalties move the hands too)
    public bool followGameTimer = false;

    float seconds;

    void Start()
    {
        seconds = 0;
        UpdateHands();
    }

    void Update()
    {
        if (followGameTimer && GameManager.Instance != null)
            seconds = GameManager.Instance.Elapsed;
        else
            seconds += Time.deltaTime * timeScale;

        UpdateHands();
    }

    void UpdateHands()
    {
        float total = (startHour % 12) * 3600f + startMinute * 60f + seconds;

        float s = total % 60f;
        float m = (total / 60f) % 60f;
        float h = (total / 3600f) % 12f;

        // negative so they go clockwise when looking at the face
        if (secondHand != null) secondHand.localRotation = Quaternion.Euler(0, 0, -s * 6f);
        if (minuteHand != null) minuteHand.localRotation = Quaternion.Euler(0, 0, -m * 6f);
        if (hourHand != null) hourHand.localRotation = Quaternion.Euler(0, 0, -h * 30f);
    }
}
