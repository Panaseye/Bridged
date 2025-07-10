using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private int objectsOnPlate = 0;
    [SerializeField] private int neededObjectsOnPlate;
    [SerializeField] private Renderer plateRenderer;
    [SerializeField] private Material pressedMaterial;
    [SerializeField] private Material releasedMaterial;
    [SerializeField] private float pressedYScale = 0.2f;
    [SerializeField] private float animationDuration = 0.2f;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Coroutine scaleRoutine;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Or use layers/tags for other objects
        {
            objectsOnPlate++;
            if (objectsOnPlate == neededObjectsOnPlate)
            {
                onPressed.Invoke();
                plateRenderer.material = pressedMaterial;
                AnimatePlate(true);
            }
                
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectsOnPlate--;
            if (objectsOnPlate != neededObjectsOnPlate)
            {
                onReleased.Invoke();
                plateRenderer.material = releasedMaterial;
                AnimatePlate(false);
            }
        }
    }

    private void AnimatePlate(bool pressed)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(AnimateScale(pressed));
    }

    private IEnumerator AnimateScale(bool pressed)
    {
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = originalScale;
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = originalPosition;

        if (pressed)
        {
            endScale = new Vector3(originalScale.x, pressedYScale, originalScale.z);
            float yOffset = (originalScale.y - pressedYScale) * 0.5f;
            endPos = originalPosition - new Vector3(0, yOffset, 0);
        }

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
        transform.localPosition = endPos;
    }
}