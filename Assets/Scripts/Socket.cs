using UnityEngine;

public class Socket : MonoBehaviour
{
    public string socketID; // Only accepts objects with matching ID
    public Transform snapPoint; // Where the object will snap to
    public Material ghostMaterial; // Assign in Inspector
    public PuzzleManager puzzleManager; // Assign in Inspector if used for a puzzle

    private GrabbableObject currentObject;
    private GameObject ghostObjectInstance;

    public bool CanAccept(GrabbableObject obj)
    {
          return currentObject == null;
    }

    public void PlaceObject(GrabbableObject obj)
    {
        currentObject = obj;
        obj.transform.position = snapPoint.position;
        obj.transform.rotation = snapPoint.rotation;
        obj.transform.SetParent(snapPoint);
        obj.isHeld = false;
        HideGhost();
        // Optionally: trigger events, play sound, etc.
        if (puzzleManager != null)
            puzzleManager.OnSocketChanged();
    }

    public void RemoveObject()
    {
        if (currentObject != null)
        {
            currentObject.transform.SetParent(null);
            currentObject = null;
        }
        if (puzzleManager != null)
            puzzleManager.OnSocketChanged();
    }

    // Show a ghost version of the grabbable object at the snap point
    public void ShowGhost(GrabbableObject obj)
    {
        if (ghostObjectInstance != null || ghostMaterial == null) return;
        ghostObjectInstance = Instantiate(obj.gameObject, snapPoint.position, snapPoint.rotation, snapPoint);
        ghostObjectInstance.transform.localScale = obj.transform.localScale;
        // Remove scripts and colliders from the ghost
        foreach (var comp in ghostObjectInstance.GetComponents<MonoBehaviour>()) Destroy(comp);
        foreach (var col in ghostObjectInstance.GetComponentsInChildren<Collider>()) Destroy(col);
        // Set all renderers to use the ghost material
        foreach (var rend in ghostObjectInstance.GetComponentsInChildren<Renderer>())
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = ghostMaterial;
            }
            rend.materials = mats;
        }
    }

    public void HideGhost()
    {
        if (ghostObjectInstance != null)
        {
            Destroy(ghostObjectInstance);
            ghostObjectInstance = null;
        }
    }

    public GrabbableObject GetCurrentObject() => currentObject;
}