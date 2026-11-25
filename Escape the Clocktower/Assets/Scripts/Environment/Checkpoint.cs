using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && GameManager.Instance != null)
            GameManager.Instance.respawnPoint = spawnPoint != null ? spawnPoint : transform;
    }
}
