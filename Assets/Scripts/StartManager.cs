using UnityEngine;
using System.Collections;

public class Puzzle1StartManager : MonoBehaviour
{
    public DialogueManager dialogueManager; // Reference to the DialogueManager script

    private void Start()
    {
        // Start the sequence after the scene is loaded
        StartCoroutine(FreezeAndShowDialogue());
    }

    private IEnumerator FreezeAndShowDialogue()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(5f);

        // Start the pre-bulb dialogue
        dialogueManager.StartDialogue(postBulb: false); // Ensure this starts the pre-bulb dialogues
    }
}
