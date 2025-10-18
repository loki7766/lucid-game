using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public GreenBoard greenBoard;
    public GameObject keyFragmentPrefab;
    public Transform player;
    public float interactionDistance = 0.5f;

    public AudioClip successAudioClip;
    public AudioClip failureAudioClip;
    public AudioClip spawnAudioClip;
    public AudioSource audioSource;

    private List<int> numberSequence;
    private int currentSequenceIndex;
    private List<PuzzleTable> selectedTables = new List<PuzzleTable>();
    private bool sequenceCorrect = true;

    void Start()
    {
        numberSequence = greenBoard.GetNumberSequence();
        currentSequenceIndex = numberSequence.Count - 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            int tableNumber = GetClosestTableNumber();

            if (tableNumber != -1 && selectedTables.Count < 5)
            {
                OnTableTapped(tableNumber);
            }
        }
    }

    private int GetClosestTableNumber()
    {
        PuzzleTable[] tables = FindObjectsOfType<PuzzleTable>();

        foreach (PuzzleTable table in tables)
        {
            if (Vector2.Distance(player.position, table.transform.position) <= interactionDistance)
            {
                return table.tableNumber;
            }
        }
        return -1;
    }

    public void OnTableTapped(int tableNumber)
    {
        PuzzleTable[] tables = FindObjectsOfType<PuzzleTable>();

        foreach (var table in tables)
        {
            if (table.tableNumber == tableNumber)
            {
                table.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = Color.green;
                selectedTables.Add(table);
                break;
            }
        }

        if (tableNumber == numberSequence[currentSequenceIndex])
        {
            currentSequenceIndex--;
        }
        else
        {
            sequenceCorrect = false;
        }

        if (selectedTables.Count == 5)
        {
            StartCoroutine(HandleSequenceCompletion());
        }
    }

    private IEnumerator HandleSequenceCompletion()
    {
        if (sequenceCorrect)
        {
            audioSource.PlayOneShot(successAudioClip);
            yield return new WaitForSeconds(1f);
            DisableLightsOnAllTables(); // Turn off the lights only when the puzzle is completed
            SpawnKeyFragment();
        }
        else
        {
            // If the sequence is wrong, turn the lights red for 1 second
            ResetTables(Color.red);
            audioSource.PlayOneShot(failureAudioClip);
            yield return new WaitForSeconds(1f);

            // Return the lights to their original color (white)
            ResetTables(Color.white);
        }

        // Reset for the next attempt if the puzzle isn't completed
        currentSequenceIndex = numberSequence.Count - 1;
        selectedTables.Clear();
        sequenceCorrect = true;
    }

    private void ResetTables(Color color)
    {
        foreach (var table in selectedTables)
        {
            table.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = color;
        }
    }

    private void DisableLightsOnAllTables()
    {
        PuzzleTable[] allTables = FindObjectsOfType<PuzzleTable>();

        foreach (var table in allTables)
        {
            table.DisableLight();
        }
    }


    private void SpawnKeyFragment()
    {
        if (keyFragmentPrefab != null)
        {
            keyFragmentPrefab.SetActive(true);
            Instantiate(keyFragmentPrefab, keyFragmentPrefab.transform.position, keyFragmentPrefab.transform.rotation);
            audioSource.PlayOneShot(spawnAudioClip);
            greenBoard.boardText.gameObject.SetActive(false);
        }
    }
}
