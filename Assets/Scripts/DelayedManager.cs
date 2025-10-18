using UnityEngine;
using System.Collections;

public class DelayedActivation : MonoBehaviour
{
    public GameObject doorPrefab;
    public AudioSource audioSource;
    public AudioClip doorActivationAudioClip;
    public DialogueManager2 dialogueManager;
    public float interactionMessageDelay = 2f; // Delay before showing the interaction message
    public float interactionMessageDuration = 4f; // Duration to show the interaction message

    public void ActivateDoorWithDelay(float delay)
    {
        StartCoroutine(ActivateDoorCoroutine(delay));
    }

    private IEnumerator ActivateDoorCoroutine(float delay)
    {
        // Wait for the specified delay before activating the door
        yield return new WaitForSeconds(delay);

        // Activate the door
        if (doorPrefab != null)
        {
            doorPrefab.SetActive(true);

            // Play the door activation sound
            if (audioSource != null && doorActivationAudioClip != null)
            {
                audioSource.PlayOneShot(doorActivationAudioClip);
            }

            // Show the interaction message after a further delay
            if (dialogueManager != null)
            {
                yield return new WaitForSeconds(interactionMessageDelay); // Wait before showing the interaction message
                dialogueManager.DisplayInteractionMessageForSeconds(interactionMessageDuration); // Show the message for the specified duration
            }
            else
            {
                Debug.LogWarning("DialogueManager is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("Door prefab is not assigned.");
        }
    }
}
