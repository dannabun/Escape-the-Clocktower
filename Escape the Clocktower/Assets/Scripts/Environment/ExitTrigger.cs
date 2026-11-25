using UnityEngine;

// the bell rope at the top. only works once the clockwork is running (the hatch is driven by it)
public class ExitTrigger : MonoBehaviour
{
    public string lockedMessage = "The belfry hatch won't budge. Its chain runs down into the clockwork... and the clockwork is dead.";
    float nextMessage;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null || GameManager.Instance == null) return;

        if (ClockMechanism.Instance != null && !ClockMechanism.Instance.IsRunning)
        {
            if (Time.time > nextMessage && HUD.Instance != null)
            {
                HUD.Instance.ShowMessage(lockedMessage, 4f);
                nextMessage = Time.time + 4f;
            }
            return;
        }
        GameManager.Instance.Win();
    }
}
