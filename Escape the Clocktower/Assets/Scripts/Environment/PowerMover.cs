using UnityEngine;

// slides/rotates an object from where it starts to an offset as the mechanism powers up.
// used for walkway sections that swing into place when the clockwork starts
public class PowerMover : MonoBehaviour
{
    public Vector3 moveBy;
    public Vector3 rotateBy;

    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }

    void Update()
    {
        float p = ClockMechanism.Power;
        transform.localPosition = startPos + moveBy * p;
        transform.localRotation = startRot * Quaternion.Euler(rotateBy * p);
    }
}
