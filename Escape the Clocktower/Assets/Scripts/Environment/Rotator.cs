using UnityEngine;

// keeps something turning around one of its axes (rideable gears, rotating walkways)
// the player gets carried along by PlayerController when standing on it
public class Rotator : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float degreesPerSecond = 10f;
    public bool needsPower = true;

    void Update()
    {
        float p = needsPower ? ClockMechanism.Power : 1f;
        transform.Rotate(axis, degreesPerSecond * p * Time.deltaTime, Space.Self);
    }
}
