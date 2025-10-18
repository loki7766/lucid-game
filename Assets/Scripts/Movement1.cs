using UnityEngine;

public class Movement1 : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D body;
    public Transform roomTransform; // Reference to the room's transform
    public Animator animator;

    Vector2 movement;

    void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize movement to ensure consistent speed
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }

        // If the movement is not zero, update the animator parameters
        if (movement.x != 0)
        {
            movement.y = 0;
        }
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Update last direction parameters
        if (movement.sqrMagnitude > 0)
        {
            animator.SetFloat("LastHorizontal", movement.x);
            animator.SetFloat("LastVertical", movement.y);
        }
    }

    void FixedUpdate()
    {
        if (roomTransform != null)
        {
            // Move character in local space relative to the room
            Vector2 localMovement = roomTransform.TransformDirection(movement);
            body.MovePosition(body.position + localMovement * speed * Time.fixedDeltaTime);
        }
        else
        {
            Debug.LogWarning("RoomTransform is not assigned.");
        }
    }
}
