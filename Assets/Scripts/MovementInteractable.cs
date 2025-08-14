using UnityEngine;
using System.Collections.Generic;

public class MovementInteractable : MonoBehaviour
{
    public float movementThreshold = 0.1f;
    public float stopDelay = 0.2f;
    public DoorOrGate door;

    private HashSet<Rigidbody> playersInTrigger = new HashSet<Rigidbody>();
    public bool isAnyPlayerMoving { get; private set; }
    private float lastMoveTime = 0f;
    private bool doorIsOpen = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
                playersInTrigger.Add(rb);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
                playersInTrigger.Remove(rb);
        }
    }

    void Update()
    {
        bool anyPlayerMoving = false;
        foreach (var rb in playersInTrigger)
        {
            if (rb.velocity.magnitude > movementThreshold)
            {
                anyPlayerMoving = true;
                lastMoveTime = Time.time;
                break;
            }
        }

        isAnyPlayerMoving = anyPlayerMoving;

        // Open the door if a player starts moving and it's not already open
        if (isAnyPlayerMoving && !doorIsOpen)
        {
           
            doorIsOpen = true;
        }

        // Close the door if no player has moved for stopDelay seconds and it's open
        if (!isAnyPlayerMoving && doorIsOpen && Time.time - lastMoveTime > stopDelay)
        {
            
            doorIsOpen = false;
        }
    }
}