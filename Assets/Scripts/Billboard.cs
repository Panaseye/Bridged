using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera targetCamera;

    void LateUpdate()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        // Make the bubble face the camera
        transform.forward = targetCamera.transform.forward;
    }
}