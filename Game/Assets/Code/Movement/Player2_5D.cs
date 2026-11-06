using System;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Player2_5D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float groundAcceleration = 17.5f;
    [SerializeField] private float groundDeceleration = 25f;
    [SerializeField] private float airAcceleration = 8f;
    [SerializeField] private float airDeceleration = 0f;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 7.75f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.25f;

    [Header("Gravity")]
    [SerializeField] private float gravity = 12f;
    [SerializeField] private float fallGravityMultiplier = 1.5f;
    [SerializeField] private float maxFallSpeed = 75f;
    [SerializeField] private float apexThreshold = 2.5f;
    [SerializeField] private float apexGravityMultiplier = 2f;


    [Header("Ground")]
    [SerializeField] private float groundedGraceTime = 0.05f;

    [Header("Physics Material")]
    [SerializeField] private PhysicsMaterial noFrictionMaterial;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    private float inputX;
    private bool isGrounded;

    private float groundedTimer;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool jumpCutRequested;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        // Keep the player awake so OnCollisionStay keeps reporting ground contacts.
        rb.sleepThreshold = 0f;

        // Avoid tunnelling through thin platforms at high vertical speed.
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Assign a frictionless material so the player cannot cling to walls.
        capsule.material = noFrictionMaterial;

        // Custom gravity
        rb.useGravity = false;
    }

    // Handle user input
    private void Update()
    {
        ReadMovementInput();
        ReadJumpInput();
    }

    // Execute every tick
    private void FixedUpdate()
    {
        UpdateGroundedState();
        UpdateJumpTimers();
        MoveHorizontal();
        TryJump();
        ApplyGravity();
        ApplyJumpCut();
    }

    // Get horizontal movement direction input 
    private void ReadMovementInput()
    {
        inputX = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            inputX = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            inputX = 1f;
    }

    // Get jump input
    private void ReadJumpInput()
    {
        // Only register a new jump on the key press
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpBufferTimer = jumpBufferTime;

        // Register that the player wants to end the jump
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            jumpCutRequested = true;
    }

    // Set grounded grace time based on collision with floor
    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // Check for a mostly vertical normal (likely a floor) and if it does not have vertical velocity
            if (contact.normal.y > 0.5f && rb.linearVelocity.y <= 0.01f)
            {
                groundedTimer = groundedGraceTime;
                return;
            }
        }
    }

    // Set the isGrounded flag only after grace time passes
    private void UpdateGroundedState()
    {
        isGrounded = groundedTimer > 0f;
        groundedTimer -= Time.fixedDeltaTime;
    }

    // Horizontal movement handler
    private void MoveHorizontal()
    {
        // Actual velocity and target velocity
        float currentVx = rb.linearVelocity.x;
        float targetVx = inputX * maxSpeed;

        float rate;
        if (isGrounded)
            rate = (targetVx != 0) ? groundAcceleration : groundDeceleration;
        else
            rate = (targetVx != 0) ? airAcceleration : airDeceleration;

        // Moves currentVx towards targetVx at the rate every step
        float newVx = Mathf.MoveTowards(currentVx, targetVx, rate * Time.fixedDeltaTime);

        // Adds the new velocity change as a force
        rb.AddForce(new Vector3(newVx - currentVx, 0f, 0f), ForceMode.VelocityChange);
    }

    // Update jump related timers
    private void UpdateJumpTimers()
    {
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.fixedDeltaTime;

        jumpBufferTimer -= Time.fixedDeltaTime;
    }

    // Attempt to jump
    private void TryJump()
    {
        // If theres no buffer nor coyote timer, player can't jump
        if (jumpBufferTimer <= 0f || coyoteTimer <= 0f)
            return;

        // Set variables for on air status
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
        groundedTimer = 0f;
        isGrounded = false;

        // Set vertical speed
        Vector3 velocity = rb.linearVelocity;
        velocity.y = jumpSpeed;
        rb.linearVelocity = velocity;
    }

    private void ApplyGravity()
    {
        // More gravity at the highest point of the jump, a bit more falling and less jumping
        if (Math.Abs(rb.linearVelocity.y) < apexThreshold && !isGrounded)
            rb.AddForce(Vector3.up * -gravity * apexGravityMultiplier, ForceMode.Acceleration);
        else if (rb.linearVelocity.y < 0f)
            rb.AddForce(Vector3.up * -gravity * fallGravityMultiplier, ForceMode.Acceleration);
        else
            rb.AddForce(Vector3.up * -gravity, ForceMode.Acceleration);

        // Set max fall speed
        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            Vector3 v = rb.linearVelocity;
            v.y = -maxFallSpeed;
            rb.linearVelocity = v;
        }
    }

    private void ApplyJumpCut()
    {
        // Releasing jump while rising shortens the jump
        if (jumpCutRequested && rb.linearVelocity.y > 0f)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y *= jumpCutMultiplier;
            rb.linearVelocity = velocity;
        }

        jumpCutRequested = false;
    }
}
