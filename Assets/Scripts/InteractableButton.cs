using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour
{
    private bool playerInRange = false;
    private bool isOn = false;
    private float blinkTimer = 0f;

    [Header("Materials")]
    public Material offMaterial;
    public Material onMaterial;

    [Header("Blink Color")]
    public Color blinkColor; // The color to blink towards

    [SerializeField] private Renderer rend;
    private Material runtimeMaterial; // Instance for blinking
    private Color baseColor;

    [Header("Interaction Event")]
    public UnityEvent onInteract;

    [Header("Lever Animation (optional)")]
    public bool isLever = false;
    public Animator leverAnimator; // Assign if this is a lever

    // Optional: assign in inspector or dynamically
    // public GameObject thingToActivate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        SetButtonMaterial();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            blinkTimer = 0f;
            // Optional: show UI prompt
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            SetButtonMaterial();
            // Optional: hide UI prompt
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            blinkTimer += Time.deltaTime * 2f; // Speed of blinking
            float lerp = (Mathf.Sin(blinkTimer) + 1f) / 2f; // 0 to 1
            Color lerpedColor = Color.Lerp(baseColor, blinkColor, lerp);
            runtimeMaterial.color = lerpedColor;
        }

    }

    public void Interact()
    {
        isOn = !isOn;
        SetButtonMaterial();

        // Lever animation logic
        if (isLever && leverAnimator != null)
        {
            leverAnimator.SetBool("LeverUp", isOn);
        }

        Debug.Log("Button pressed! (implement your logic here)");
        onInteract.Invoke();
    }

    void SetButtonMaterial()
    {
        if (rend != null)
        {
            runtimeMaterial = new Material(isOn ? onMaterial : offMaterial);
            rend.material = runtimeMaterial;
            baseColor = runtimeMaterial.color;
        }
    }
}
