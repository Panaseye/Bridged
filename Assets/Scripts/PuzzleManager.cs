using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    public Socket[] sockets; // Assign in Inspector, order matters!
    public string[] correctIDs; // The required socketID for each socket, in order
    [Header("Events")]
    public UnityEvent onPuzzleSolved;

    private bool puzzleSolved = false;

    public void OnSocketChanged()
    {
        if (puzzleSolved) return;
        for (int i = 0; i < sockets.Length; i++)
        {
            var obj = sockets[i].GetCurrentObject();
            if (obj == null || obj.socketID != correctIDs[i])
                return; // Not solved
        }
        // Puzzle solved!
        puzzleSolved = true;
        Debug.Log("Puzzle solved!");
        onPuzzleSolved.Invoke();
    }
}