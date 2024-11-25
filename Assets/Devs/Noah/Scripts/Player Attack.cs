using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerAttack : MonoBehaviour
{
    private float playerSize;
    private float knockbackStrength = 150f;
    private float cooldown = 0;
    private float cooldownValue = 1;
    private float punchLength = 5f;
    private float punchDamageCooldownValue = .5f;
    private float punchDamageCooldown = 0f;

    private Animator animator;
    private Rigidbody rb;

    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (cooldown > 0) // When a player has punched
        {
            cooldown -= Time.deltaTime;
        }

        if (punchDamageCooldown > 0) // If it's higher than 0 it will register punches with the glove
        {
            punchDamageCooldown -= Time.deltaTime;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, punchLength))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    hit.rigidbody.AddForce(new Vector3(playerController.lastMoveDirection.x * knockbackStrength, 0f, playerController.lastMoveDirection.y * knockbackStrength) , ForceMode.Impulse);
                }
            }
        }

        Debug.Log(playerController.lastMoveDirection);
        
    }

    public void OnPunch(InputAction.CallbackContext _context) // The punch input
    {
        if (_context.performed && cooldown <= 0)
        {
            animator.SetTrigger("Punch");
            cooldown = cooldownValue;
            punchDamageCooldown = punchDamageCooldownValue;
        }
    }
}
