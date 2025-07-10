using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool useKeyboardInput = true;
    [SerializeField] private float rotationSpeed = 10f; // How fast the player rotates to face movement direction
    
    [Header("Mobile Settings")]
    [SerializeField] private bool showMobileJoystick = false;
    [SerializeField] private GameObject joystickPrefab;
    
    private Rigidbody rb;
    private Vector3 moveDirection;
    private GameObject currentJoystick;
    private Quaternion targetRotation;
    
    public enum ControlScheme
    {
        WASD,
        Arrows
        // Add more if needed (e.g., Gamepad1, Gamepad2)
    }

    public ControlScheme controlScheme = ControlScheme.WASD;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Check if we're on mobile and should show joystick
        #if UNITY_ANDROID || UNITY_IOS
        if (showMobileJoystick && joystickPrefab != null)
        {
            SpawnMobileJoystick();
        }
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        float h = 0f, v = 0f;

        switch (controlScheme)
        {
            case ControlScheme.WASD:
                h = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
                v = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
                break;
            case ControlScheme.Arrows:
                h = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
                v = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
                break;
        }

        Vector3 move = new Vector3(h, 0, v).normalized;
        moveDirection = move; // <-- THIS LINE IS NEEDED

        // Handle mobile joystick input (will be implemented later)
        HandleMobileInput();
        
        // Handle rotation towards movement direction
        HandleRotation();
    }
    
    void FixedUpdate()
    {
        // Apply movement using physics
        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }
    
    private void HandleKeyboardInput()
    {
        // Get input from WASD or arrow keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Create movement direction (X and Z axis only)
        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }
    
    private void HandleMobileInput()
    {
        // This will be implemented when we add joystick support
        // For now, it's a placeholder
    }
    
    private void SpawnMobileJoystick()
    {
        // This will spawn the joystick UI when we implement it
        // For now, it's a placeholder
        Debug.Log("Mobile joystick spawning will be implemented");
    }
    
    // Public method to set movement direction from external sources (like joystick)
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = new Vector3(direction.x, 0f, direction.z).normalized;
    }
    
    // Public method to stop movement
    public void StopMovement()
    {
        moveDirection = Vector3.zero;
    }
    
    // Getter for current movement direction (useful for animations)
    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
    
    // Getter for whether the player is moving
    public bool IsMoving()
    {
        return moveDirection != Vector3.zero;
    }
    
    private void HandleRotation()
    {
        // Only rotate if we're moving
        if (moveDirection != Vector3.zero)
        {
            // Calculate the target rotation based on movement direction
            targetRotation = Quaternion.LookRotation(moveDirection);
            
            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
