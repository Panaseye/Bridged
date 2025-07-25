using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DoorOrGate : MonoBehaviour
{
    [Header("Positions")]
    public Vector3 closedPosition; // Local position when closed
    public Vector3 openedPosition; // Local position when open

    [Header("Animation")]
    public float moveDuration = 1f;

    [Header("Materials")]
    public Renderer doorRenderer;
    public Material idleMaterial;
    public Material movingMaterial;

    [Header("Events")]
    public UnityEvent onOpened;
    public UnityEvent onClosed;

    [SerializeField] bool openOveride = false;
    private bool isOpen = false;
    private Coroutine moveRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set initial position and material
        if (closedPosition == Vector3.zero)
        {
            closedPosition = transform.localPosition;
            transform.localPosition = closedPosition;
        }
        if (openedPosition == Vector3.zero)
        {
            openedPosition = transform.localPosition;
            transform.localPosition = openedPosition;
        }else{
            transform.localPosition = closedPosition;
        }

        
        SetIdleMaterial();
    }

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            MoveTo(openedPosition);
            onOpened.Invoke();
        }
    }

    public void OpenOverride()
    {       openOveride = true;
            isOpen = true;
            MoveTo(openedPosition);
            onOpened.Invoke();
           
        
    }

    public void Close()
    {
        if (isOpen && !openOveride)
        {
            isOpen = false;
            MoveTo(closedPosition);
            onClosed.Invoke();
        }
    }

    public void Toggle()
    {
        if (isOpen && !openOveride)
            Close();
        else
            Open();
    }

    private void MoveTo(Vector3 targetPos)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveDoor(targetPos));
    }

    private IEnumerator MoveDoor(Vector3 targetPos)
    {
        if (doorRenderer != null && movingMaterial != null)
            doorRenderer.material = movingMaterial;

        Vector3 startPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPos;

        SetIdleMaterial();
    }

    private void SetIdleMaterial()
    {
        if (doorRenderer != null && idleMaterial != null)
            doorRenderer.material = idleMaterial;
    }
    

}
