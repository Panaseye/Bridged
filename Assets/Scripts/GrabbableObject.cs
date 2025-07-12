using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    [Tooltip("Optional: Only fits in sockets with this ID/tag.")]
    public string socketID;

    [HideInInspector] public bool isHeld = false;

    // Optionally: Add events for OnGrabbed, OnReleased, etc.
}
