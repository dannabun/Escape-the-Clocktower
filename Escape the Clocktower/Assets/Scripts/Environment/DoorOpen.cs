using System.Collections;
using UnityEngine;

// Swings a door open around its hinge.
// Put this on an empty "Hinge" object at the edge of the door, with the door mesh as a child.
public class DoorOpen : MonoBehaviour
{
    public bool isLocked = true;
    public float openAngle = 90f;
    public float openTime = 1.2f;
    public AudioSource openSound;

    bool isOpen = false;
    public bool IsOpen { get { return isOpen; } }

    Quaternion closedRot;
    Quaternion openRot;

    void Start()
    {
        closedRot = transform.localRotation;
        openRot = closedRot * Quaternion.Euler(0, openAngle, 0);
    }

    public void Unlock()
    {
        isLocked = false;
    }

    // call this from the interact script / puzzle when the player uses the door
    public void TryOpen()
    {
        if (isLocked || isOpen) return;

        isOpen = true;
        if (openSound != null) openSound.Play();
        else SFX.Play("door_open", transform.position);
        StartCoroutine(Swing());
    }

    // for puzzles that open the door on their own
    public void UnlockAndOpen()
    {
        Unlock();
        TryOpen();
    }

    IEnumerator Swing()
    {
        float t = 0;
        while (t < openTime)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / openTime);
            transform.localRotation = Quaternion.Slerp(closedRot, openRot, p);
            yield return null;
        }
        transform.localRotation = openRot;
    }
}
