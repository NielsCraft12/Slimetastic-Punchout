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

            // Modified knockback handling
            if (knockbackTimer > 0)
            {
                knockbackTimer -= Time.deltaTime;
                if (knockbackTimer <= 0)
                {
                    rb.velocity = Vector3.zero; // Stop knockback when timer ends
                }
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
            rb.velocity = Vector3.zero;
            rb.AddForce(_hitDirection * knockbackStrength, ForceMode.Impulse);
            playerController.SetKnockback(true);

            // Start a coroutine to reset knockback state
            StartCoroutine(ResetKnockback(_knockbackDuration));
        }
    }

    private IEnumerator ResetKnockback(float duration)
    {
        yield return new WaitForSeconds(duration);
        playerController.SetKnockback(false);
    }
}
