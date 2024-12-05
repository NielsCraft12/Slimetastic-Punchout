using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerAttack : MonoBehaviour
{
    private float playerSize;
    [SerializeField] private float knockbackStrength = 1000f;
    [SerializeField] private float knockbackDuration = 0.5f; // How long knockback lasts

    private float cooldown = 0;
    private float cooldownValue = 1;
    private float punchLength = 5f;
    private float punchDamageCooldownValue = .5f;
    private float punchDamageCooldown = 0f;

    private Animator animator;
    private Rigidbody rb;

    private PlayerController playerController;

    private Vector3 hitDirection;
    private float knockbackTimer = 0f; // Tracks remaining knockback time
    private Vector3 knockbackForce = Vector3.zero; // Tracks the knockback force to apply

    private RaycastHit raycastHit;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Handle cooldown for punches
        if (cooldown > 0) 
        {
            cooldown -= Time.deltaTime;
        }

        if (punchDamageCooldown > 0) 
        {
            punchDamageCooldown -= Time.deltaTime;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, punchLength))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    raycastHit = hit;
                    raycastHit.transform.gameObject.GetComponent<PlayerAttack>().HitKnockback(knockbackDuration, hitDirection);
                }
            }
        }

        // Apply knockback if the timer is active
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            rb.AddForce(knockbackForce, ForceMode.VelocityChange); // Apply force smoothly over time
        }
    }

    public void OnPunch(InputAction.CallbackContext _context) // The punch input
    {
        if (_context.performed && cooldown <= 0)
        {
            animator.SetTrigger("Punch");
            cooldown = cooldownValue;
            punchDamageCooldown = punchDamageCooldownValue;
            hitDirection = new Vector3(playerController.lastMoveDirection.x, 0f, playerController.lastMoveDirection.y);
        }
    }

    public void HitKnockback(float _knockbackDuration, Vector3 _hitDirection)
    {
        // Initiate knockback only if it's not already in progress
        if (knockbackTimer <= 0)
        {
            knockbackTimer = _knockbackDuration;
            knockbackForce = _hitDirection * knockbackStrength;
        }
    }
}
