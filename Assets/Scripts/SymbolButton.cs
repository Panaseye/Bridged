using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SymbolButton : MonoBehaviour
{
    public Transform symbolParent; // Assign the Canvas in Inspector
    public UnityEvent onSymbolChanged;

    private int currentIndex = 0;
    private Transform[] symbolChildren;
    private List<PlayerInteraction> playersInRange = new List<PlayerInteraction>();

    void Start()
    {
        // Cache all children of symbolParent as symbol options
        int count = symbolParent.childCount;
        symbolChildren = new Transform[count];
        for (int i = 0; i < count; i++)
            symbolChildren[i] = symbolParent.GetChild(i);

        UpdateSymbol();
    }

    void Update()
    {
        foreach (var player in playersInRange)
        {
            if (Input.GetKeyDown(player.interactKey))
            {
                Interact();
                break; // Only allow one interaction per frame
            }
        }
    }

    public void Interact()
    {
        currentIndex = (currentIndex + 1) % symbolChildren.Length;
        UpdateSymbol();
        onSymbolChanged.Invoke();
    }

    void UpdateSymbol()
    {
        for (int i = 0; i < symbolChildren.Length; i++)
            symbolChildren[i].gameObject.SetActive(i == currentIndex);
    }

    public int GetSymbolIndex() => currentIndex;

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerInteraction>();
        if (player != null && !playersInRange.Contains(player))
            playersInRange.Add(player);
    }

    void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerInteraction>();
        if (player != null)
            playersInRange.Remove(player);
    }
}