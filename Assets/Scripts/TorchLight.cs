using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torchlight : MonoBehaviour
{
    public Light2D torchLight;
    public CircleCollider2D torchCollider;
    private Vector2 movement;

    void Start()
    {
        // Initialize torch state as off
        torchLight.enabled = false;
        torchCollider.enabled = false;
    }

    void Update()
    {
        // Check for the 'T' key press to toggle torch state
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool isTorchOn = !torchLight.enabled;
            torchLight.enabled = isTorchOn;
            torchCollider.enabled = isTorchOn;
        }

        // Handle movement and torch rotation
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
        {
            movement.y = 0;
        }

        movement = movement.normalized;

        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            torchLight.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
    }
}
