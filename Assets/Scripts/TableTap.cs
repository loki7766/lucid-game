using UnityEngine;
using UnityEngine.Rendering.Universal;
public class PuzzleTable : MonoBehaviour
{
    public int tableNumber;

    private PuzzleManager manager;
    private bool isPlayerNearby = false;
    public Light2D tableLight;

    void Start()
    {
        manager = FindObjectOfType<PuzzleManager>();
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.I))
        {
            manager.OnTableTapped(tableNumber);
        }
    }

    public void DisableLight()
    {
        if (tableLight != null)
        {
            tableLight.enabled = false; 
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            isPlayerNearby = false;
        }
    }
}