using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Button nextButton;
    public GameObject InteractionCanvas;
    public Text InteractionText;
    public GameObject InteractionCanvas1;
    public Text InteractionText1;
    public float typingSpeed = 0.03f;
    public AudioClip buttonClickSound;

    private AudioSource audioSource;
    private bool isDialogueActive = false;
    private int currentLineIndex = 0;
    private bool isTyping = false;

    public delegate void DialogueEndHandler();
    public event DialogueEndHandler OnDialogueEnd;

    // Initial dialogues (displayed before the bulb is destroyed)
    public string dialogueLine1 = "I don't know where everyone went.";
    public string dialogueLine2 = "This is my next dialogue.";
    public string dialogueLine3 = "Do you want to continue?";
    public string dialogueLine4 = "I hope we find them soon.";

    // New dialogues (displayed after the bulb is destroyed)
    public string postBulbDialogue1 = "The bulb is gone.";
    public string postBulbDialogue2 = "What happens next?";

    private bool isPostBulbDialogue = false;

    public string InteractionMessage = "Press 'T' to turn on torch light";

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nextButton.onClick.AddListener(OnNextButtonClick);

        dialoguePanel.SetActive(false);
        if (InteractionCanvas != null)
        {
            InteractionCanvas.SetActive(false);
        }
        if (InteractionCanvas1 != null)
        {
            InteractionCanvas1.SetActive(false);
        }

        Time.timeScale = 1;
    }

    private void OnNextButtonClick()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }

        if (!isTyping)
        {
            ShowNextDialogue();
        }
    }

    public void StartDialogue(bool postBulb = false)
    {
        if (isDialogueActive)
            return;

        isPostBulbDialogue = postBulb;
        currentLineIndex = 1; // Start with the first dialogue line
        isDialogueActive = true;
        Time.timeScale = 0; // Freeze game time
        dialoguePanel.SetActive(true);
        ShowNextDialogue();
    }

    public void ShowNextDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = GetDialogueLine(currentLineIndex);
            isTyping = false;
        }
        else
        {
            if ((isPostBulbDialogue && currentLineIndex <= 2) || (!isPostBulbDialogue && currentLineIndex <= 4))
            {
                StartCoroutine(DisplayDialogue(GetDialogueLine(currentLineIndex)));
                currentLineIndex++;
            }
            else
            {
                EndDialogue();
                currentLineIndex = 1; // Reset index when dialogue ends
            }
        }
    }

    private IEnumerator DisplayDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        isTyping = false;
    }

    private IEnumerator DisplayInteractionMessage(string message)
    {
        if (InteractionCanvas == null) yield break; // Exit if InteractionCanvas is destroyed
        InteractionText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            InteractionText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        // Wait for 4 seconds before hiding the interaction message
        yield return new WaitForSecondsRealtime(3f);

        if (InteractionCanvas != null)
        {
            InteractionCanvas.SetActive(false);
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1;
        isDialogueActive = false;

        if (InteractionCanvas != null)
        {
            InteractionCanvas.SetActive(true);
            StartCoroutine(DisplayInteractionMessage(InteractionMessage));
        }

        OnDialogueEnd?.Invoke();
    }

    private string GetDialogueLine(int index)
    {
        if (isPostBulbDialogue)
        {
            switch (index)
            {
                case 1: return postBulbDialogue1;
                case 2: return postBulbDialogue2;
                default: return "";
            }
        }
        else
        {
            switch (index)
            {
                case 1: return dialogueLine1;
                case 2: return dialogueLine2;
                case 3: return dialogueLine3;
                case 4: return dialogueLine4;
                default: return "";
            }
        }
    }
}