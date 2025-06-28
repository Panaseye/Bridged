using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool directionLeft = true;
    
    [Header("Obstacle Type")]
    [SerializeField] private bool movingPlatform = false;
    [SerializeField] private bool turningObstacle = false;
    [SerializeField] private Transform obstacleToMove; // The child object that should move
    
    [Header("Waypoint Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool loopMovement = true;
    [SerializeField] private bool pingPongMovement = false;
    
    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;
    
    private int currentWaypointIndex = 0;
    private bool movingForward = true;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody movingRb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Use obstacleToMove if assigned, otherwise use this object
        Transform movingTransform = obstacleToMove != null ? obstacleToMove : transform;
        movingRb = movingTransform.GetComponent<Rigidbody>();
        if (movingRb == null)
        {
            movingRb = movingTransform.gameObject.AddComponent<Rigidbody>();
        }
        movingRb.isKinematic = true;
        startPosition = movingTransform.position;
        startRotation = movingTransform.rotation;
        
        // Validate waypoints
        if (movingPlatform && (waypoints == null || waypoints.Length < 2))
        {
            Debug.LogWarning("MovingObstacle: Moving platform needs at least 2 waypoints!");
            movingPlatform = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (movingPlatform)
        {
            MoveBetweenWaypoints();
        }
        else if (turningObstacle)
        {
            RotateObstacle();
        }
    }
    
    private void MoveBetweenWaypoints()
    {
        if (waypoints == null || waypoints.Length < 2) 
        {
            Debug.LogWarning("No waypoints or not enough waypoints!");
            return;
        }
        
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        if (targetWaypoint == null) 
        {
            Debug.LogError("Target waypoint is null at index: " + currentWaypointIndex);
            return;
        }
        
        Transform movingTransform = obstacleToMove != null ? obstacleToMove : transform;
        
        // Calculate distance to target waypoint
        float distanceToWaypoint = Vector3.Distance(movingTransform.position, targetWaypoint.position);
        
        // If we're very close to the waypoint, snap to it and move to next
        if (distanceToWaypoint < 0.1f)
        {
            movingRb.MovePosition(targetWaypoint.position);
            MoveToNextWaypoint();
            return;
        }
        
        // Calculate movement for this frame
        Vector3 direction = (targetWaypoint.position - movingTransform.position).normalized;
        float moveDistance = speed * Time.deltaTime;
        
        // Check if we would overshoot the waypoint
        if (moveDistance > distanceToWaypoint)
        {
            movingRb.MovePosition(targetWaypoint.position);
            MoveToNextWaypoint();
        }
        else
        {
            movingRb.MovePosition(movingTransform.position + direction * moveDistance);
        }
    }
    
    private void MoveToNextWaypoint()
    {
        if (pingPongMovement)
        {
            if (movingForward)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = waypoints.Length - 2;
                    movingForward = false;
                }
            }
            else
            {
                currentWaypointIndex--;
                if (currentWaypointIndex < 0)
                {
                    currentWaypointIndex = 1;
                    movingForward = true;
                }
            }
        }
        else
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                if (loopMovement)
                {
                    currentWaypointIndex = 0;
                }
                else
                {
                    currentWaypointIndex = waypoints.Length - 1;
                }
            }
        }
    }
   
    private void RotateObstacle()
    {
        float rotationDirection = directionLeft ? -1f : 1f;
        Transform rotatingTransform = obstacleToMove != null ? obstacleToMove : transform;
        Rigidbody rotatingRb = movingRb;
        // For rotation, use Rigidbody.MoveRotation if rotating the whole object
        if (rotatingTransform == (obstacleToMove != null ? obstacleToMove : transform))
        {
            Quaternion deltaRotation = Quaternion.Euler(0, rotationDirection * speed * Time.deltaTime, 0);
            rotatingRb.MoveRotation(rotatingRb.rotation * deltaRotation);
        }
        else
        {
            rotatingTransform.Rotate(0, rotationDirection * speed * Time.deltaTime, 0);
        }
    }
    
    // Editor helper methods
    public void AddWaypoint()
    {
        // Create a new waypoint as a child of this MovingObstacle GameObject
        GameObject waypoint = new GameObject("Waypoint_" + (waypoints != null ? waypoints.Length : 0));
        waypoint.transform.SetParent(transform);
        
        // Position the waypoint relative to the obstacle's current position
        waypoint.transform.position = transform.position + Vector3.right * 2f; // Default offset
        
        // Add to waypoints array
        if (waypoints == null)
        {
            waypoints = new Transform[1];
        }
        else
        {
            System.Array.Resize(ref waypoints, waypoints.Length + 1);
        }
        waypoints[waypoints.Length - 1] = waypoint.transform;
    }
    
    public void ClearWaypoints()
    {
        if (waypoints != null)
        {
            foreach (Transform waypoint in waypoints)
            {
                if (waypoint != null)
                {
                    DestroyImmediate(waypoint.gameObject);
                }
            }
        }
        waypoints = null;
        currentWaypointIndex = 0;
    }
    
    // Draw gizmos in the editor
    void OnDrawGizmos()
    {
        if (!showGizmos) return;
        
        if (movingPlatform && waypoints != null && waypoints.Length > 0)
        {
            Gizmos.color = Color.green;
            
            // Draw waypoints
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
                    
                    // Draw line to next waypoint
                    if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                    }
                    else if (loopMovement && waypoints[0] != null)
                    {
                        // Draw line from last to first waypoint if looping
                        Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                    }
                }
            }
            
            // Highlight current waypoint
            if (currentWaypointIndex < waypoints.Length && waypoints[currentWaypointIndex] != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waypoints[currentWaypointIndex].position, 0.2f);
            }
        }
    }
}
