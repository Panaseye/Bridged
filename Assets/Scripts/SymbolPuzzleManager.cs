using UnityEngine;
using UnityEngine.Events;

public class SymbolPuzzleManager : MonoBehaviour
{
    public SymbolButton[] buttons; // Assign in Inspector
    public int[] correctIndices;   // The correct index for each button
    public UnityEvent onPuzzleSolved;

    public void OnButtonChanged()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetSymbolIndex() != correctIndices[i])
                return;
        }
        Debug.Log("Puzzle solved!");
        onPuzzleSolved.Invoke();
    }
}