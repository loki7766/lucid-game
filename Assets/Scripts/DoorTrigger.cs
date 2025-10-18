using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    public Animator doorAnimator;
    public KeyCode interactionKey = KeyCode.KeypadEnter;
    public float detectionRadius = 1f;
    public string sceneToLoad;

    private bool isNearDoor = false;
    private bool isDoorOpen = false;

    void Update()
    {
        if (isNearDoor && Input.GetKeyDown(interactionKey) && !isDoorOpen)
        {
            OpenDoor();
        }

        if (isNearDoor && isDoorOpen && Input.GetKeyDown(interactionKey))
        {
            StartCoroutine(LoadNextSceneWithDelay(2f)); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            isNearDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            isNearDoor = false;
        }
    }

    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", true);
            isDoorOpen = true;
        }
        else
        {
            Debug.LogWarning("Door Animator is not assigned.");
        }
    }

    private IEnumerator LoadNextSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not specified.");
        }
    }
}
