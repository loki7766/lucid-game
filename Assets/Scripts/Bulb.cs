using UnityEngine.Rendering.Universal;
using UnityEngine;

public class DestroyBulb : MonoBehaviour
{
    public GameObject bright;
    public Light2D normalLight;
    public GameObject torchLight;
    public AudioSource bulbCollectedAudio;
    public DialogueManager dialogueManager; // Reference to the DialogueManager

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            torchLight.SetActive(false);
            bright.SetActive(false);
            normalLight.intensity = 1f;

            if (bulbCollectedAudio != null)
            {
                bulbCollectedAudio.enabled = true;
                bulbCollectedAudio.Play();
            }

            Destroy(gameObject); // Destroy the bulb

            // Destroy both interaction canvases
            if (dialogueManager.InteractionCanvas != null)
            {
                Destroy(dialogueManager.InteractionCanvas);
            }

            if (dialogueManager.InteractionCanvas1 != null)
            {
                Destroy(dialogueManager.InteractionCanvas1);
            }

            // Start the post-bulb dialogue sequence
            dialogueManager.StartDialogue(postBulb: true);

            // Scene transition will be handled after the dialogues are complete
            dialogueManager.OnDialogueEnd += OnDialogueEnd;
        }
    }

    // Callback for when the dialogue sequence ends
    private void OnDialogueEnd()
    {
        dialogueManager.OnDialogueEnd -= OnDialogueEnd; // Unsubscribe from the event
        SceneTransitionManager.Instance.TriggerSceneTransition(); // Load the next scene
    }
}
