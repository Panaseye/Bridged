using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.Instance.SetCheckpoint(gameObject.transform.position);
            CheckpointManager.Instance.SetCheckpointRotation(gameObject.transform.rotation);
            // Optional: Add VFX, SFX, UI feedback here
        }
    }
}
