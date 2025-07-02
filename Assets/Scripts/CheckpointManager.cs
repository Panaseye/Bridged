using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    public Vector3 CurrentCheckpoint { get; private set; }
    public Quaternion CurrentCheckpointRotation { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCheckpoint(Vector3 position)
    {
        CurrentCheckpoint = position;
    }

    public void SetCheckpointRotation(Quaternion rotation)
    {
        CurrentCheckpointRotation = rotation;
    }
}