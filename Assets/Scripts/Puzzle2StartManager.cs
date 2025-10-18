using UnityEngine;

public class Puzzle2StartManager : MonoBehaviour
{
    public DialogueManager2 dialogueManager; 
    public RoomRotationPuzzle roomRotationPuzzle; 

    private void Start()
    {
        roomRotationPuzzle.OnFirstRotationStart += OnRoomRotationStart; // Subscribe to the event
    }

    private void OnDestroy()
    {
        roomRotationPuzzle.OnFirstRotationStart -= OnRoomRotationStart; // Unsubscribe to avoid memory leaks
    }

    private void OnRoomRotationStart()
    {
        dialogueManager.StartDialogue(); // Start dialogue when room rotation starts
    }
}
