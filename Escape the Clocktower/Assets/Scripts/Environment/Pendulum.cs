using UnityEngine;

// swings back and forth around the local Z axis. put it on the pivot at the top
public class Pendulum : MonoBehaviour
{
    public float angle = 25f;
    public float speed = 1.8f;
    public float offset = 0f;
    public bool needsPower = false;

    float phase;

    void Start()
    {
        phase = offset;
    }

    void Update()
    {
        float p = needsPower ? ClockMechanism.Power : 1f;
        phase += Time.deltaTime * speed * p;
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(phase) * angle * p);
    }
}
