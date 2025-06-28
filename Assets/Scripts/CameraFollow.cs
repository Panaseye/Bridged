using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] private float followSpeed = 5f;
    
    [Header("Rotation Settings")]
    [SerializeField] private bool lockRotation = true;
    [SerializeField] private Vector3 lockedRotation = new Vector3(15, 0, 0); // Default camera angle
    
    private Vector3 targetPosition;
    
    void Start()
    {
        // If no target is assigned, try to find the player
        if (target == null)
        {
            playerMovement player = FindObjectOfType<playerMovement>();
            if (player != null)
            {
                target = player.transform;
            }
        }
        
        // Set initial rotation if locking rotation
        if (lockRotation)
        {
            transform.rotation = Quaternion.Euler(lockedRotation);
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Calculate target position
        targetPosition = target.position + offset;
        
        // Smoothly move camera to target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        // Keep rotation locked if enabled
        if (lockRotation)
        {
            transform.rotation = Quaternion.Euler(lockedRotation);
        }
    }
    
    // Public method to change the locked rotation
    public void SetLockedRotation(Vector3 newRotation)
    {
        lockedRotation = newRotation;
        if (lockRotation)
        {
            transform.rotation = Quaternion.Euler(lockedRotation);
        }
    }
    
    // Public method to toggle rotation lock
    public void ToggleRotationLock(bool lockRot)
    {
        lockRotation = lockRot;
        if (lockRotation)
        {
            transform.rotation = Quaternion.Euler(lockedRotation);
        }
    }
} 