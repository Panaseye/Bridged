using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Teleport player to checkpoint
            other.transform.position = CheckpointManager.Instance.CurrentCheckpoint;
            other.transform.rotation = CheckpointManager.Instance.CurrentCheckpointRotation;
            // Optional: Reset velocity, play sound, etc.
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = Vector3.zero;
        }
    }
}
