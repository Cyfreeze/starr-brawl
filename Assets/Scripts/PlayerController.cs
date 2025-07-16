using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the Player
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float airMoveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float airControl = 0.5f;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public int maxJumps = 2;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.15f;

    [Header("Gravity")]
    public float fallMultiplier = 2.5f;
    public float fastFallMultiplier = 3.5f;
    public float fastFallTreshold = -5f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Visual Rotation")]
    public Transform visual; // model/mesh assigned to this in Inspector.
    public float turnSpeed = 15.0f;
    private Vector3 lastDirection = Vector3.left;

    [Header("Recovery Special")]
    public float recoverySpecialSpeed = 200f;
    public float recoverySpecialDuration = 0.2f;

    private bool canUseRecoverySpecial = true;
    private bool isRecoveryActive = false;
    private float recoveryTimer = 0f;

    // --------------------------------------------------------------------

    private Rigidbody rb;
    private Vector3 input;
    private bool jumpPressed;
    private bool isGrounded;
    private int jumpsLeft;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    // --------------------------------------------------------------------

    public bool IsGrounded()
    {
        return isGrounded;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Input handling
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0).normalized;

        // I: Recovery Special
        if (Input.GetKey(KeyCode.I) && canUseRecoverySpecial && !isRecoveryActive)
        {
            StartRecoverySpecial();
        }

        // Jump logic
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        jumpBufferCounter -= Time.deltaTime;

        // Fast fall
        if (Input.GetKey(KeyCode.S) && rb.velocity.y < 0)
        {
            rb.velocity += (fastFallMultiplier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
    }

    void FixedUpdate()
    {
        CheckGround();

        if (HandleRecoverySpecial()) return;

        HandleMovement();
        HandleJump();
        HandleSmoothRotation();
        HandleGravityTweaks();
        HandleCoyoting();
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
    }

    void HandleMovement()
    {
        Vector3 moveDir = new(input.x, 0, 0);

        float targetSpeed = isGrounded ? moveSpeed : airMoveSpeed;

        Vector3 targetVelocity = moveDir * targetSpeed;
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, 0);

        float control = isGrounded ? 1f : airControl;
        velocityChange = Vector3.ClampMagnitude(velocityChange, acceleration * control * Time.deltaTime);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void HandleJump()
    {
        if (jumpBufferCounter > 0 && (isGrounded || coyoteTimeCounter > 0f || jumpsLeft > 0))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            if (!isGrounded)
            {
                jumpsLeft--;
            }
        }
    }

    void HandleSmoothRotation()
    {
        // Only rotate if there is horizontal input.
        if (input.x != 0)
        {
            lastDirection = new Vector3(input.x, 0, 0); // Save the last horizontal facing direction
        }

        // Smoothly rotate visual toward movement direction
        if (visual != null && lastDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection, Vector3.up);
            visual.rotation = Quaternion.Slerp(visual.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }
    }

    void HandleGravityTweaks()
    {
        if (rb.velocity.y < 0)
        {
            // Vector3: (X, Y, Z)
            // Vector3.up: (0, 1, 0)
            // Pyysics.gravity.y = -9.81
            rb.velocity += (fallMultiplier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
    }

    void HandleCoyoting()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpsLeft = maxJumps - 1;
        }
        else // Is not grounded
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    #region Recovery Special --------------------------------------------------------------------
    private bool HandleRecoverySpecial()
    {
        if (isRecoveryActive)
        {
            recoveryTimer -= Time.fixedDeltaTime;
            if (recoveryTimer <= 0)
            {
                EndRecoverySpecial();
            }
            else
            {
                rb.velocity = new(lastDirection.x * recoverySpecialSpeed, recoverySpecialSpeed, 0);
                return true;
            }
        }

        return false;
    }

    void StartRecoverySpecial()
    {
        isRecoveryActive = true;
        canUseRecoverySpecial = false;
        recoveryTimer = recoverySpecialDuration;

        // Todo: Add any effects/animations
    }

    void EndRecoverySpecial()
    {
        isRecoveryActive = false;
        StartCoroutine(EnableRecoverySpecialAfterDelay());
    }

    IEnumerator EnableRecoverySpecialAfterDelay()
    {
        yield return new WaitUntil(() => isGrounded);
        canUseRecoverySpecial = true;
    }

    #endregion // --------------------------------------------------------------------

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
