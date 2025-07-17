using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public float holdDistance = 1.5f;
    public Transform holdPoint; // Should be in front of player
    public KeyCode interactKey = KeyCode.E;

    public HintUIManager myHintUIManager; // Assign in Inspector

    private HintInteractable nearbyHint;

    private GrabbableObject heldObject;
    private List<GrabbableObject> nearbyGrabbables = new List<GrabbableObject>();
    private List<Socket> nearbySockets = new List<Socket>();
    private InteractableButton nearbyButton;
    private Socket highlightedSocket;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            // HINT INTERACTION
            if (nearbyHint != null)
            {
                nearbyHint.Interact(myHintUIManager);
                return;
            }
            // BUTTON INTERACTION
            if (nearbyButton != null)
            {
                nearbyButton.Interact();
                return;
            }
            // GRABBABLE/SOCKET INTERACTION
            if (heldObject == null)
                TryGrab();
            else
                TryReleaseOrSocket();
        }
        // While holding, keep object in front of player
        if (heldObject != null)
        {
            Vector3 targetPos = holdPoint.position + holdPoint.forward * holdDistance;
            heldObject.transform.position = targetPos;
            heldObject.transform.rotation = holdPoint.rotation;
        }
        // Socket ghosting logic
        UpdateSocketGhosting();
    }

    void TryGrab()
    {
        if (nearbyGrabbables.Count > 0)
        {
            GrabbableObject grabbable = GetClosestGrabbable();

            // If the grabbable is in a socket, remove it from the socket
            foreach (var socket in nearbySockets)
            {
                if (socket.snapPoint != null && grabbable.transform.parent == socket.snapPoint)
                {
                    socket.RemoveObject();
                    break;
                }
            }

            heldObject = grabbable;
            heldObject.isHeld = true;
            heldObject.transform.SetParent(null); // Unparent in case it was parented
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb) { rb.isKinematic = true; rb.useGravity = false; }
        }
    }

    void TryReleaseOrSocket()
    {
        Socket socket = GetClosestCompatibleSocket();
        if (socket != null && socket.CanAccept(heldObject))
        {
            socket.PlaceObject(heldObject);
            heldObject = null;
        }
        else
        {
            // Drop
            if (heldObject != null)
            {
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb) { rb.isKinematic = false; rb.useGravity = true; }
                heldObject.isHeld = false;
                heldObject = null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GrabbableObject grabbable = other.GetComponent<GrabbableObject>();
        if (grabbable != null && !nearbyGrabbables.Contains(grabbable) && !grabbable.isHeld)
            nearbyGrabbables.Add(grabbable);
        Socket socket = other.GetComponent<Socket>();
        if (socket != null && !nearbySockets.Contains(socket))
            nearbySockets.Add(socket);
        // HINT INTERACTION
        HintInteractable hint = other.GetComponent<HintInteractable>();
        if (hint != null)
        {
            nearbyHint = hint;
            if (hint.mode == HintInteractable.HintMode.Hold)
            {
                hint.OnEnterRange(myHintUIManager);
            }
        }
        InteractableButton button = other.GetComponent<InteractableButton>();
        if (button != null)
            nearbyButton = button;
    }

    void OnTriggerExit(Collider other)
    {
        GrabbableObject grabbable = other.GetComponent<GrabbableObject>();
        if (grabbable != null)
            nearbyGrabbables.Remove(grabbable);
        Socket socket = other.GetComponent<Socket>();
        if (socket != null)
            nearbySockets.Remove(socket);
        // HINT INTERACTION
        HintInteractable hint = other.GetComponent<HintInteractable>();
        if (hint != null && nearbyHint == hint)
        {
            hint.EndInteract();
            nearbyHint = null;
        }
        InteractableButton button = other.GetComponent<InteractableButton>();
        if (button != null && nearbyButton == button)
            nearbyButton = null;
    }

    GrabbableObject GetClosestGrabbable()
    {
        float minDist = float.MaxValue;
        GrabbableObject closest = null;
        foreach (var g in nearbyGrabbables)
        {
            float dist = Vector3.Distance(transform.position, g.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = g;
            }
        }
        return closest;
    }

    Socket GetClosestCompatibleSocket()
    {
        float minDist = float.MaxValue;
        Socket closest = null;
        foreach (var s in nearbySockets)
        {
            if (heldObject != null && s.CanAccept(heldObject))
            {
                float dist = Vector3.Distance(transform.position, s.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = s;
                }
            }
        }
        return closest;
    }

    void UpdateSocketGhosting()
    {
        Socket socket = GetClosestCompatibleSocket();
        if (highlightedSocket != socket)
        {
            if (highlightedSocket != null)
                highlightedSocket.HideGhost();
            highlightedSocket = socket;
            if (highlightedSocket != null && heldObject != null)
                highlightedSocket.ShowGhost(heldObject);
        }
    }
}