using UnityEngine;

public class SpinGear : MonoBehaviour
{
    public float speed = 15f;          // degrees per second, negative spins the other way
    public bool needsPower = false;    // only turns when the tower mechanism is running

    void Update()
    {
        float p = needsPower ? ClockMechanism.Power : 1f;
        transform.Rotate(Vector3.up, speed * p * Time.deltaTime, Space.Self);
    }
}
