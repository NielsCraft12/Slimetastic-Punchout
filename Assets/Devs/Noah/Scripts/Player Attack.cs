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

    private GameManager gameManager;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!gameManager.isPerformingWin)
        {
            // Handle cooldown for punches
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }

            if (punchDamageCooldown > 0)
            {
                punchDamageCooldown -= Time.deltaTime;

                // Debug ray visualization
                Debug.DrawRay(transform.position, transform.forward * punchLength, Color.red);

                RaycastHit hit;
                // Use a bigger radius for better hit detection
                if (Physics.SphereCast(transform.position, 1f, transform.forward, out hit, punchLength))
                {
                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        // Calculate direction from puncher to target
                        Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;
                        knockbackDir.y = 0; // Keep knockback horizontal
                        hit.transform.gameObject.GetComponent<PlayerAttack>().HitKnockback(knockbackDuration, knockbackDir);
                    }
                }
            }

            // Remove the knockback timer velocity reset
            if (knockbackTimer > 0)
            {
                knockbackTimer -= Time.deltaTime;
            }
        }
    }

    public void OnPunch(InputAction.CallbackContext _context) // The punch input
    {
        if (_context.performed && cooldown <= 0)
        {
            animator.SetTrigger("Punch");
            cooldown = cooldownValue;
            punchDamageCooldown = punchDamageCooldownValue;
            hitDirection = transform.forward; // Use actual forward direction instead of lastMoveDirection
        }
    }

    public void HitKnockback(float _knockbackDuration, Vector3 _hitDirection)
    {
        if (knockbackTimer <= 0)
        {
            knockbackTimer = _knockbackDuration;
            Vector3 knockbackForce = _hitDirection * knockbackStrength + Vector3.up * knockbackStrength * 0.5f;
            rb.AddForce(knockbackForce, ForceMode.Impulse);
            playerController.SetKnockback(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            knockbackTimer = 0;
            playerController.SetKnockback(false);
        }
    }
}
