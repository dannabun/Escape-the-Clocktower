using UnityEngine;

// makes a light flicker a little, for lamps and candles
[RequireComponent(typeof(Light))]
public class FlickerLight : MonoBehaviour
{
    public float amount = 0.25f;
    public float speed = 6f;

    Light lt;
    float baseIntensity;
    float seed;

    void Start()
    {
        lt = GetComponent<Light>();
        baseIntensity = lt.intensity;
        seed = Random.Range(0f, 100f);
    }

    void Update()
    {
        float n = Mathf.PerlinNoise(seed, Time.time * speed);
        lt.intensity = baseIntensity * (1f - amount + n * amount * 2f);
    }
}
