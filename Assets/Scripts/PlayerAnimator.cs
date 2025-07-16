using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the Player
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Walking animation
        bool isWalking = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f && playerController.IsGrounded();
        animator.SetBool("IsWalking", isWalking);

        // Jumping animation
        bool isJumping = !playerController.IsGrounded();
        animator.SetBool("IsJumping", isJumping);
    }
}
