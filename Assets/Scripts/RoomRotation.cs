using UnityEngine;
using System.Collections;

public class RoomRotationPuzzle : MonoBehaviour
{
    public Transform room;
    public GameObject leverPrefab;
    public Transform[] spawnPoints;
    public GameObject keyPrefab;
    public float rotationSpeed = 15f;
    public float rotationInterval = 4f;
    public float leverSpawnInterval = 5f;
    public Camera mainCamera;
    private bool isRotating = true;
    private GameObject currentLever;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private float originalCameraSize;

    public AudioSource audioSource;
    public AudioClip rotationAudioClip;

    private Transform characterTransform;

    // Define the event here
    public delegate void RotationStartHandler();
    public event RotationStartHandler OnFirstRotationStart;

    private bool hasFirstRotationStarted = false;

    void Start()
    {
        originalScale = room.localScale;
        originalRotation = room.rotation;
        originalCameraSize = mainCamera.orthographicSize;

        // Find the character transform in the scene
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        Movement1 characterMovement = characterTransform.GetComponent<Movement1>();
        if (characterMovement != null)
        {
            characterMovement.roomTransform = room;
        }
        else
        {
            Debug.LogWarning("Movement script is not found on the character.");
        }
        StartCoroutine(RotateRoom());
        StartCoroutine(SpawnLeverAfterDelay());
    }

    IEnumerator RotateRoom()
    {
        while (isRotating)
        {
            yield return new WaitForSeconds(rotationInterval);

            // Trigger the event only on the first rotation
            if (!hasFirstRotationStarted)
            {
                hasFirstRotationStarted = true;
                OnFirstRotationStart?.Invoke();
            }

            if (audioSource != null && rotationAudioClip != null)
            {
                audioSource.PlayOneShot(rotationAudioClip);
            }

            Quaternion targetRotation = room.rotation * Quaternion.Euler(0, 0, 90f);
            float duration = 0.75f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                room.rotation = Quaternion.Slerp(room.rotation, targetRotation, t);
                AdjustRoomScale();
                AdjustCameraSize();

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final rotation is perfectly set
            room.rotation = targetRotation;
            AdjustRoomScale();
            AdjustCameraSize();

            // Fix any minor positional errors when returning to the original position
            if (Mathf.Abs(room.rotation.eulerAngles.z) < 0.1f || Mathf.Abs(room.rotation.eulerAngles.z - 360) < 0.1f)
            {
                room.localScale = originalScale;
                room.rotation = originalRotation;
                mainCamera.orthographicSize = originalCameraSize;
            }
            else if (Mathf.Abs(room.rotation.eulerAngles.z - 180) < 0.1f)
            {
                // No adjustment needed, but ensure it's set
                room.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
        }
    }

    IEnumerator SpawnLeverAfterDelay()
    {
        yield return new WaitForSeconds(rotationInterval);
        StartCoroutine(ChangeLeverPosition());
    }

    IEnumerator ChangeLeverPosition()
    {
        while (isRotating)
        {
            if (currentLever != null)
            {
                Destroy(currentLever);
            }

            bool validPositionFound = false;

            while (!validPositionFound)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Transform selectedPoint = spawnPoints[randomIndex];

                float distanceToCharacter = Vector3.Distance(characterTransform.position, selectedPoint.position);

                if (distanceToCharacter > 4f)
                {
                    currentLever = Instantiate(leverPrefab, selectedPoint.position, Quaternion.identity);
                    currentLever.transform.SetParent(room);  // Ensure the lever is a child of the room
                    currentLever.SetActive(true);

                    validPositionFound = true;
                }
                yield return null;
            }

            if (!validPositionFound)
            {
                Debug.LogWarning("Failed to find a valid spawn position for the lever.");
            }

            yield return new WaitForSeconds(leverSpawnInterval);
        }
    }

    public void StopRotation()
    {
        StopAllCoroutines();

        isRotating = false;
        if (currentLever != null)
        {
            Destroy(currentLever);
        }

        room.rotation = originalRotation;
        room.localScale = originalScale;
        mainCamera.orthographicSize = originalCameraSize;

        EnableKey();
    }

    private void EnableKey()
    {
        if (keyPrefab != null)
        {
            keyPrefab.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Key prefab is not assigned.");
        }
    }

    private void AdjustRoomScale()
    {
        float rotationAngle = room.rotation.eulerAngles.z;

        if (Mathf.Approximately(rotationAngle, 90f) || Mathf.Approximately(rotationAngle, 270f))
        {
            room.localScale = new Vector3(originalScale.y, originalScale.x, originalScale.z);
        }
        else if (Mathf.Approximately(rotationAngle, 180f))
        {
            room.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            room.localScale = originalScale;
        }
    }

    private void AdjustCameraSize()
    {
        // Adjust the camera size to fit the room after rotation
        Bounds roomBounds = new Bounds(room.position, Vector3.zero);
        foreach (Renderer renderer in room.GetComponentsInChildren<Renderer>())
        {
            roomBounds.Encapsulate(renderer.bounds);
        }

        float roomHeight = roomBounds.size.y;
        float roomWidth = roomBounds.size.x;

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float roomAspect = roomWidth / roomHeight;

        if (roomAspect > screenAspect)
        {
            mainCamera.orthographicSize = roomWidth / screenAspect / 2;
        }
        else
        {
            mainCamera.orthographicSize = roomHeight / 2;
        }
    }
}
