using UnityEngine;

public class StopRotationTrigger : MonoBehaviour
{
    public RoomRotationPuzzle roomRotationPuzzle;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character")) // Ensure the character has the tag "Character"
        {
            roomRotationPuzzle.StopRotation();
            Destroy(gameObject); // Destroy the lever after it's used
        }
    }
}