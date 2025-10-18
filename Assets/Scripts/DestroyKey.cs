using UnityEngine;

public class KeyCollision : MonoBehaviour
{
    public DoorSpawner doorSpawner;
    public AudioSource audioSource;
    public AudioClip keyCollectionAudioClip;
    public DialogueManager2 dialogueManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            // Play key collection sound
            if (audioSource != null && keyCollectionAudioClip != null)
            {
                audioSource.PlayOneShot(keyCollectionAudioClip);
            }
            else
            {
                Debug.LogWarning("AudioSource or KeyCollectionAudioClip is not assigned.");
            }

            // Start spawning the door after collecting the key
            if (doorSpawner != null)
            {
                doorSpawner.StartSpawningDoor(); // Trigger delayed activation
            }
            else
            {
                Debug.LogWarning("DoorSpawner is not assigned.");
            }

            // Start the post-dialogue after collecting the key
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue(true); // true indicates it's post-dialogue
            }
            else
            {
                Debug.LogWarning("DialogueManager2 script is not assigned.");
            }

            // Destroy the key object after collection
            Destroy(gameObject);
        }
    }
}
