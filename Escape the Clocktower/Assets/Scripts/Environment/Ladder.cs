using UnityEngine;

// trigger volume in front of a ladder. while the player is inside, W/S climbs instead of walking
public class Ladder : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var p = other.GetComponent<PlayerController>();
        if (p != null) p.onLadder = true;
    }

    void OnTriggerExit(Collider other)
    {
        var p = other.GetComponent<PlayerController>();
        if (p != null) p.onLadder = false;
    }
}
