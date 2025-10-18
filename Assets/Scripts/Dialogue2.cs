using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager2 : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Button nextButton;
    public GameObject InteractionCanvas;
    public Text InteractionText;
    public float typingSpeed = 0.03f;
    public AudioClip buttonClickSound;

    private AudioSource audioSource;
    private bool isDialogueActive = false;
    private int currentLineIndex = 0;
    private bool isTyping = false;

    public delegate void DialogueEndHandler();
    public event DialogueEndHandler OnDialogueEnd;

    public string dialogueLine1 = "Oh no...the room is rotating now";
    public string dialogueLine2 = "There must be something to stop the rotation";

    public string postDialogue1 = "The weird things happening in this room, it all feels like a dream";

    private bool isPostDialogue = false;

    public string InteractionMessage = "Press 'E' to open the door";

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nextButton.onClick.AddListener(OnNextButtonClick);

        dialoguePanel.SetActive(false);
        if (InteractionCanvas != null)
        {
            InteractionCanvas.SetActive(false);
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

    public void StartDialogue(bool postDialogue = false)
    {
        if (isDialogueActive)
            return;

        isPostDialogue = postDialogue;
        currentLineIndex = 0; // Start with the first dialogue line
        isDialogueActive = true;
        Time.timeScale = 0; // Freeze game time
        dialoguePanel.SetActive(true);
        ShowNextDialogue();
    }

    private void ShowNextDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = GetDialogueLine(currentLineIndex);
            isTyping = false;
        }
        else
        {
            if (currentLineIndex < GetTotalDialogueLines())
            {
                StartCoroutine(DisplayDialogue(GetDialogueLine(currentLineIndex)));
                currentLineIndex++;
            }
            else
            {
                EndDialogue();
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

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1;
        isDialogueActive = false;

        if (!isPostDialogue && OnDialogueEnd != null)
        {
            OnDialogueEnd.Invoke();
        }
    }

    public void DisplayInteractionMessageForSeconds(float duration)
    {
        if (InteractionCanvas != null)
        {
            InteractionCanvas.SetActive(true);
            StartCoroutine(DisplayInteractionMessage(InteractionMessage));
        }

        StartCoroutine(HideInteractionMessageAfterDelay(duration));
    }

    private IEnumerator DisplayInteractionMessage(string message)
    {
        InteractionText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            InteractionText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    private IEnumerator HideInteractionMessageAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (InteractionCanvas != null)
        {
            InteractionCanvas.SetActive(false);
        }
    }

    private string GetDialogueLine(int index)
    {
        if (isPostDialogue)
        {
            return postDialogue1;
        }
        else
        {
            switch (index)
            {
                case 0: return dialogueLine1;
                case 1: return dialogueLine2;
                default: return "";
            }
        }
    }

    private int GetTotalDialogueLines()
    {
        return isPostDialogue ? 1 : 2; // Adjust according to the dialogue lines
    }
}
