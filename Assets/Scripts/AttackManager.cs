using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the Player
[RequireComponent(typeof(Rigidbody))]
public class AttackManager : MonoBehaviour
{
    public float attackCooldown = 1f;

    [Header("Special 1: Projectile")]
    public GameObject projectilePrefab; // Editor attached.
    public Transform projectileSpawnPoint; // Editor attached.
    public float projectileCooldown = 2.5f;
    public float projectileSpeed = 10f;
    private float lastProjectedTime;

    [Header("Special 2: Dash")]
    public float dashForce = 150f;
    private float lastDashTime;
    public float dashCooldown = 2.5f;

    [Header("Player")]
    private Rigidbody rb;
    private PlayerController playerController;

    // --------------------------------------------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PerformNormalAttack();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
            {
                PerformDashAttack();
            }
            else
            {
                PerformProjectileAttack();
            }
        }
    }

    private void PerformProjectileAttack()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            if (Time.time - lastProjectedTime < projectileCooldown)
            {
                return;
            }

            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            if (projectileRB)
            {
                Vector3 direction = transform.forward;
                projectileRB.velocity = direction * projectileSpeed;
                lastProjectedTime = Time.time;
            }
        }
    }

    private void PerformDashAttack()
    {
        if (Time.time - lastDashTime < dashCooldown)
        {
            return;
        }

        float direction = Input.GetAxisRaw("Horizontal");
        rb.AddForce(new Vector3(direction, 0, 0) * dashForce, ForceMode.VelocityChange);
        lastDashTime = Time.time;
        Debug.Log("Dash Attack!");
    }

    private void PerformNormalAttack()
    {
        throw new NotImplementedException();
    }
}
