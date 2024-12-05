using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float airControlMultiplier = 0.5f; // Add this field - controls air movement strength

    private Vector2 moveDirection = Vector2.zero;
    public Vector2 lastMoveDirection = Vector2.zero;
    private bool isGrounded = true;

    private Animator animator;
    private GameManager gameManager;

    private bool isKnockedBack = false; // Add this field

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.drag = 0.5f;           // Remove air resistance
        rb.mass = 1f;           // Set consistent mass
        rb.angularDrag = 0.05f; // Minimal rotation resistance
        rb.useGravity = true;   // Ensure gravity is on

        animator = GetComponentInChildren<Animator>();

        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        LookAt();

        if (moveSpeed < 0)
        {
            moveSpeed = 0;
        }

        // Animation
        if (moveDirection != Vector2.zero)
        {
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    private void FixedUpdate()
    {
        if (!gameManager.isPerformingWin)
        {
            // Don't process movement if knocked back
            if (!isKnockedBack)
            {
                // Only update lastMoveDirection when moveDirection changes.
                if (moveDirection != Vector2.zero)
                {
                    // If only one axis has changed to zero, keep the last non-zero value.
                    float x = moveDirection.x != 0 ? moveDirection.x : lastMoveDirection.x;
                    float y = moveDirection.y != 0 ? moveDirection.y : lastMoveDirection.y;

                    lastMoveDirection = new Vector2(x, y).normalized;
                    moveSpeed = maxSpeed;
                }
                else if (moveSpeed > 0)
                {
                    // Gradually decrease speed if no input is detected
                    moveSpeed -= Time.deltaTime * 20;
                }

                float currentSpeed = moveSpeed * (isGrounded ? 1f : airControlMultiplier);
                // Maintain the vertical velocity to avoid interference with jumping
                float yVelocity = rb.velocity.y;

                // Apply the calculated movement direction and speed
                rb.velocity = new Vector3(
                    lastMoveDirection.x * currentSpeed,
                    yVelocity,
                    lastMoveDirection.y * currentSpeed
                );
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnJump(InputAction.CallbackContext _context)
    {
        if (isGrounded && _context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        moveDirection = _context.ReadValue<Vector2>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (moveDirection.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    // Add these methods to communicate with PlayerAttack
    public void SetKnockback(bool knocked)
    {
        isKnockedBack = knocked;
    }
}
