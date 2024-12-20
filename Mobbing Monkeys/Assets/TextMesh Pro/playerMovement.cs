using System.Collections;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float jumpHeight = 3f;
    public float gravity = -9.8f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isKnockedBack = false; // Flag to track knockback state
    private Vector3 knockbackDirection; // Stores the knockback direction
    private float knockbackTimer = 0f;
    private float knockbackDuration = 1f; // Knockback will last for exactly 1 second

    void Update()
    {
        if (isKnockedBack)
        {
            ApplyKnockbackMovement();
            return; // Skip regular movement during knockback
        }

        HandleMovement();
        HandleJumpAndGravity();
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset vertical velocity when grounded
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    private void HandleJumpAndGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Jump logic
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Initial jump velocity
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Double jump velocity
                canDoubleJump = false;
            }
        }

        // Apply consistent gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the character with gravity
        controller.Move(velocity * Time.deltaTime);
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (!isKnockedBack) // Ensure knockback isn't applied multiple times
        {
            isKnockedBack = true;

            // Limit vertical component of the knockback direction
            direction.y = Mathf.Clamp(direction.y, -0.01f, 0.01f); // Adjust these values for less verticality

            // Apply knockback velocity directly
            knockbackDirection = direction.normalized * force;

            // Set the initial knockback timer
            knockbackTimer = knockbackDuration;
        }
    }

    private void ApplyKnockbackMovement()
    {
        if (knockbackTimer > 0)
        {
            // Calculate the deceleration per frame to ensure it ends in exactly 1 second
            float decelerationRate = Time.deltaTime / knockbackDuration;

            // Apply knockback movement
            controller.Move(knockbackDirection * Time.deltaTime);

            // Gradually reduce the knockback direction's magnitude
            knockbackDirection = Vector3.Lerp(knockbackDirection, Vector3.zero, decelerationRate);

            // Reduce the timer
            knockbackTimer -= Time.deltaTime;
        }
        else
        {
            // End knockback after 1 second
            isKnockedBack = false;
            knockbackDirection = Vector3.zero;
        }
    }
}
