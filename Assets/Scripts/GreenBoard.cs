using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GreenBoard : MonoBehaviour
{
    public Text boardText;
    public BoxCollider2D innerCollider;
    private List<int> numberSequence;
    public int sequenceLength = 6;

    public GameObject secondPanelCanvas; // Reference to the second panel canvas
    public Text secondPanelText; // Reference to the text component in the second panel
    public string secondPanelMessage = "Press 'I' to interact with the tables"; // Message for the second panel

    public float typingSpeed = 0.03f; // Typing speed for interaction text

    private bool puzzleStarted = false;
    private bool secondPanelDisplayed = false; // Track if the second panel message has been displayed

    void Start()
    {
        numberSequence = GenerateNumberSequence(sequenceLength);
        boardText.text = "";
        ToggleTableLights(false); // Ensure all table lights are off initially
        secondPanelCanvas.SetActive(false); // Ensure the second panel canvas is inactive at the start
    }

    private List<int> GenerateNumberSequence(int length)
    {
        List<int> sequence = new List<int>();
        int previousNumber = -1;

        for (int i = 0; i < length; i++)
        {
            int randomNumber;

            do
            {
                randomNumber = Random.Range(1, 11);
            }
            while ((i > 0 && randomNumber == previousNumber) ||
                   (i >= 2 && randomNumber == sequence[i - 1] && sequence[i - 2] == sequence[i - 1]));

            sequence.Add(randomNumber);
            previousNumber = randomNumber;
        }

        return sequence;
    }

    public List<int> GetNumberSequence()
    {
        return numberSequence;
    }

    private void DisplayNumberSequence()
    {
        boardText.text = string.Join(" ", numberSequence);
        if (!secondPanelDisplayed)
        {
            StartCoroutine(DisplaySecondPanelMessage(secondPanelMessage)); // Show the second panel after displaying the number sequence
            secondPanelDisplayed = true; // Ensure the second panel message is displayed only once
        }
    }

    private void HideNumberSequence()
    {
        boardText.text = "";
    }

    private void ToggleTableLights(bool state)
    {
        PuzzleTable[] allTables = FindObjectsOfType<PuzzleTable>();

        foreach (var table in allTables)
        {
            if (table.tableLight != null)
            {
                table.tableLight.enabled = state;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Torchlight") && other.bounds.Intersects(innerCollider.bounds))
        {
            DisplayNumberSequence();
            ToggleTableLights(true); // Turn on the table lights when the sequence is displayed
            if (!puzzleStarted)
            {
                StartCoroutine(DisplayInteractionMessage("Press 'I' to interact with the tables")); // Show interaction text
                puzzleStarted = true; // Ensure puzzle starts only once
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Torchlight"))
        {
            HideNumberSequence();
        }
    }

    public void OnPuzzleCompleted()
    {
        ToggleTableLights(false); // Turn off all table lights after the puzzle is completed
        puzzleStarted = false;
        HideNumberSequence();
        secondPanelCanvas.SetActive(false); // Hide the second panel when the puzzle is completed
    }

    private IEnumerator DisplayInteractionMessage(string message)
    {
        yield return new WaitForSecondsRealtime(4f);
        foreach (char letter in message.ToCharArray())
        {
            yield return new WaitForSecondsRealtime(typingSpeed); // Use WaitForSecondsRealtime to work with Time.timeScale = 0
        }
        yield return new WaitForSecondsRealtime(4f);
        secondPanelCanvas.SetActive(false);
    }

    private IEnumerator DisplaySecondPanelMessage(string message)
    {
        secondPanelText.text = ""; // Clear text initially
        secondPanelCanvas.SetActive(true); // Activate the second panel
        foreach (char letter in message.ToCharArray())
        {
            secondPanelText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed); // Typing effect
        }
        yield return new WaitForSecondsRealtime(4f); // Keep the message visible for a bit
        secondPanelCanvas.SetActive(false); // Disable the second panel after 4 seconds
    }
}
