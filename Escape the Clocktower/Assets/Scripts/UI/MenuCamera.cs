using UnityEngine;

// slow drift for the main menu camera so the office doesnt look like a still image
public class MenuCamera : MonoBehaviour
{
    public float swayAngle = 4f;
    public float swaySpeed = 0.15f;
    public float bob = 0.03f;

    Quaternion startRot;
    Vector3 startPos;

    void Start()
    {
        startRot = transform.rotation;
        startPos = transform.position;
    }

    void Update()
    {
        float t = Time.time * swaySpeed;
        transform.rotation = startRot * Quaternion.Euler(Mathf.Sin(t * 0.7f) * swayAngle * 0.3f, Mathf.Sin(t) * swayAngle, 0);
        transform.position = startPos + Vector3.up * Mathf.Sin(t * 1.3f) * bob;
    }
}
