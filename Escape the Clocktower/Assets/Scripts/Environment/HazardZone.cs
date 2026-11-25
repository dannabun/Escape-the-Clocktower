using UnityEngine;

// touching this knocks the player back to a checkpoint (and can cost time)
// needs a trigger collider (and a kinematic rigidbody if it moves)
public class HazardZone : MonoBehaviour
{
    public float timePenalty = 30f;
    public Transform sendTo;                 // leave empty to use the last checkpoint
    public string message = "";              // leave empty for the default message
    public bool needsPower = false;          // pendulum doesn't hurt while it's stopped

    void OnTriggerEnter(Collider other)
    {
        if (needsPower && ClockMechanism.Power < 0.5f) return;
        if (other.GetComponent<PlayerController>() != null && GameManager.Instance != null)
            GameManager.Instance.HitHazard(timePenalty, sendTo, message);
    }
}
