using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 normalOffset = new Vector3(0, 10, -10);
    [SerializeField] private Vector3 mazerOffset = new Vector3(0, 10, 0);
    [SerializeField] private Vector3 mazeMasterOffset = new Vector3(0, 5, -10);
    [SerializeField] private float followSpeed = 5f;
    
    [Header("Rotation Settings")]
    [SerializeField] private bool lockRotation = true;
    [SerializeField] private Vector3 normallockedRotation = new Vector3(40, 0, 0); // Default camera angle
    [SerializeField] private Vector3 mazerlockedRotation = new Vector3(90, 0, 0); // Default camera angle
    [SerializeField] private Vector3 mazeMasterlockedRotation = new Vector3(15, 0, 0); // Default camera angle
    private Vector3 targetPosition;

    public bool normalMode;
    public bool mazerMode;
    public bool mazeMasterMode;
    public GameObject mazeMasterCameraPos;
    
    void Start()
    {
        // If no target is assigned, try to find the player
        if (target == null)
        {
            PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                target = player.transform;
            }

            normalMode = true;
            mazerMode = false;
            mazeMasterMode = false;
        }
        
        // Set initial rotation if locking rotation
        if (lockRotation)
        {
            transform.rotation = Quaternion.Euler(normallockedRotation);
        }

        normalMode = true;
    }
    
    void FixedUpdate()
    {
        if (target == null) return;

        if (normalMode)
        {
            // Calculate target position
            targetPosition = target.position + normalOffset;
            // Smoothly move camera to target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            // Keep rotation locked if enabled
            if (lockRotation)
            {
                transform.rotation = Quaternion.Euler(normallockedRotation);
            }

        }
        else if (mazerMode)
        {
            // Calculate target position
            targetPosition = target.position + mazerOffset;
            // Smoothly move camera to target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            // Keep rotation locked if enabled
            if (lockRotation)
            {
                transform.rotation = Quaternion.Euler(mazerlockedRotation);
            }

        }
        else if (mazeMasterMode)
        {
            // Calculate target position
            targetPosition = mazeMasterCameraPos.transform.position;
            // Smoothly move camera to target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            // Keep rotation locked if enabled
            if (lockRotation)
            {
                transform.rotation = Quaternion.Euler(mazeMasterlockedRotation);
            }
        }
    }

} 